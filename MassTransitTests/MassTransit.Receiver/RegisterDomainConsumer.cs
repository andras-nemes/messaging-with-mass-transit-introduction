using MyCompany.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassTransit.Receiver
{
	public class RegisterDomainConsumer : IConsumer<IRegisterDomain>
	{
		public Task Consume(ConsumeContext<IRegisterDomain> context)
		{
			Console.WriteLine(string.Concat("New domain registered. Target and importance: ", 
				context.Message.Target, " / ", context.Message.Importance));
			return Task.FromResult<IRegisterDomain>(context.Message);
		}
	}
}
