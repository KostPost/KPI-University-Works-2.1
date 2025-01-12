using System.Data;
using Npgsql;

namespace Laba3;

public class DataService : IDisposable
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=homework;Username=postgres;Password=2025";

    public NpgsqlConnection? connection;
    private int comparisonsCount;
    public void InitializeDatabase()
    {
        try
        {
            connection = new NpgsqlConnection(ConnectionString);
            connection.Open();
            CreateTablesIfNotExist();
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to initialize database", ex);
        }
    }


    private void CreateTablesIfNotExist()
    {
        if (connection == null) throw new InvalidOperationException("Database connection not initialized");

        using var cmd = new NpgsqlCommand(@"
            CREATE TABLE IF NOT EXISTS data_records (
                key_id INTEGER PRIMARY KEY,
                data_value VARCHAR(255)
            );
            
            CREATE TABLE IF NOT EXISTS dense_index (
                key_id INTEGER PRIMARY KEY,
                record_pointer INTEGER,
                FOREIGN KEY (record_pointer) REFERENCES data_records(key_id)
            );", connection);

        cmd.ExecuteNonQuery();
    }

    public void Dispose()
    {
        if (connection != null)
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }

            connection.Dispose();
            connection = null;
        }
    }

    public DataRecord Search(int key)
    {
        using (var cmd = new NpgsqlCommand())
        {
            cmd.Connection = connection;
            cmd.CommandText = @"
                    SELECT d.key_id, d.data_value 
                    FROM data_records d
                    INNER JOIN dense_index i ON d.key_id = i.record_pointer
                    WHERE d.key_id = @key";
            cmd.Parameters.AddWithValue("@key", key);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new DataRecord
                    {
                        Key = reader.GetInt32(0),
                        Data = reader.GetString(1)
                    };
                }
            }
        }

        return null;
    }

    public void Insert(int key, string data)
    {
        using (var transaction = connection.BeginTransaction())
        {
            try
            {
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.Transaction = transaction;
                    cmd.CommandText = "INSERT INTO data_records (key_id, data_value) VALUES (@key, @data)";
                    cmd.Parameters.AddWithValue("@key", key);
                    cmd.Parameters.AddWithValue("@data", data);
                    cmd.ExecuteNonQuery();
                }

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.Transaction = transaction;
                    cmd.CommandText = "INSERT INTO dense_index (key_id, record_pointer) VALUES (@key, @key)";
                    cmd.Parameters.AddWithValue("@key", key);
                    cmd.ExecuteNonQuery();
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }

    public void Delete(int key)
    {
        using (var transaction = connection.BeginTransaction())
        {
            try
            {
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.Transaction = transaction;
                    cmd.CommandText = "DELETE FROM dense_index WHERE key_id = @key";
                    cmd.Parameters.AddWithValue("@key", key);
                    cmd.ExecuteNonQuery();
                }

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.Transaction = transaction;
                    cmd.CommandText = "DELETE FROM data_records WHERE key_id = @key";
                    cmd.Parameters.AddWithValue("@key", key);
                    cmd.ExecuteNonQuery();
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }

    public void Update(int key, string newData)
    {
        using (var cmd = new NpgsqlCommand())
        {
            cmd.Connection = connection;
            cmd.CommandText = "UPDATE data_records SET data_value = @data WHERE key_id = @key";
            cmd.Parameters.AddWithValue("@key", key);
            cmd.Parameters.AddWithValue("@data", newData);
            cmd.ExecuteNonQuery();
        }
    }

    public void GenerateRandomData(int count)
    {
        Random random = new Random();
        for (int i = 0; i < count; i++)
        {
            int key = random.Next(1, 100000);
            string data = $"Data_{random.Next(1000000)}";
            try
            {
                Insert(key, data);
            }
            catch (Exception)
            {
                i--;
            }
        }
    }

    public double MeasureAverageComparisons()
    {
        Random random = new Random();
        int numberOfSearches = 15;
        int totalComparisons = 0;

        for (int i = 0; i < numberOfSearches; i++)
        {
            int randomKey = random.Next(1, 100000);
            var startTime = DateTime.Now;
            Search(randomKey);
            var endTime = DateTime.Now;
            totalComparisons += (endTime - startTime).Milliseconds;
        }

        return totalComparisons / (double)numberOfSearches;
    }


    public DataTable GetAllRecords(int limit = 1000)
    {
        if (connection == null) throw new InvalidOperationException("Database connection not initialized");

        var table = new DataTable();

        using var command = new NpgsqlCommand(@"
            SELECT d.key_id as ""Key"", 
                   d.data_value as ""Data""
            FROM data_records d
            INNER JOIN dense_index i ON d.key_id = i.record_pointer
            ORDER BY d.key_id
            LIMIT @limit", connection);

        command.Parameters.AddWithValue("@limit", limit);

        using var adapter = new NpgsqlDataAdapter(command);
        adapter.Fill(table);

        return table;
    }
    
    
    
    public List<SearchAttempt> MeasureSearchComparisons(int numberOfAttempts = 15)
    {
        var random = new Random();
        var results = new List<SearchAttempt>();
        
        var existingKeys = GetExistingKeys();
        
        for (int i = 0; i < numberOfAttempts; i++)
        {
            var randomKey = existingKeys[random.Next(existingKeys.Count)];
            comparisonsCount = 0;
            
            SearchWithComparisons(randomKey);
            
            results.Add(new SearchAttempt 
            { 
                AttemptNumber = i + 1,
                Comparisons = comparisonsCount,
                SearchedKey = randomKey
            });
        }

        return results;
    }
    
    
    private DataRecord? SearchWithComparisons(int key)
    {
        comparisonsCount = 0;
        if (connection == null) throw new InvalidOperationException("Database connection not initialized");

        using var command = new NpgsqlCommand(@"
        SELECT d.key_id, d.data_value
        FROM data_records d
        INNER JOIN dense_index i ON d.key_id = i.record_pointer
        WHERE d.key_id = @key", connection);

        command.Parameters.AddWithValue("@key", key);

        // Реалізація бінарного пошуку
        var allKeys = GetExistingKeys();
        int left = 0;
        int right = allKeys.Count - 1;

        while (left <= right)
        {
            comparisonsCount++; // Збільшуємо лічильник порівнянь
            int mid = (left + right) / 2;
        
            if (allKeys[mid] == key)
            {
                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new DataRecord
                    {
                        Key = reader.GetInt32(0),
                        Data = reader.GetString(1)
                    };
                }
            }
        
            if (allKeys[mid] < key)
                left = mid + 1;
            else
                right = mid - 1;
        }

        return null;
    }
    
    private List<int> GetExistingKeys()
    {
        var keys = new List<int>();
        if (connection == null) throw new InvalidOperationException("Database connection not initialized");

        using var command = new NpgsqlCommand("SELECT key_id FROM data_records ORDER BY key_id", connection);
        using var reader = command.ExecuteReader();
        
        while (reader.Read())
        {
            keys.Add(reader.GetInt32(0));
        }
        
        return keys;
    }

    public DataTable GetSearchComparisonTable(int attempts = 15)
    {
        var results = MeasureSearchComparisons(attempts);
        var table = new DataTable();
    
        // Визначаємо колонки з правильними типами
        table.Columns.Add("Номер спроби пошуку", typeof(string));
        table.Columns.Add("Число порівнянь", typeof(int));  // змінюємо назад на int
        table.Columns.Add("Шуканий ключ", typeof(string));
    
        // Додаємо результати пошуку
        foreach (var result in results)
        {
            table.Rows.Add(
                result.AttemptNumber.ToString(),
                result.Comparisons,  // залишаємо як int
                result.SearchedKey.ToString()
            );
        }

        // Обчислюємо середнє значення
        int avgComparisons = (int)Math.Round(results.Average(r => r.Comparisons));
    
        // Додаємо рядок із середнім значенням
        table.Rows.Add("Середнє", avgComparisons, "-");
    
        return table;
    }
    
}