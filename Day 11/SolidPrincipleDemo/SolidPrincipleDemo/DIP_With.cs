using System;

namespace SolidPrincipleDemo
{
    // Abstraction
    public interface IMessageService
    {
        void SendMessage(string message);
    }

    // Low-level module depends on interface
    public class EmailServiceDIP : IMessageService
    {
        public void SendMessage(string message)
        {
            Console.WriteLine("Email sent via DIP: " + message);
        }
    }

    public class SMSService : IMessageService
    {
        public void SendMessage(string message)
        {
            Console.WriteLine("SMS sent via DIP: " + message);
        }
    }

    // High-level module depends on abstraction
    public class DIP_NotificationManager
    {
        private readonly IMessageService _messageService;

        public DIP_NotificationManager(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public void Send(string message)
        {
            _messageService.SendMessage(message);
        }
    }

    public class DIP_With
    {
        public static void Run()
        {
            Console.WriteLine("=== Dependency Inversion Principle - With DIP ===");

            IMessageService emailService = new EmailServiceDIP();
            DIP_NotificationManager emailManager = new DIP_NotificationManager(emailService);
            emailManager.Send("Hello via Email!");

            IMessageService smsService = new SMSService();
            DIP_NotificationManager smsManager = new DIP_NotificationManager(smsService);
            smsManager.Send("Hello via SMS!");
        }
    }
}
