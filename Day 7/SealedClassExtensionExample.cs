using System;

public sealed class SealedService
{
    public void OriginalMethod()
    {
        Console.WriteLine("Original method from sealed class");
    }
}

public static class SealedServiceExtensions
{
    public static void ExtendedMethod(this SealedService svc)
    {
        Console.WriteLine("Extension method for sealed class");
    }
}

public class DemoSealedExtension
{
    public static void Main()
    {
        SealedService s = new SealedService();
        s.OriginalMethod();
        s.ExtendedMethod();
    }
}