using MyCompany.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassTransit.Receiver
{
	public class WorldNewsConsumer : IConsumer<IWorldNews>
	{
		public Task Consume(ConsumeContext<IWorldNews> context)
		{
			IWorldNews news = context.Message;
			Console.WriteLine("Latest news: " + news.Message);
			return Task.FromResult(context.Message);
		}
	}
}
