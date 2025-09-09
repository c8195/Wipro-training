using System;
using System.Collections.Generic;

class GenericExample
{
    static void Main()
    {
        Console.WriteLine("----- Generic Example -----");

        List<int> intList = new List<int>();
        int value = 20;

        intList.Add(value); // No Boxing
        double value2 = intList[0]; // No Unboxing needed
        Console.WriteLine("The value retrieved from List<int>: " + value2);
    }
}
