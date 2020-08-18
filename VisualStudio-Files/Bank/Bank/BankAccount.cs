using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    class BankAccount
    {



        private int AccountNo;
        private double Balance;

        public BankAccount(int accountno, double balance)
        {
            this.AccountNo = accountno;
            this.Balance = balance;
        }

        public double ReturnBalance()
        {
            return Balance;
        }
        public double UpdateBalance()
        {
            return Balance;
        }
    }
}
