using System;

namespace SolidPrincipleDemo
{
    // Low-level module
    public class EmailService
    {
        public void SendEmail(string message)
        {
            Console.WriteLine("Email sent: " + message);
        }
    }

    // High-level module depends on low-level module directly
    public class NotificationManager
    {
        private EmailService _emailService;

        public NotificationManager()
        {
            _emailService = new EmailService(); // Tightly coupled
        }

        public void Send(string message)
        {
            _emailService.SendEmail(message);
        }
    }

    public class DIP_Without
    {
        public static void Run()
        {
            Console.WriteLine("=== Dependency Inversion Principle - Without DIP ===");

            NotificationManager manager = new NotificationManager();
            manager.Send("Hello, customer!");
        }
    }
}
