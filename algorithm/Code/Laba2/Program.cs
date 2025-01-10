using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using Laba2;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        int[,] initialState = new int[,] {
            { 1, 2, 3 },
            { 4, 0, 6 },
            { 7, 5, 8 }
        };

        int[,] goalState = new int[,] {
            { 1, 2, 3 },
            { 4, 5, 6 },
            { 7, 8, 0 }
        };

        var puzzle = new EightPuzzle(initialState, goalState);
        
        // LDFS
        Console.WriteLine("Solving puzzle using LDFS:");
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        
        int maxDepth = 20;
        var solutionLDFS = puzzle.SolveLDFS(maxDepth);
        var ldfsStats = puzzle.Statistics;
        
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
        
        // RBFS
        Console.WriteLine("\nSolving puzzle using RBFS:");
        puzzle = new EightPuzzle(initialState, goalState); // Create new instance for clean statistics
        stopwatch.Restart();
        
        int fLimit = 100;
        var solutionRBFS = puzzle.SolveRBFS(fLimit);
        var rbfsStats = puzzle.Statistics;
        
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

        // LDFS Table
        Console.WriteLine("\nТаблиця 3.1 - Характеристики оцінювання алгоритму LDFS (задача 8-puzzle)");
        PrintTable(solutionLDFS, ldfsStats);

        // RBFS Table
        Console.WriteLine("\nТаблиця 3.2 - Характеристики оцінювання алгоритму RBFS (задача 8-puzzle)");
        PrintTable(solutionRBFS, rbfsStats);
    }

    static void PrintTable(List<int[,]> solution, SearchStatistics stats)
    {
        Console.WriteLine("|{0,15}|{1,10}|{2,15}|{3,15}|{4,20}|", 
            "Початкові стани", "Ітерації", "К-сть гл. кутів", "Всього вузлів", "Всього вузлів у пам'яті");
        Console.WriteLine(new string('-', 80));

        if (solution != null)
        {
            int totalSteps = solution.Count - 1;
            int nodesPerStep = stats.GeneratedStates / totalSteps;
            int deadEndsPerStep = stats.DeadEnds / totalSteps;
            int memoryPerStep = stats.StatesInMemory / totalSteps;

            for (int i = 0; i < totalSteps; i++)
            {
                Console.WriteLine("|{0,15}|{1,10}|{2,15}|{3,15}|{4,20}|",
                    $"Стан {i+1}", 
                    i, 
                    deadEndsPerStep * (i + 1),
                    nodesPerStep * (i + 1),
                    memoryPerStep * (i + 1));
            }
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