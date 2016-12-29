using MassTransit;
using MassTransit.Scheduling;
using MyCompany.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassTransitTests
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("CUSTOMER REGISTRATION COMMAND PUBLISHER");
			Console.Title = "Publisher window";
			RunMassTransitPublisherWithRabbit();
		}

		private static void ScheduleMessagesWithQuartzInMemory()
		{
			string rabbitMqAddress = "rabbitmq://localhost:5672/accounting";
			string rabbitMqQueue = "mycompany.queues.news.scheduled";
			Uri rabbitMqRootUri = new Uri(rabbitMqAddress);

			IBusControl rabbitBusControl = Bus.Factory.CreateUsingRabbitMq(rabbit =>
			{
				rabbit.Host(rabbitMqRootUri, settings =>
				{
					settings.Password("accountant");
					settings.Username("accountant");
				});

				rabbit.UseInMemoryScheduler();
			});
			Uri sendUri = new Uri(string.Concat(rabbitMqAddress, "/", rabbitMqQueue));
			Task<ISendEndpoint> sendEndpointTask = rabbitBusControl.GetSendEndpoint(sendUri);
			ISendEndpoint sendEndpoint = sendEndpointTask.Result;

			Task<ScheduledMessage<IWorldNews>> scheduledMessageTask = sendEndpoint.ScheduleSend<IWorldNews>
				(new Uri("rabbitmq://localhost:5672/quartz"), TimeSpan.FromSeconds(30), new { Message = "The world is going down." });
			ScheduledMessage <IWorldNews> scheduledMessage = scheduledMessageTask.Result;

			Console.ReadKey();
		}

		private static void RunMassTransitPublisherWithRabbit()
		{
			string rabbitMqAddress = "rabbitmq://localhost:5672/accounting";
			string rabbitMqQueue = "mycompany.domains.queues";
			Uri rabbitMqRootUri = new Uri(rabbitMqAddress);

			IBusControl rabbitBusControl = Bus.Factory.CreateUsingRabbitMq(rabbit =>
			{
				rabbit.Host(rabbitMqRootUri, settings =>
				{
					settings.Password("accountant");
					settings.Username("accountant");
				});				
			});

			

			//rabbitBusControl.ConnectSendObserver(new SendObjectObserver());

			Task<ISendEndpoint> sendEndpointTask = rabbitBusControl.GetSendEndpoint(new Uri(string.Concat(rabbitMqAddress, "/", rabbitMqQueue)));
			ISendEndpoint sendEndpoint = sendEndpointTask.Result;

			Task sendTask = sendEndpoint.Send<IRegisterCustomer>(new
			{
				Address = "New Street",
				Id = Guid.NewGuid(),
				Preferred = true,
				RegisteredUtc = DateTime.UtcNow,
				Name = "Nice people LTD",
				Type = 1,
				DefaultDiscount = 0,
				Target = "Customers",
				Importance = 1
			}, c =>
			{
				c.FaultAddress = new Uri("rabbitmq://localhost:5672/accounting/mycompany.queues.errors.newcustomers");
			});
			
			Console.ReadKey();
		}
	}
}
