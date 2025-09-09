using System;

namespace SolidPrincipleDemo
{
    public class Bird
    {
        public virtual void Fly()
        {
            Console.WriteLine("Bird is flying");
        }
    }

    public class Sparrow : Bird
    {
        public override void Fly()
        {
            Console.WriteLine("Sparrow is flying");
        }
    }

    public class Ostrich : Bird
    {
        public override void Fly()
        {
            throw new NotSupportedException("Ostrich can't fly!");
        }
    }

    public class LSP_Without
    {
        public static void Run()
        {
            Console.WriteLine("=== Liskov Substitution Principle - Without LSP ===");

            Bird bird1 = new Sparrow();
            bird1.Fly();

            Bird bird2 = new Ostrich();  // Violates LSP
            try
            {
                bird2.Fly();  // Will throw exception — not good!
            }
            catch (NotSupportedException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
