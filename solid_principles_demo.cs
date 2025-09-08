//// SRP

//using System;

//public class User
//{
//    public string Username { get; set; }
//    public string Email { get; set; }

//    public void SaveUserToDatabase()
//    {
//        // Logic to save user data to a database
//        Console.WriteLine($"Saving user {Username} to database.");
//    }
//}

//public class UserService
//{ 
//    public void SendWelcomeEmail()
//    {
//        // Logic to send a welcome email to the user
//        Console.WriteLine($"Sending welcome email to {Email}.");
//    }
//}
//========================================================================
//  Open /Closed Principle (OCP)

//using System;

//public enum ShapeType
//{
//    Rectangle,
//    Circle
//}
//public class AreaCalculator
//{
//    public double CalculateArea(ShapeType type, double value1, double value2 = 0)
//    {
//        if (type == ShapeType.Rectangle)
//        {
//            return value1 * value2;
//        }
//        else if (type == ShapeType.Circle)
//        {
//            return Math.PI * value1 * value1; // value1 is radius
//        }
//        // If a new shape like Triangle is added, this class needs modification.
//        return 0;
//    }
//}
//========================================================================

//  LSP - Liskov Substitution Principle

using System;

public class Bird
{
    public virtual void Fly()  {   Console.WriteLine("The bird is flying.");  }
}

public class Sparrow : Bird { 
    public override void Fly()  {   Console.WriteLine("The sparrow is flying gracefully.");  }
}

public class Penguin : Bird
{
    public override void Fly()
    {
        throw new InvalidOperationException("Penguins cannot fly!");
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        Bird sparrow = new Sparrow();
        sparrow.Fly(); // Output: The sparrow is flying gracefully.
        Bird penguin = new Penguin();
        try
        {
            penguin.Fly(); // This will throw an exception
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message); // Output: Penguins cannot fly!
        }
    }
}


//  dependency_inversion_principle.cs


// without using DIP
public class DatabaseSaver
{
    public void Save(string reportContent)
    {
        // Logic to save report to a specific database (e.g., SQL Server)
        Console.WriteLine($"Saving '{reportContent}' to SQL Database.");
    }
}
public class ReportGenerator
{
    private DatabaseSaver _saver;

    public ReportGenerator()
    {
        _saver = new DatabaseSaver(); // Direct dependency on a concrete class
    }

    public void GenerateAndSaveReport(string data)
    {
        string report = $"Generated Report: {data}";
        _saver.Save(report);
    }
}