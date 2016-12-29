using MassTransit.Pipeline;
using MyCompany.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassTransit.Receiver
{
	public class RegisterCustomerMessageObserver : IConsumeMessageObserver<IRegisterCustomer>
	{
		public Task ConsumeFault(ConsumeContext<IRegisterCustomer> context, Exception exception)
		{
			throw new NotImplementedException();
		}

		public Task PostConsume(ConsumeContext<IRegisterCustomer> context)
		{
			throw new NotImplementedException();
		}

		public Task PreConsume(ConsumeContext<IRegisterCustomer> context)
		{
			throw new NotImplementedException();
		}
	}
}
