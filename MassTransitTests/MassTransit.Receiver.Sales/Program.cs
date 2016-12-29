using MassTransit.RabbitMqTransport;
using System;

namespace MassTransit.Receiver.Sales
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.Title = "Sales consumer";
			Console.WriteLine("SALES");
			RunMassTransitReceiverWithRabbit();
		}

		private static void RunMassTransitReceiverWithRabbit()
		{
			IBusControl rabbitBusControl = Bus.Factory.CreateUsingRabbitMq(rabbit =>
			{
				IRabbitMqHost rabbitMqHost = rabbit.Host(new Uri("rabbitmq://localhost:5672/accounting"), settings =>
				{
					settings.Password("accountant");
					settings.Username("accountant");
				});

				rabbit.ReceiveEndpoint(rabbitMqHost, "mycompany.domains.queues.events.sales", conf =>
				{
					conf.Consumer<CustomerRegisteredConsumerSls>();
				});
			});

			rabbitBusControl.Start();
			Console.ReadKey();

			rabbitBusControl.Stop();
		}
	}
}
