using System;
using System.Collections.Generic;
using System.Diagnostics;
using Laba2;

class Program
{
    static void Main(string[] args)
    {
        // Начальное состояние (0 представляет пустую ячейку)
        int[,] initialState = new int[,] {
            { 1, 2, 3 },
            { 4, 0, 6 },
            { 7, 5, 8 }
        };

        // Целевое состояние
        int[,] goalState = new int[,] {
            { 1, 2, 3 },
            { 4, 5, 6 },
            { 7, 8, 0 }
        };

        var puzzle = new EightPuzzle(initialState, goalState);
        
        
        Console.WriteLine("Solving puzzle using LDFS:");
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        
        int maxDepth = 20;
        var solutionLDFS = puzzle.SolveLDFS(maxDepth);
        
        stopwatch.Stop();
        Console.WriteLine($"LDFS Time taken: {stopwatch.ElapsedMilliseconds}ms");
        
        if (solutionLDFS != null)
        {
            Console.WriteLine("LDFS Solution found!");
            PrintSolution(solutionLDFS);
        }
        else
        {
            Console.WriteLine("LDFS Solution not found within the specified depth.");
        }
        
        Console.WriteLine("\nSolving puzzle using RBFS:");
        stopwatch.Restart();
        
        int fLimit = 100;
        var solutionRBFS = puzzle.SolveRBFS(fLimit);
        
        stopwatch.Stop();
        Console.WriteLine($"RBFS Time taken: {stopwatch.ElapsedMilliseconds}ms");

        if (solutionRBFS != null)
        {
            Console.WriteLine("RBFS Solution found!");
            PrintSolution(solutionRBFS);
        }
        else
        {
            Console.WriteLine("RBFS Solution not found within the specified limit.");
        }
    }

    static void PrintSolution(List<int[,]> solution)
    {
        Console.WriteLine($"Number of moves: {solution.Count - 1}");
        for (int i = 0; i < solution.Count; i++)
        {
            Console.WriteLine($"\nStep {i}:");
            PrintState(solution[i]);
        }
    }

    static void PrintState(int[,] state)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Console.Write(state[i, j] == 0 ? "_ " : state[i,j] + " ");
            }
            Console.WriteLine();
        }
    }
}