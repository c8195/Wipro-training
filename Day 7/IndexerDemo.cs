using System;

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; }

    public Employee(int id, string name)
    {
        this.Id = id;
        this.Name = name;
    }

    public object this[int index]
    {
        get
        {
            if (index == 0) return Id;
            else if (index == 1) return Name;
            else return null;
        }
        set
        {
            if (index == 0) Id = Convert.ToInt32(value);
            else if (index == 1) Name = value.ToString();
        }
    }
}

public class IndexerDemo
{
    public static void Main()
    {
        Employee e = new Employee(101, "Niti");
        Console.WriteLine("Name using indexer: " + e[1]);

        if (int.TryParse("12", out int value))
        {
            Console.WriteLine("Parsed value: " + value);
        }
    }
}
