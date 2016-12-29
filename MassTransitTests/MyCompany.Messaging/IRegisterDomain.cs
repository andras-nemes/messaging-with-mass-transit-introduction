using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompany.Messaging
{
	public interface IRegisterDomain
	{
		string Target { get; }
		int Importance { get; } 
	}
}
