using System;

namespace SolidPrincipleDemo
{
    public interface IWorker
    {
        void Work();
        void Eat();
    }

    public class HumanWorker : IWorker
    {
        public void Work()
        {
            Console.WriteLine("Human is working.");
        }

        public void Eat()
        {
            Console.WriteLine("Human is eating.");
        }
    }

    public class RobotWorker : IWorker
    {
        public void Work()
        {
            Console.WriteLine("Robot is working.");
        }

        public void Eat()
        {
            Console.WriteLine("Robot cannot eat! (Bad design: forced to implement Eat)");
        }
    }

    public class ISP_Without
    {
        public static void Run()
        {
            Console.WriteLine("=== Interface Segregation Principle - Without ISP ===");
            IWorker human = new HumanWorker();
            human.Work();
            human.Eat();

            IWorker robot = new RobotWorker();
            robot.Work();
            robot.Eat(); // Violates ISP
        }
    }
}
