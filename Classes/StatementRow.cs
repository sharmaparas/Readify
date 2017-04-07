using ReadifyBank.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadifyBank
{
    class StatementRow : IStatementRow
    {
        public StatementRow(IAccount account, decimal amount, decimal balance, DateTimeOffset date, string description)
        {
            Account = account;
            Amount = amount;
            Balance = balance;
            Date = date;
            Description = description; 
        }
        public IAccount Account { get; set; }
        public decimal Amount { get; set; }

        public decimal Balance { get; set; }
        public DateTimeOffset Date { get; set; }

        public string Description { get; set; }
    }
}
