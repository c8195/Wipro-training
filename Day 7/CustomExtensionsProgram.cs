using System;
using System.Collections.Generic;
using System.Linq;

public static class CustomExtensionsProgram
{
    public static void Main()
    {
        List<int> numbers = new List<int> { 1, 2, 3, 4, 5 };
        Console.WriteLine($"Sum of squares: {numbers.SumOfSquares()}");
    }
}

public static class ListExtensions
{
    public static int SumOfSquares(this List<int> list)
    {
        return list.Sum(n => n * n);
    }
}