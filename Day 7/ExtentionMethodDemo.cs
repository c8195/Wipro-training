using System;
using System.Linq;

public static class ExtensionMethodsDemo
{
    public static void Main()
    {
        string s = "Level";
        Console.WriteLine(s.IsPalindrome());
    }
}

public static class StringExtensions
{
    public static bool IsPalindrome(this string s)
    {
        var rev = new string(s.Reverse().ToArray());
        return s.Equals(rev, StringComparison.OrdinalIgnoreCase);
    }
}
