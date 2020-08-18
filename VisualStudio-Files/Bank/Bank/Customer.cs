using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    class Customer
    {
        private string CustomerName;
        private string Address;

        private CurrentAccount myAccount; 

        public Customer(string name, string address, CurrentAccount account)
        {
            this.CustomerName = name;
            this.Address = address; 

        }

        public void ShowDetails()
        {
            Console.WriteLine("Customer name {0}", CustomerName);
            Console.WriteLine("Customer address {0}", Address);
            Console.WriteLine("Customer balance £{0}", myAccount.ReturnBalance());
        }

    }
}
