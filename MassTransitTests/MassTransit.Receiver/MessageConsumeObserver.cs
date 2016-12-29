using MassTransit.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassTransit.Receiver
{
	public class MessageConsumeObserver : IConsumeObserver
	{
		private readonly string _logger = "[MessageConsumeObserver]";
		public async Task ConsumeFault<T>(ConsumeContext<T> context, Exception exception) where T : class
		{			
			Console.WriteLine(string.Concat(_logger, ": A fault is consumed for type ", 
				context.Message.GetType(), ", exception message: ", exception.Message));
			await context.CompleteTask;
		}

		public async Task PostConsume<T>(ConsumeContext<T> context) where T : class
		{
			Console.WriteLine(string.Concat(_logger, ": A message is past the consume stage for type ", context.Message.GetType()));
			await context.CompleteTask;
		}

		public async Task PreConsume<T>(ConsumeContext<T> context) where T : class
		{
			Console.WriteLine(string.Concat(_logger, ": A message is before the consume stage for type ", context.Message.GetType()));
			await context.CompleteTask;
		}
	}
}
