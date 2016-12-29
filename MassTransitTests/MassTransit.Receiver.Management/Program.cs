using MassTransit.RabbitMqTransport;
using System;

namespace MassTransit.Receiver.Management
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.Title = "Management consumer";
			Console.WriteLine("MANAGEMENT");
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

				rabbit.ReceiveEndpoint(rabbitMqHost, "mycompany.domains.queues.events.mgmt", conf =>
				{
					conf.Consumer<CustomerRegisteredConsumerMgmt>();
				});
			});
			rabbitBusControl.Start();
			Console.ReadKey();
			rabbitBusControl.Stop();
		}
	}
}
