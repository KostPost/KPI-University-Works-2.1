using Npgsql;

namespace Laba3;

  public class DenseIndex
    {
        private const string connectionString = 
            "Host=localhost;Port=5432;Database=homework;Username=postgres;Password=2025";
        private readonly int reconstructThreshold;
        private List<Record> indexArea;

        public DenseIndex(int threshold = 1000)
        {
            reconstructThreshold = threshold;
            indexArea = new List<Record>();
            CreateTableIfNotExists();
            LoadIndexFromDB();
        }

        private NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(connectionString);
        }

        private void CreateTableIfNotExists()
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"
                        CREATE TABLE IF NOT EXISTS records (
                            key_id INTEGER PRIMARY KEY,
                            data_value TEXT NOT NULL
                        );
                        
                        CREATE TABLE IF NOT EXISTS index_area (
                            key_id INTEGER PRIMARY KEY,
                            is_active BOOLEAN DEFAULT true
                        );";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void LoadIndexFromDB()
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(
                    "SELECT r.key_id, r.data_value FROM records r " +
                    "INNER JOIN index_area i ON r.key_id = i.key_id " +
                    "WHERE i.is_active = true ORDER BY r.key_id", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            indexArea.Add(new Record(
                                reader.GetInt32(0),
                                reader.GetString(1)
                            ));
                        }
                    }
                }
            }
        }

        public Record Search(int key)
        {
            int left = 0;
            int right = indexArea.Count - 1;

            while (left <= right)
            {
                int mid = (left + right) / 2;
                
                if (indexArea[mid].Key == key)
                    return indexArea[mid];
                
                if (indexArea[mid].Key < key)
                    left = mid + 1;
                else
                    right = mid - 1;
            }

            return null;
        }

        public void Insert(int key, string data)
        {
            // Проверка на существующий ключ
            if (Search(key) != null)
                throw new Exception("Ключ уже существует");

            using (var conn = GetConnection())
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Вставка в таблицу records
                        using (var cmd = new NpgsqlCommand(
                            "INSERT INTO records (key_id, data_value) VALUES (@key, @data)", conn))
                        {
                            cmd.Parameters.AddWithValue("key", key);
                            cmd.Parameters.AddWithValue("data", data);
                            cmd.ExecuteNonQuery();
                        }

                        // Вставка в index_area
                        using (var cmd = new NpgsqlCommand(
                            "INSERT INTO index_area (key_id) VALUES (@key)", conn))
                        {
                            cmd.Parameters.AddWithValue("key", key);
                            cmd.ExecuteNonQuery();
                        }

                        var record = new Record(key, data);
                        int insertIndex = indexArea.BinarySearch(record, 
                            Comparer<Record>.Create((x, y) => x.Key.CompareTo(y.Key)));
                        
                        if (insertIndex < 0)
                            insertIndex = ~insertIndex;
                        
                        indexArea.Insert(insertIndex, record);

                        if (NeedsReconstruction())
                            ReconstructIndexArea(conn);

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        private bool NeedsReconstruction()
        {
            return indexArea.Count >= reconstructThreshold;
        }

        private void ReconstructIndexArea(NpgsqlConnection conn)
        {
            // Реализация метода Шарра для перестроения
            var newIndexArea = new List<Record>();
            int step = (int)Math.Sqrt(indexArea.Count);
            
            using (var cmd = new NpgsqlCommand("UPDATE index_area SET is_active = false", conn))
            {
                cmd.ExecuteNonQuery();
            }

            for (int i = 0; i < indexArea.Count; i += step)
            {
                var record = indexArea[i];
                using (var cmd = new NpgsqlCommand(
                    "UPDATE index_area SET is_active = true WHERE key_id = @key", conn))
                {
                    cmd.Parameters.AddWithValue("key", record.Key);
                    cmd.ExecuteNonQuery();
                }
                newIndexArea.Add(record);
            }

            indexArea = newIndexArea;
        }

        public void Delete(int key)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (var cmd = new NpgsqlCommand(
                            "DELETE FROM index_area WHERE key_id = @key;" +
                            "DELETE FROM records WHERE key_id = @key;", conn))
                        {
                            cmd.Parameters.AddWithValue("key", key);
                            var rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected == 0)
                                throw new Exception("Запись не найдена");
                        }

                        var record = Search(key);
                        if (record != null)
                            indexArea.Remove(record);

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Update(int key, string newData)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        using (var cmd = new NpgsqlCommand(
                            "UPDATE records SET data_value = @data WHERE key_id = @key", conn))
                        {
                            cmd.Parameters.AddWithValue("key", key);
                            cmd.Parameters.AddWithValue("data", newData);
                            var rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected == 0)
                                throw new Exception("Запись не найдена");
                        }

                        var record = Search(key);
                        if (record != null)
                            record.Data = newData;

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }