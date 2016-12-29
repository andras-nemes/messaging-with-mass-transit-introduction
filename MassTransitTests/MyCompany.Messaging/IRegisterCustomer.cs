using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompany.Messaging
{
	public interface IRegisterCustomer : IRegisterDomain
	{
		Guid Id { get; }
		DateTime RegisteredUtc { get; }
		int Type { get; }
		string Name { get; }
		bool Preferred { get; }
		decimal DefaultDiscount { get; }
		string Address { get; }
	}
}
