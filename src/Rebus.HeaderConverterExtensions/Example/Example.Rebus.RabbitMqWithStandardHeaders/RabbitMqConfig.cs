using System;

namespace Example.Rebus.RabbitMqWithStandardHeaders
{
    public class RabbitMqConfig
    {
        public static bool InContainer => 
            Environment.GetEnvironmentVariable("RUNNING_IN_CONTAINER") == "true";
        
        public string User { get; set; } = "amqp";

        public string Password { get; set; } = "amqp";

        public string Hostname { get; set; } = InContainer ? "rabbitmq" : "localhost";

        public int Port { get; set; } = 5672;

        public string VirtualHost { get; set; } = "";
        
        public string ConnectionString =>
            $"amqp://{User}:{Password}@{Hostname}:{Port}/{VirtualHost}";
    }
}