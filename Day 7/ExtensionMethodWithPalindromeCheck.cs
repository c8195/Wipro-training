using System;
using System.Linq;

public static class PalindromeProgram
{
    public static void Main()
    {
        string word = "Racecar";
        Console.WriteLine($"Is '{word}' a palindrome? {word.IsPalindrome()}");
    }
}

public static class PalindromeExtensions
{
    public static bool IsPalindrome(this string s)
    {
        return s.SequenceEqual(s.Reverse(), StringComparer.OrdinalIgnoreCase);
    }
}
