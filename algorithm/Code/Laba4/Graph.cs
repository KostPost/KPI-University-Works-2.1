class Graph
{
    private List<List<int>> adjacencyList;  // Список суміжності графа
    private int vertexCount;                // Кількість вершин
    private Random random;                  // Генератор випадкових чисел

    // Конструктор для створення випадкового графа
    public Graph(int vertices, int maxDegree)
    {
        vertexCount = vertices;
        adjacencyList = new List<List<int>>();
        random = new Random();

        // Створюємо порожні списки для кожної вершини
        for (int i = 0; i < vertices; i++)
        {
            adjacencyList.Add(new List<int>());
        }

        // Додаємо випадкові ребра
        for (int i = 0; i < vertices; i++)
        {
            // Кожній вершині призначаємо від 1 до maxDegree ребер
            int edgesToAdd = random.Next(1, maxDegree + 1);
                
            while (adjacencyList[i].Count < edgesToAdd)
            {
                int neighbor = random.Next(vertices);
                    
                // Перевіряємо чи можна додати ребро
                if (neighbor != i && 
                    !adjacencyList[i].Contains(neighbor) && 
                    adjacencyList[neighbor].Count < maxDegree)
                {
                    adjacencyList[i].Add(neighbor);
                    adjacencyList[neighbor].Add(i);
                }
            }
        }
    }

    // Отримати список сусідів вершини
    public List<int> GetNeighbors(int vertex)
    {
        return adjacencyList[vertex];
    }

    // Отримати кількість вершин
    public int GetVertexCount()
    {
        return vertexCount;
    }
}