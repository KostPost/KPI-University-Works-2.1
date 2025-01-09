 class Solution
    {
        private int[] colors;           // Масив кольорів для кожної вершини
        private Graph graph;            // Посилання на граф
        private Random random;          // Генератор випадкових чисел
        public double Fitness { get; private set; }  // Якість розв'язку

        // Конструктор для створення випадкового розв'язку
        public Solution(Graph graph, int maxColors)
        {
            this.graph = graph;
            random = new Random();
            colors = new int[graph.GetVertexCount()];

            // Призначаємо випадкові кольори
            for (int i = 0; i < graph.GetVertexCount(); i++)
            {
                colors[i] = random.Next(maxColors);
            }

            CalculateFitness();
        }

        // Копіювання розв'язку
        public Solution Clone()
        {
            Solution newSolution = new Solution(graph, 0);
            newSolution.colors = (int[])colors.Clone();
            newSolution.Fitness = this.Fitness;
            return newSolution;
        }

        // Розрахунок якості розв'язку
        private void CalculateFitness()
        {
            int conflicts = 0;
            
            // Перевіряємо кожну вершину
            for (int i = 0; i < graph.GetVertexCount(); i++)
            {
                // Перевіряємо всіх сусідів
                foreach (int neighbor in graph.GetNeighbors(i))
                {
                    // Якщо кольори однакові - маємо конфлікт
                    if (colors[i] == colors[neighbor])
                    {
                        conflicts++;
                    }
                }
            }

            // Розраховуємо якість (чим менше конфліктів, тим краще)
            Fitness = 1.0 / (1.0 + conflicts);
        }

        // Локальний пошук - намагаємося покращити розв'язок
        public void LocalSearch()
        {
            // Вибираємо випадкову вершину
            int vertex = random.Next(graph.GetVertexCount());
            int oldColor = colors[vertex];
            
            // Пробуємо всі можливі кольори
            for (int newColor = 0; newColor < colors.Max() + 1; newColor++)
            {
                if (newColor != oldColor)
                {
                    colors[vertex] = newColor;
                    double newFitness = CalculateNewFitness();
                    
                    // Якщо знайшли кращий колір - залишаємо його
                    if (newFitness > Fitness)
                    {
                        Fitness = newFitness;
                        return;
                    }
                }
            }
            
            // Якщо кращого кольору не знайшли - повертаємо старий
            colors[vertex] = oldColor;
        }

        // Розрахунок нової якості
        private double CalculateNewFitness()
        {
            int conflicts = 0;
            for (int i = 0; i < graph.GetVertexCount(); i++)
            {
                foreach (int neighbor in graph.GetNeighbors(i))
                {
                    if (colors[i] == colors[neighbor])
                    {
                        conflicts++;
                    }
                }
            }
            return 1.0 / (1.0 + conflicts);
        }
    }