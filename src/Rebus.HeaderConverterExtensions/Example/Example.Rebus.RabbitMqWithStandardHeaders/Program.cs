using System;
using System.Threading;
using Rebus.Activation;
using Rebus.Bus;
using Rebus.Config;
using Rebus.HeaderConverterExtensions;

namespace Example.Rebus.RabbitMqWithStandardHeaders
{
    class Program
    {
        static void Main(string[] args)
        {
            var handler = new BuiltinHandlerActivator();
            var rabbitMqConfig = new RabbitMqConfig();
            const string inputQueue = "dotnetQueue";
            AddMessageHandler(handler);
            var bus = ConfigureWithRabbitMqTransport(handler, rabbitMqConfig, inputQueue);
            Thread.Sleep(TimeSpan.FromMinutes(60));
            Environment.Exit(1);
        }
        
        private static IBus ConfigureWithRabbitMqTransport(
            IHandlerActivator handler,
            RabbitMqConfig rabbitMqConfig,
            string inputQueue)
        {
            return Configure.With(handler)
                .Options(optionsConfigure =>
                {
                    // Add standard header converter extension.
                    // The extension will convert incoming messages with standard headers to rebus headers,
                    // and convert rebus headers to standard headers on outgoing messages.
                    optionsConfigure.AddStandardHeaderConverter();
                })
                .Transport(t =>
                    t.UseRabbitMq(rabbitMqConfig.ConnectionString, inputQueue)
                        .EnablePublisherConfirms())
                .Start();
        }

        private static void AddMessageHandler(BuiltinHandlerActivator handler)
        {
            handler.Handle<Message>((bus, messageContext, message) =>
            {
                Console.WriteLine($"Received message: {message.MessagePayload}");
                if (message.TellMeToExit)
                {
                    messageContext.TransactionContext.OnCompleted(async () =>
                    {
                        Console.WriteLine("I'm told to exit - Bye!");
                        Environment.Exit(0); 
                    });
                }
                message.MessagePayload = "\n\t - Hello Python From Dotnet - Standard Headers Works!\n";
                return bus.Reply(message);
            });
        }
    }
}