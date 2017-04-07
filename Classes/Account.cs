using ReadifyBank.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadifyBank
{
    class Account : IAccount
    {
        public Account(string accountNumber, decimal balance, string customerName, DateTimeOffset openedDate)
        {
            AccountNumber = accountNumber;
            Balance = balance;
            CustomerName = customerName;
            OpenedDate = openedDate;
        }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public string CustomerName { get; set; }
        public DateTimeOffset OpenedDate { get; set; }
    }
}
