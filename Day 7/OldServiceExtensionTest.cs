using System;

public sealed class OldService
{
    public int x = 300;

    public void Test1() => Console.WriteLine("Test1 from OldService");
    public void Test2() => Console.WriteLine("Test2 from OldService");
}

public static class OldServiceExtensions
{
    public static void Test4(this OldService os) => Console.WriteLine("Extension method Test4");
    public static void Test5(this OldService os, int a, int b)
    {
        Console.WriteLine($"Sum: {a + b}");
        Console.WriteLine($"Accessing x: {os.x}");
    }
}

public class ProgramOldServiceExtension
{
    public static void Main()
    {
        OldService obj = new OldService();
        obj.Test1();
        obj.Test2();
        obj.Test4();
        obj.Test5(100, 200);
    }
}