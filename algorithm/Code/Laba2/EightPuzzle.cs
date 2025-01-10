namespace Laba2;

class EightPuzzle
{
    private int[,] initialState;
    private int[,] goalState;
    private readonly (int, int)[] moves = { (-1, 0), (1, 0), (0, -1), (0, 1) }; // Up, Down, Left, Right
    private DateTime startTime;
    private const int TIME_LIMIT_MINUTES = 30;
    private const long MEMORY_LIMIT_BYTES = 1L * 1024 * 1024 * 1024; // 1GB
    public SearchStatistics Statistics { get; private set; }

    public EightPuzzle(int[,] initial, int[,] goal)
    {
        initialState = initial;
        goalState = goal;
        Statistics = new SearchStatistics();
    }

    private bool IsTimeLimitExceeded()
    {
        return (DateTime.Now - startTime).TotalMinutes > TIME_LIMIT_MINUTES;
    }

    private bool IsMemoryLimitExceeded()
    {
        long memoryUsed = GC.GetTotalMemory(false);
        return memoryUsed > MEMORY_LIMIT_BYTES;
    }

    // LDFS Implementation
    public List<int[,]> SolveLDFS(int maxDepth)
    {
        Statistics = new SearchStatistics();
        startTime = DateTime.Now;

        HashSet<string> visited = new HashSet<string>();
        var result = LDFS(initialState, maxDepth, visited, new List<int[,]>());

        Statistics.ExecutionTime = DateTime.Now - startTime;
        return result;
    }

    private List<int[,]> LDFS(int[,] currentState, int depth, HashSet<string> visited, List<int[,]> path)
    {
        if (IsTimeLimitExceeded())
        {
            Statistics.TimeLimitExceeded = true;
            return null;
        }

        if (IsMemoryLimitExceeded())
        {
            Statistics.MemoryLimitExceeded = true;
            return null;
        }

        if (depth < 0)
        {
            Statistics.DeadEnds++;
            return null;
        }

        string stateStr = StateToString(currentState);
        if (visited.Contains(stateStr))
        {
            Statistics.DeadEnds++;
            return null;
        }

        Statistics.GeneratedStates++;
        visited.Add(stateStr);
        path.Add(currentState);

        Statistics.StatesInMemory = Math.Max(Statistics.StatesInMemory, visited.Count);

        if (IsGoalState(currentState))
        {
            Statistics.Steps = path.Count - 1;
            return new List<int[,]>(path);
        }

        var (emptyRow, emptyCol) = FindEmptyCell(currentState);

        foreach (var (dx, dy) in moves)
        {
            int newRow = emptyRow + dx;
            int newCol = emptyCol + dy;

            if (IsValidPosition(newRow, newCol))
            {
                var newState = CreateNewState(currentState, emptyRow, emptyCol, newRow, newCol);
                var result = LDFS(newState, depth - 1, visited, path);

                if (result != null)
                {
                    return result;
                }
            }
        }

        path.RemoveAt(path.Count - 1);
        visited.Remove(stateStr);
        return null;
    }

    // RBFS Implementation
    public List<int[,]> SolveRBFS(int fLimit)
    {
        Statistics = new SearchStatistics();
        startTime = DateTime.Now;

        var path = new List<int[,]>();
        var result = RBFS(initialState, 0, fLimit, path);

        Statistics.ExecutionTime = DateTime.Now - startTime;
        return result.success ? path : null;
    }

    private (bool success, int fMin) RBFS(int[,] currentState, int g, int fLimit, List<int[,]> path)
    {
        if (IsTimeLimitExceeded())
        {
            Statistics.TimeLimitExceeded = true;
            return (false, int.MaxValue);
        }

        if (IsMemoryLimitExceeded())
        {
            Statistics.MemoryLimitExceeded = true;
            return (false, int.MaxValue);
        }

        Statistics.GeneratedStates++;
        path.Add(currentState);
        Statistics.StatesInMemory = Math.Max(Statistics.StatesInMemory, path.Count);

        if (IsGoalState(currentState))
        {
            Statistics.Steps = path.Count - 1;
            return (true, g);
        }

        var (emptyRow, emptyCol) = FindEmptyCell(currentState);
        var successors = new List<(int[,] state, int f)>();

        foreach (var (dx, dy) in moves)
        {
            int newRow = emptyRow + dx;
            int newCol = emptyCol + dy;

            if (IsValidPosition(newRow, newCol))
            {
                var newState = CreateNewState(currentState, emptyRow, emptyCol, newRow, newCol);
                int h = CalculateHeuristic(newState);
                successors.Add((newState, g + 1 + h));
            }
        }

        if (!successors.Any())
        {
            Statistics.DeadEnds++;
            path.RemoveAt(path.Count - 1);
            return (false, int.MaxValue);
        }

        while (true)
        {
            successors = successors.OrderBy(x => x.f).ToList();
            var best = successors[0];

            if (best.f > fLimit)
            {
                Statistics.DeadEnds++;
                path.RemoveAt(path.Count - 1);
                return (false, best.f);
            }

            int alternative = successors.Count > 1 ? successors[1].f : int.MaxValue;
            var result = RBFS(best.state, g + 1, Math.Min(fLimit, alternative), path);

            if (result.success)
            {
                return result;
            }

            successors[0] = (best.state, result.fMin);
        }
    }

    // H1 heuristic - number of misplaced tiles
    private int CalculateHeuristic(int[,] state)
    {
        int misplaced = 0;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (state[i, j] != 0 && state[i, j] != goalState[i, j])
                {
                    misplaced++;
                }
            }
        }

        return misplaced;
    }

    private bool IsGoalState(int[,] state)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (state[i, j] != goalState[i, j])
                {
                    return false;
                }
            }
        }

        return true;
    }

    private (int, int) FindEmptyCell(int[,] state)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (state[i, j] == 0)
                {
                    return (i, j);
                }
            }
        }

        throw new Exception("Empty cell not found");
    }

    private bool IsValidPosition(int row, int col)
    {
        return row >= 0 && row < 3 && col >= 0 && col < 3;
    }

    private int[,] CreateNewState(int[,] currentState, int emptyRow, int emptyCol, int newRow, int newCol)
    {
        int[,] newState = new int[3, 3];
        Array.Copy(currentState, newState, 9);

        newState[emptyRow, emptyCol] = currentState[newRow, newCol];
        newState[newRow, newCol] = 0;

        return newState;
    }

    private string StateToString(int[,] state)
    {
        return string.Join("", Enumerable.Range(0, 3)
            .SelectMany(i => Enumerable.Range(0, 3)
                .Select(j => state[i, j].ToString())));
    }
}