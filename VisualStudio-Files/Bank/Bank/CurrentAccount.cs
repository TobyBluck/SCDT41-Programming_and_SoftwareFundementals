using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{

     

    class CurrentAccount:BankAccount
    {
        public double CalculateCharges()
        {
            return UpdateBalance();
        }
        public double CalculateInterest()
        {
            return ReturnBalance() * 0.12;
        }

        public Branch branch;

        public CurrentAccount(int accountnumber, double balance, Branch branch): base(accountnumber, balance)
        {

             

        }

    }
}
