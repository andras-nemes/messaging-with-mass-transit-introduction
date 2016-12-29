using MassTransit.RabbitMqTransport;
using MyCompany.Domains;
using MyCompany.Repository.Dummy;
using StructureMap;
using System;

namespace MassTransit.Receiver
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.Title = "This is the customer registration command receiver.";
			Console.WriteLine("CUSTOMER REGISTRATION COMMAND RECEIVER.");
			RunMassTransitReceiverWithRabbit();
		}

		private static void ReceiveWorldNews()
		{
			IBusControl rabbitBusControl = Bus.Factory.CreateUsingRabbitMq(rabbit =>
			{
				IRabbitMqHost rabbitMqHost = rabbit.Host(new Uri("rabbitmq://localhost:5672/accounting"), settings =>
				{
					settings.Password("accountant");
					settings.Username("accountant");
				});

				rabbit.ReceiveEndpoint(rabbitMqHost, "mycompany.queues.news.scheduled", conf =>
				{
					rabbit.UseInMemoryScheduler();
					conf.Consumer(() => new WorldNewsConsumer());
					//conf.Consumer<WorldNewsConsumer>();
				});				
			});

			rabbitBusControl.Start();			
			Console.ReadKey();

			rabbitBusControl.Stop();
		}

		private static void RunMassTransitReceiverWithRabbit()
		{
			var container = new Container(conf =>
			{
				conf.For<ICustomerRepository>().Use<CustomerRepository>();
			});
			string whatDoIHave = container.WhatDoIHave();

			IBusControl rabbitBusControl = Bus.Factory.CreateUsingRabbitMq(rabbit =>
			{
				IRabbitMqHost rabbitMqHost = rabbit.Host(new Uri("rabbitmq://localhost:5672/accounting"), settings =>
				{
					settings.Password("accountant");
					settings.Username("accountant");
				});

				rabbit.BusObserver(new BusObserver());

				rabbit.ReceiveEndpoint(rabbitMqHost, "mycompany.domains.queues", conf =>
				{
					
					conf.Consumer<RegisterCustomerConsumer>(container);
					conf.Consumer<RegisterDomainConsumer>();
					conf.UseRetry(Retry.Immediate(5));
					
					//conf.UseRetry(Retry.Filter<Exception>(e => e.Message.IndexOf("We pretend that an exception was thrown") > -1).Immediate(5));
					//conf.UseRetry(Retry.Exponential(5, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(5)));
					//conf.UseRetry(Retry.Intervals(TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(4)));
				});

				rabbit.ReceiveEndpoint(rabbitMqHost, "mycompany.queues.errors.newcustomers", conf =>
				{
					conf.Consumer<RegisterCustomerFaultConsumer>();
				});
			});
			
			rabbitBusControl.Start();
			//rabbitBusControl.ConnectReceiveObserver(new MessageReceiveObserver());
			//rabbitBusControl.ConnectConsumeObserver(new MessageConsumeObserver());
			//rabbitBusControl.ConnectConsumeMessageObserver(new RegisterCustomerMessageObserver());
			Console.ReadKey();

			rabbitBusControl.Stop();

			Console.ReadKey();
		}
	}
}
