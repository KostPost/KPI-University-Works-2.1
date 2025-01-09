using System;

namespace GraphColoring
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Running the Bee Algorithm for Graph Coloring");
            
            // Create and run the algorithm
            BeeAlgorithm algorithm = new BeeAlgorithm(
                vertices: 200,     // Number of vertices
                maxDegree: 30,     // Maximum degree of a vertex
                bees: 40,          // Total number of bees
                scouts: 2,         // Number of scout bees
                colors: 30         // Maximum number of colors
            );

            // Run for 1000 iterations
            algorithm.Run(1000);

            // Display the results
        
        }
    }
}