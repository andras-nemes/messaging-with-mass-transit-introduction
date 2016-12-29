using MyCompany.Domains;
using System;

namespace MyCompany.Repository.Dummy
{
	public class CustomerRepository : ICustomerRepository
	{
		public void Save(Customer customer)
		{
			Console.WriteLine(string.Concat("The concrete customer repository was called for customer ", customer.Name));
		}
	}
}
