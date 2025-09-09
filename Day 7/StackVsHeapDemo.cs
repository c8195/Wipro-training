using System;

public class StackVsHeapDemo
{
    public static void Main()
    {
        // Stack example
        int a = 5;
        int b = a;
        b = 60;
        Console.WriteLine("Stack Example:");
        Console.WriteLine("a: " + a); // 5
        Console.WriteLine("b: " + b); // 60

        // Heap example
        Student s1 = new Student();
        s1.Name = "Niti";
        Student s2 = s1; // Reference copy
        s2.Name = "Preeti";

        Console.WriteLine("\nHeap Example:");
        Console.WriteLine("s1.Name: " + s1.Name); // Preeti
        Console.WriteLine("s2.Name: " + s2.Name); // Preeti
    }
}

public class Student
{
    public string Name;
}