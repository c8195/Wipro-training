using System;

namespace SolidPrincipleDemo
{
    public interface IWorkable
    {
        void Work();
    }

    public interface IEatable
    {
        void Eat();
    }

    public class NewHumanWorker : IWorkable, IEatable
    {
        public void Work()
        {
            Console.WriteLine("New Human is working.");
        }

        public void Eat()
        {
            Console.WriteLine("New Human is eating.");
        }
    }

    public class NewRobotWorker : IWorkable
    {
        public void Work()
        {
            Console.WriteLine("New Robot is working.");
        }
    }

    public class ISP_With
    {
        public static void Run()
        {
            Console.WriteLine("=== Interface Segregation Principle - With ISP ===");
            IWorkable human = new NewHumanWorker();
            human.Work();
            ((IEatable)human).Eat();

            IWorkable robot = new NewRobotWorker();
            robot.Work();
        }
    }
}
