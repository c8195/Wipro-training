// create a delegate for a admin who is responsible for calculating the invoice(int tutionfess , int transportfees)
//and one more delegate which will print the invoice
using System;
class Delegate3
{
    public delegate int Admin(int tFees, int tpFees);
    public delegate void PrintData(int total);
    static void Main()
    {
        // assigning methods to delegate
        Admin ad = TotalFess;
        PrintData p = Show;
       int res = ad(10000, 2000);
        p(res);
    }
    public static int TotalFess(int tf, int tpf)
    {
        int gst = 10;
        return gst + tf + tpf;
    }
    public static void Show(int total)
    {
        Console.WriteLine("the total fees is:" + total);
            }
}