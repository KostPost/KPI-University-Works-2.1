namespace Laba2;

class EightPuzzle
{
    private int[,] initialState;
    private int[,] goalState;
    private readonly (int, int)[] moves = { (-1, 0), (1, 0), (0, -1), (0, 1) }; // Вверх, вниз, влево, вправо

    
    
    public EightPuzzle(int[,] initial, int[,] goal)
    {
        initialState = initial;
        goalState = goal;
    }

    
    public List<int[,]> SolveLDFS(int maxDepth)
    {
        HashSet<string> visited = new HashSet<string>();
        var result = LDFS(initialState, maxDepth, visited, new List<int[,]>());
        return result;
    }

    private List<int[,]> LDFS(int[,] currentState, int depth, HashSet<string> visited, List<int[,]> path)
    {
        if (depth < 0) return null;

        string stateStr = StateToString(currentState);
        if (visited.Contains(stateStr)) return null;

        visited.Add(stateStr);
        path.Add(currentState);

        if (IsGoalState(currentState))
        {
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


    public List<int[,]> SolveRBFS(int fLimit)
    {
        var path = new List<int[,]>();
        var result = RBFS(initialState, 0, fLimit, path);
        return result.success ? path : null;
    }

    private (bool success, int fMin) RBFS(int[,] currentState, int g, int fLimit, List<int[,]> path)
    {
        path.Add(currentState);

        if (IsGoalState(currentState))
        {
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
            path.RemoveAt(path.Count - 1);
            return (false, int.MaxValue);
        }

        while (true)
        {
            successors = successors.OrderBy(x => x.f).ToList();
            var best = successors[0];

            if (best.f > fLimit)
            {
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

    private (int, int) FindPositionInGoalState(int value)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (goalState[i, j] == value)
                {
                    return (i, j);
                }
            }
        }

        throw new Exception("Value not found in goal state");
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

        throw new Exception("Пустая ячейка не найдена");
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