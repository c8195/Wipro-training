using System;
using System.IO;

namespace SolidPrincipleDemo
{
    public class Report
    {
        public string Title { get; set; }
        public string Content { get; set; }

        public void PrintReport()
        {
            Console.WriteLine("Title: " + Title);
            Console.WriteLine("Content: " + Content);
        }

        public void SaveToFile(string filename)
        {
            File.WriteAllText(filename, $"Title: {Title}\nContent: {Content}");
            Console.WriteLine($"Report saved to {filename}");
        }
    }

    public class SRP_Without
    {
        public static void Run()
        {
            Console.WriteLine("=== Single Responsibility Principle - Without SRP ===");

            Report report = new Report { Title = "Monthly Report", Content = "All systems operational." };
            report.PrintReport();
            report.SaveToFile("report.txt"); // Report class is doing too much (Printing + Saving)
        }
    }
}
