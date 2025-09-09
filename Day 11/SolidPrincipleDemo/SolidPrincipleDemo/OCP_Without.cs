using System;

namespace SolidPrincipleDemo
{
    public enum ReportType
    {
        PDF,
        Word
    }

    public class ReportGenerator
    {
        public void GenerateReport(ReportType type)
        {
            if (type == ReportType.PDF)
            {
                Console.WriteLine("Generating PDF Report...");
            }
            else if (type == ReportType.Word)
            {
                Console.WriteLine("Generating Word Report...");
            }
            else
            {
                throw new NotSupportedException("Report type not supported.");
            }
        }
    }

    public class OCP_Without
    {
        public static void Run()
        {
            Console.WriteLine("=== Open/Closed Principle - Without OCP ===");

            ReportGenerator generator = new ReportGenerator();
            generator.GenerateReport(ReportType.PDF);
            generator.GenerateReport(ReportType.Word);

            // If a new report type is needed, ReportGenerator class must be modified — this breaks OCP
        }
    }
}
