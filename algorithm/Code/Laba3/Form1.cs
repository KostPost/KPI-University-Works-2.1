namespace Laba3
{
    public partial class Form1 : Form
    {
        private DenseIndex db;
        private Random random = new Random();

        // Элементы управления
        private TextBox txtKey;
        private TextBox txtData;
        private TextBox txtResult;
        private Button btnSearch;
        private Button btnInsert;
        private Button btnDelete;
        private Button btnUpdate;
        private Button btnGenerateData;
        private Button btnTestSearch;
        private DataGridView gridSearchStats;
        private Label lblKey;
        private Label lblData;
        private Label lblResult;

        public Form1()
        {
            InitializeComponent();
            InitializeDatabase();
            InitializeCustomComponents();
        }

        private void InitializeDatabase()
        {
            try
            {
                db = new DenseIndex();
                MessageBox.Show("База данных успешно инициализирована", "Успех", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при инициализации базы данных: {ex.Message}", 
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeCustomComponents()
        {
            this.Text = "Dense Index Database";
            this.Size = new Size(800, 600);

            // Labels
            lblKey = new Label
            {
                Location = new Point(20, 20),
                Size = new Size(70, 20),
                Text = "Ключ:"
            };

            lblData = new Label
            {
                Location = new Point(20, 50),
                Size = new Size(70, 20),
                Text = "Данные:"
            };

            lblResult = new Label
            {
                Location = new Point(20, 80),
                Size = new Size(70, 20),
                Text = "Результат:"
            };

            // TextBoxes
            txtKey = new TextBox
            {
                Location = new Point(100, 20),
                Size = new Size(100, 20)
            };

            txtData = new TextBox
            {
                Location = new Point(100, 50),
                Size = new Size(200, 20)
            };

            txtResult = new TextBox
            {
                Location = new Point(100, 80),
                Size = new Size(400, 100),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };

            // Buttons
            btnSearch = new Button
            {
                Location = new Point(20, 200),
                Size = new Size(100, 30),
                Text = "Поиск"
            };
            btnSearch.Click += btnSearch_Click;

            btnInsert = new Button
            {
                Location = new Point(130, 200),
                Size = new Size(100, 30),
                Text = "Вставить"
            };
            btnInsert.Click += btnInsert_Click;

            btnDelete = new Button
            {
                Location = new Point(240, 200),
                Size = new Size(100, 30),
                Text = "Удалить"
            };
            btnDelete.Click += btnDelete_Click;

            btnUpdate = new Button
            {
                Location = new Point(350, 200),
                Size = new Size(100, 30),
                Text = "Обновить"
            };
            btnUpdate.Click += btnUpdate_Click;

            btnGenerateData = new Button
            {
                Location = new Point(20, 240),
                Size = new Size(150, 30),
                Text = "Генерация данных"
            };
            btnGenerateData.Click += btnGenerateData_Click;

            btnTestSearch = new Button
            {
                Location = new Point(180, 240),
                Size = new Size(150, 30),
                Text = "Тест поиска"
            };
            btnTestSearch.Click += btnTestSearch_Click;

            // Таблица статистики
            gridSearchStats = new DataGridView
            {
                Location = new Point(20, 280),
                Size = new Size(700, 200),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true
            };
            gridSearchStats.Columns.Add("AttemptNumber", "Номер попытки");
            gridSearchStats.Columns.Add("Comparisons", "Число сравнений");
            gridSearchStats.Columns.Add("Key", "Ключ");
            gridSearchStats.Columns.Add("Result", "Результат");

            // Добавляем элементы на форму
            this.Controls.AddRange(new Control[] {
                lblKey, lblData, lblResult,
                txtKey, txtData, txtResult,
                btnSearch, btnInsert, btnDelete, btnUpdate,
                btnGenerateData, btnTestSearch,
                gridSearchStats
            });
        }

        private void UpdateSearchStatsGrid()
        {
            gridSearchStats.Rows.Clear();
            var history = db.GetSearchHistory();
            foreach (var stat in history)
            {
                gridSearchStats.Rows.Add(
                    stat.AttemptNumber,
                    stat.Comparisons,
                    stat.Key,
                    stat.Found ? "Найдено" : "Не найдено"
                );
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtKey.Text, out int key))
            {
                try
                {
                    var (record, stats) = db.Search(key);
                    txtResult.Text = record != null
                        ? $"Найдено: {record.Data}\n" +
                          $"Количество сравнений: {stats.Comparisons}"
                        : $"Не найдено\n" +
                          $"Количество сравнений: {stats.Comparisons}";
                    UpdateSearchStatsGrid();
                }
                catch (Exception ex)
                {
                    txtResult.Text = $"Ошибка поиска: {ex.Message}";
                }
            }
            else
            {
                txtResult.Text = "Некорректный формат ключа";
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtKey.Text, out int key))
            {
                try
                {
                    db.Insert(key, txtData.Text);
                    txtResult.Text = "Запись успешно добавлена";
                }
                catch (Exception ex)
                {
                    txtResult.Text = $"Ошибка вставки: {ex.Message}";
                }
            }
            else
            {
                txtResult.Text = "Некорректный формат ключа";
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtKey.Text, out int key))
            {
                try
                {
                    db.Delete(key);
                    txtResult.Text = "Запись успешно удалена";
                }
                catch (Exception ex)
                {
                    txtResult.Text = $"Ошибка удаления: {ex.Message}";
                }
            }
            else
            {
                txtResult.Text = "Некорректный формат ключа";
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtKey.Text, out int key))
            {
                try
                {
                    db.Update(key, txtData.Text);
                    txtResult.Text = "Запись успешно обновлена";
                }
                catch (Exception ex)
                {
                    txtResult.Text = $"Ошибка обновления: {ex.Message}";
                }
            }
            else
            {
                txtResult.Text = "Некорректный формат ключа";
            }
        }

        private void btnGenerateData_Click(object sender, EventArgs e)
        {
            try
            {
                int successCount = 0;
                for (int i = 0; i < 10000; i++)
                {
                    try
                    {
                        int key = random.Next(1, 100000);
                        string data = $"Data_{key}";
                        db.Insert(key, data);
                        successCount++;
                    }
                    catch
                    {
                        // Пропускаем дубликаты ключей
                    }
                }
                txtResult.Text = $"Сгенерировано {successCount} случайных записей";
            }
            catch (Exception ex)
            {
                txtResult.Text = $"Ошибка генерации данных: {ex.Message}";
            }
        }

        private void btnTestSearch_Click(object sender, EventArgs e)
        {
            try
            {
                int tests = 15;
                int successfulSearches = 0;
                List<double> searchTimes = new List<double>();

                for (int i = 0; i < tests; i++)
                {
                    int key = random.Next(1, 100000);
                    var startTime = DateTime.Now;
                    var (record, stats) = db.Search(key);
                    var endTime = DateTime.Now;
                    if (record != null)
                        successfulSearches++;
                    searchTimes.Add((endTime - startTime).TotalMilliseconds);
                }

                UpdateSearchStatsGrid();

                double avgTime = searchTimes.Average();
                txtResult.Text = $"Выполнено {tests} поисков\n" +
                               $"Успешных поисков: {successfulSearches}\n" +
                               $"Среднее время поиска: {avgTime:F2} мс";
            }
            catch (Exception ex)
            {
                txtResult.Text = $"Ошибка тестирования: {ex.Message}";
            }
        }
    }
}