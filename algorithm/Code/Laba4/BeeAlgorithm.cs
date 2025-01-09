namespace GraphColoring;

 class BeeAlgorithm
    {
        private List<Solution> solutions;        // Всі поточні розв'язки
        private Graph graph;                     // Граф, який розфарбовуємо
        private int beesCount;                   // Загальна кількість бджіл
        private int scoutBeesCount;              // Кількість бджіл-розвідників
        private int maxColors;                   // Максимальна кількість кольорів
        public List<double> BestFitnessHistory { get; private set; }  // Історія найкращих значень

        // Конструктор алгоритму
        public BeeAlgorithm(int vertices, int maxDegree, int bees, int scouts, int colors)
        {
            graph = new Graph(vertices, maxDegree);
            beesCount = bees;
            scoutBeesCount = scouts;
            maxColors = colors;
            solutions = new List<Solution>();
            BestFitnessHistory = new List<double>();

            // Створюємо початкові розв'язки
            for (int i = 0; i < beesCount; i++)
            {
                solutions.Add(new Solution(graph, maxColors));
            }
        }

        // Запуск алгоритму
        public void Run(int iterations)
        {
            for (int i = 0; i <= iterations; i++)
            {
                // Сортуємо розв'язки за якістю
                solutions = solutions.OrderByDescending(s => s.Fitness).ToList();

                // Зберігаємо історію кожні 20 ітерацій
                if (i % 20 == 0)
                {
                    BestFitnessHistory.Add(solutions[0].Fitness);
                    Console.WriteLine($"Iteration {i}: Best fitness = {solutions[0].Fitness}");
                }

                List<Solution> newSolutions = new List<Solution>();

                // Розвідники шукають нові розв'язки
                for (int j = 0; j < scoutBeesCount; j++)
                {
                    newSolutions.Add(new Solution(graph, maxColors));
                }

                // Робочі бджоли покращують існуючі розв'язки
                for (int j = 0; j < beesCount - scoutBeesCount; j++)
                {
                    Solution clone = solutions[j % solutions.Count].Clone();
                    clone.LocalSearch();
                    newSolutions.Add(clone);
                }

                solutions = newSolutions;
            }
        }
    }