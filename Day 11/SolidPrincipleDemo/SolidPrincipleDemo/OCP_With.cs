using System;
using System.Collections.Generic;

namespace SolidPrincipleDemo
{
    public interface IReportGenerator
    {
        void GenerateReport();
    }

    public class PDFReportGenerator : IReportGenerator
    {
        public void GenerateReport()
        {
            Console.WriteLine("Generating PDF Report...");
        }
    }

    public class WordReportGenerator : IReportGenerator
    {
        public void GenerateReport()
        {
            Console.WriteLine("Generating Word Report...");
        }
    }

    // Easily extendable without modifying existing code
    public class OCP_With
    {
        public static void Run()
        {
            Console.WriteLine("=== Open/Closed Principle - With OCP ===");

            List<IReportGenerator> generators = new List<IReportGenerator>
            {
                new PDFReportGenerator(),
                new WordReportGenerator()
            };

            foreach (var generator in generators)
            {
                generator.GenerateReport();
            }

            // Want Excel support? Just add new ExcelReportGenerator class, no need to touch existing ones!
        }
    }
}
