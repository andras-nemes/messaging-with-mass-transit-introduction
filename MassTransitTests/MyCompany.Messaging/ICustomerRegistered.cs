using System;

namespace MyCompany.Messaging
{
	public interface ICustomerRegistered
	{
		Guid Id { get; }
		DateTime RegisteredUtc { get; }
		string Name { get; }
		string Address { get; }
	}
}
