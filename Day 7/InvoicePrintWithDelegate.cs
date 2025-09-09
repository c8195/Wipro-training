using System;

class Program
{
    // Delegate to calculate invoice total
    public delegate int Admin(int tuitionFees, int transportFees);

    // Delegate to print the invoice
    public delegate void PrintInvoice(int tuitionFees, int transportFees);

    // Method that calculates total invoice
    public static int CalculateInvoice(int tuitionFees, int transportFees)
    {
        return tuitionFees + transportFees;
    }

    // Print Page 1: Normal invoice
    public static void PrintPage1(int tuitionFees, int transportFees)
    {
        int total = tuitionFees + transportFees;
        Console.WriteLine("\n=== Page 1: Invoice ===");
        Console.WriteLine($"Tuition Fees   : {tuitionFees}");
        Console.WriteLine($"Transport Fees : {transportFees}");
        Console.WriteLine($"Total Amount   : {total}");
    }

    // Print Page 2: Deduction 100% on tuition fees
    public static void PrintPage2(int tuitionFees, int transportFees)
    {
        int total = 0 + transportFees;
        Console.WriteLine("\n=== Page 2: 100% Tuition Deduction ===");
        Console.WriteLine($"Tuition Fees (Waived) : ₹0 (Original ₹{tuitionFees})");
        Console.WriteLine($"Transport Fees        : ₹{transportFees}");
        Console.WriteLine($"Total Amount          : ₹{total}");
    }

    static void Main(string[] args)
    {
        // Delegates
        Admin admin = CalculateInvoice;
        PrintInvoice printer1 = PrintPage1;
        PrintInvoice printer2 = PrintPage2;

        // User input
        Console.Write("Enter Tuition Fees: ₹");
        int tuition = Convert.ToInt32(Console.ReadLine());

        Console.Write("Enter Transport Fees: ₹");
        int transport = Convert.ToInt32(Console.ReadLine());

        // Call both print methods
        printer1(tuition, transport); // Page 1
        printer2(tuition, transport); // Page 2
    }
}
