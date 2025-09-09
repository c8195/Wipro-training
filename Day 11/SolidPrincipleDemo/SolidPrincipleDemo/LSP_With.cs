using System;
using System.Collections.Generic;

namespace SolidPrincipleDemo
{
    public abstract class BirdBase
    {
        public abstract void MakeSound();
    }

    public interface IFlyingBird
    {
        void Fly();
    }

    public class LSP_Sparrow : BirdBase, IFlyingBird
    {
        public override void MakeSound()
        {
            Console.WriteLine("Sparrow chirps");
        }

        public void Fly()
        {
            Console.WriteLine("Sparrow flies");
        }
    }

    public class LSP_Ostrich : BirdBase
    {
        public override void MakeSound()
        {
            Console.WriteLine("Ostrich makes booming sound");
        }

        // No Fly() method — doesn't violate anything
    }

    public class LSP_With
    {
        public static void Run()
        {
            Console.WriteLine("=== Liskov Substitution Principle - With LSP ===");

            List<BirdBase> birds = new List<BirdBase>
            {
                new LSP_Sparrow(),
                new LSP_Ostrich()
            };

            foreach (var bird in birds)
            {
                bird.MakeSound();
            }

            // Flying birds handled separately
            List<IFlyingBird> flyingBirds = new List<IFlyingBird>
            {
                new LSP_Sparrow()
            };

            foreach (var flyingBird in flyingBirds)
            {
                flyingBird.Fly();
            }
        }
    }
}
