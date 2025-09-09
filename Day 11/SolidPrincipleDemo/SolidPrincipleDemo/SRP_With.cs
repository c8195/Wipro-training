using System;
using System.IO;

namespace SolidPrincipleDemo
{
    public class CleanReport
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }

    public class ReportPrinter
    {
        public void Print(CleanReport report)
        {
            Console.WriteLine("Title: " + report.Title);
            Console.WriteLine("Content: " + report.Content);
        }
    }

    public class ReportSaver
    {
        public void SaveToFile(CleanReport report, string filename)
        {
            File.WriteAllText(filename, $"Title: {report.Title}\nContent: {report.Content}");
            Console.WriteLine($"Report saved to {filename}");
        }
    }

    public class SRP_With
    {
        public static void Run()
        {
            Console.WriteLine("=== Single Responsibility Principle - With SRP ===");

            CleanReport report = new CleanReport { Title = "Weekly Report", Content = "No issues reported." };

            ReportPrinter printer = new ReportPrinter();
            printer.Print(report);

            ReportSaver saver = new ReportSaver();
            saver.SaveToFile(report, "clean_report.txt"); // Each class has a single responsibility
        }
    }
}
