using System;

class ABC
{
    // Delegate declarations
    public delegate void MyShow();
    public delegate void Printing();
    public delegate void Admin();

    // Method to be assigned to MyShow delegate
    public static void Show()
    {
        Console.WriteLine("Show method called using Delegate");
    }

    // Method to be assigned to Printing delegate
    public static void Print()
    {
        Console.WriteLine("Print method called using Delegate");
    }

    // Method to be assigned to Admin delegate
    public static void Invoice()
    {
        // Invoice generation logic
    }

    static void Main(string[] args)
    {
        // Instantiating delegates with corresponding methods
        MyShow my = new MyShow(Show);
        Printing my1 = new Printing(Print);
        Admin a = new Admin(Invoice);

        // Invoking the delegates
        my();
        my1();
        a();
    }
}
