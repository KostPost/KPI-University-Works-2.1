using System;
using System.Diagnostics;

class Program
{

    static void Main()
    {
        
         Stopwatch stopwatch = new Stopwatch();
         stopwatch.Start(); 
        
        string inputFilePath = "P:\\KPI-Works 3\\algorithm\\Code\\Laba1\\large_array.txt";
        string outputFilePath = "P:\\KPI-Works 3\\algorithm\\Code\\Laba1\\sorted_large_array.txt";

        GenerateRandomNumbersToFile(inputFilePath, 500);

        int[] array = ReadNumbersFromFile(inputFilePath);

        array = MergeSort(array);

        WriteNumbersToFile(outputFilePath, array);

        Console.WriteLine($"Sorted file is saved at {outputFilePath}");
        
        stopwatch.Stop();
        Console.WriteLine($"\n Sorting completed in {stopwatch.Elapsed.TotalSeconds} seconds");

        // Если хотите узнать более подробное время, то можно использовать:
        Console.WriteLine($"Total time: {stopwatch.Elapsed.Hours} hours {stopwatch.Elapsed.Minutes} minutes {stopwatch.Elapsed.Seconds} seconds");
    }
    
    // Метод для генерации случайных чисел и записи в файл
    static void GenerateRandomNumbersToFile(string filePath, int sizeInMb)
    {
        Random random = new Random();
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            long totalNumbers = sizeInMb * 1024 * 1024 / 4; // 4 байта на одно целое число
            for (long i = 0; i < totalNumbers; i++)
            {
                writer.WriteLine(random.Next(-1000000000, 1000000000));
            }
        }
        Console.WriteLine($"Generated file with random numbers at {filePath}");
    }

    // Метод для чтения чисел из файла и их преобразования в массив
    static int[] ReadNumbersFromFile(string filePath)
    {
        string[] lines = File.ReadAllLines(filePath);
        int[] numbers = new int[lines.Length];
        
        for (int i = 0; i < lines.Length; i++)
        {
            numbers[i] = int.Parse(lines[i]);
        }
        
        return numbers;
    }

    // Метод для записи отсортированных чисел в файл
    static void WriteNumbersToFile(string filePath, int[] array)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            foreach (int number in array)
            {
                writer.WriteLine(number);
            }
        }
    }

    

    static int[] MergeSort(int[] array)
    {
        if (array.Length <= 1)
            return array;

        // Разделяем массив на две части
        int mid = array.Length / 2;
        int[] left = new int[mid];
        int[] right = new int[array.Length - mid];

        Array.Copy(array, 0, left, 0, mid);
        Array.Copy(array, mid, right, 0, array.Length - mid);

        left = MergeSort(left);
        right = MergeSort(right);

        return Merge(left, right);
    }

    static int[] Merge(int[] left, int[] right)
    {
        int[] result = new int[left.Length + right.Length];
        int i = 0, j = 0, k = 0;

        while (i < left.Length && j < right.Length)
        {
            if (left[i] <= right[j])
            {
                result[k] = left[i];
                i++;
            }
            else
            {
                result[k] = right[j];
                j++;
            }
            k++;
        }

        while (i < left.Length)
        {
            result[k] = left[i];
            i++;
            k++;
        }

        while (j < right.Length)
        {
            result[k] = right[j];
            j++;
            k++;
        }

        return result;
    }
}