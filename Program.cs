
//////// creational patterns
//////// single ton pattern
////////

//////using consoleapp_designpatternsdemo_wipro;
//////using System;
//////// sealed -- stopping the inheritance of this class

//////// static variables, static methods, static constructors
//////// static classes

//////namespace consoleapp_designpatternsdemo_wipro
//////{
//////    public sealed class Singleton
//////    {
//////        private static Singleton _instance = null;
//////        private Singleton() { }

//////        public static Singleton Instance   //read only property
//////        {
//////            get
//////            {
//////                if (_instance == null)
//////                    _instance = new Singleton();

//////                return _instance;
//////            }
//////        }

//////        public void ShowMessage(string msg)
//////        {
//////            Console.WriteLine(msg);
//////        }
//////    }
//////}


//////class MyClass
//////{
//////    static void Main(string[] args)
//////    {
//////       // Singleton obj= new Singleton(); 
//////        // this will not work as the constructor is private

//////        Singleton obj = Singleton.Instance; // get the instance of the singleton class

//////        obj.ShowMessage("Hello, Singleton Pattern!");
//////    }
//////}

//////=================================================================

////// structural patterns
////// adapter pattern

////using System;

////// Adaptee: Legacy class with incompatible interface
////public class LegacyPrinter
////{
////    public void PrintDocument()
////    {
////        Console.WriteLine("Printing document using LegacyPrinter...");
////    }
////}

////// Target Interface: What the client expects
////public interface IPrinter
////{
////    void Print();
////}

////// Adapter: Converts LegacyPrinter to IPrinter
////public class PrinterAdapter : IPrinter
////{
////    private readonly LegacyPrinter _legacyPrinter;

////    public PrinterAdapter(LegacyPrinter legacyPrinter)
////    {
////        _legacyPrinter = legacyPrinter;
////    }

////    public void Print()
////    {
////        _legacyPrinter.PrintDocument(); // Translate call
////    }
////}

////// Client Code
////class Program
////{
////    static void Main()
////    {
////        IPrinter printer = new PrinterAdapter(new LegacyPrinter());
////        printer.Print(); // Works like a charm!
////    }
////}


//// behavioural design patterns
//// chain of responsibility pattern

//using System;

//// Handler interface
//public abstract class Approver
//{
//    protected Approver _next;

//    public void SetNext(Approver next) => _next = next;

//    public abstract void ProcessRequest(PurchaseRequest request);
//}

//public class Manager : Approver
//{
//    public override void ProcessRequest(PurchaseRequest request)
//    {
//        if (request.Amount <= 10000)
//            Console.WriteLine("Manager approved request for ₹" + request.Amount);
//        else
//            _next?.ProcessRequest(request);
//    }
//}

//public class Director : Approver
//{
//    public override void ProcessRequest(PurchaseRequest request)
//    {
//        if (request.Amount <= 50000)
//            Console.WriteLine("Director approved request for ₹" + request.Amount);
//        else
//            _next?.ProcessRequest(request);
//    }
//}

//public class VicePresident : Approver
//{
//    public override void ProcessRequest(PurchaseRequest request)
//    {
//        if (request.Amount <= 100000)
//            Console.WriteLine("Vice President approved request for ₹" + request.Amount);
//        else
//            Console.WriteLine("Request for ₹" + request.Amount + " requires board approval.");
//    }
//}

//public class PurchaseRequest
//{
//    public double Amount { get; set; }

//    public PurchaseRequest(double amount)
//    {
//        Amount = amount;
//    }
//}

//class Program
//{
//    static void Main()
//    {
//        Approver manager = new Manager();
//        Approver director = new Director();
//        Approver vp = new VicePresident();

//        manager.SetNext(director);
//        director.SetNext(vp);

//        var requests = new[]
//        {
//            new PurchaseRequest(5000),
//            new PurchaseRequest(30000),
//            new PurchaseRequest(75000),
//            new PurchaseRequest(150000)
//        };

//        foreach (var request in requests)
//        {
//            manager.ProcessRequest(request);
//        }
//    }
//}