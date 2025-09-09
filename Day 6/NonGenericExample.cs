using System;
using System.Collections;
using System.Collections.Generic;

class Student
{
    public int Id;
    public string Name;
    public Student(int id, string name)
    {
        Id = id;
        Name = name;
    }
}

class NonGenericExample
{
    static void Main()
    {
        // Boxing example
        int num = 10;
        object boxedvalue = num; // Boxing

        // Unboxing example
        int valuetype = (int)boxedvalue; // Unboxing

        // List<Student> (Generic, type safe)
        List<Student> list = new List<Student>();
        for (int i = 0; i < 5; i++)
        {
            Student s = new Student(3434, "df");
            list.Add(s);
        }

        // List<int> - Generic, value types (no boxing/unboxing)
        List<int> numbers = new List<int>();
        numbers.Add(10);
        int result = numbers[0];

        // ArrayList - Non-Generic, boxing will happen
        ArrayList arrayList = new ArrayList();
        int value = 20;
        arrayList.Add(value); // Boxing
        Console.WriteLine(arrayList.GetType());
        double unboxingValue = Convert.ToDouble(arrayList[0]);

        Console.WriteLine("The value after unboxing " + unboxingValue);

        // Generic reference
        List<int> intList = new List<int>();
        intList.Add(value);
        double value2 = intList[0];
        Console.WriteLine("The value after unboxing " + value2);
    }
}
