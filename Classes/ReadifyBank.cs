using ReadifyBank.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadifyBank
{
    class ReadifyBank : IReadifyBank
    {
        public ReadifyBank()
        {
            AccountList = new List<IAccount>();
            TransactionLog = new List<IStatementRow>();
        }
        public IList<IAccount> AccountList { get; set; }
        public IList<IStatementRow> TransactionLog { get; set; }

        public int HomeLoanAccountNumber = 0;

        public int SavingsAccountNumber = 0;

        public decimal CalculateInterestToDate(IAccount account, DateTimeOffset toDate)
        {
            if (account.AccountNumber.ToUpper().Contains("LN"))
            {
                /*since interest rate of Loan account is 3.99% per year*/
                decimal interest = (account.Balance * 399 * (toDate.Year - account.OpenedDate.Year)) / 10000;
                return interest;
            }
            else if (account.AccountNumber.ToUpper().Contains("SV"))
            {
                /*since interest rate of savings account is 6% per month*/
                return (account.Balance * 6 * ((toDate.Month - account.OpenedDate.Month))) / 100;
            }
            else
            {
                Console.WriteLine("Invalid account number");
                return 0;
            }
        }

        public IEnumerable<IStatementRow> CloseAccount(IAccount account, DateTimeOffset closeDate)
        {
            AccountList.Remove(account);
            IList<IStatementRow> accountTransactionLog = new List<IStatementRow>();
            foreach (IStatementRow r in TransactionLog)
            {
                if (r.Account.AccountNumber == account.AccountNumber)
                {
                    accountTransactionLog.Add(r);
                }
            }
            return accountTransactionLog;
        }

        public decimal GetBalance(IAccount account)
        {
            return account.Balance;
        }

        public IEnumerable<IStatementRow> GetMiniStatement(IAccount account)
        {
            List<IStatementRow> ministatement = new List<IStatementRow>();
            int i = TransactionLog.Count() - 1;
            int count = 0;
            while (i >= 0 && count < 5)
            {
                if (TransactionLog[i].Account.AccountNumber == account.AccountNumber)
                {
                    ministatement.Add(TransactionLog[i]);
                    count++;
                }
                i--;
            }
            if(ministatement.Count == 0)
            {
                Console.WriteLine("No activity on the account yet");
            }
            return ministatement;
        }

        public IAccount OpenHomeLoanAccount(string customerName, DateTimeOffset openDate)
        {
            HomeLoanAccountNumber++;
            var accountBalance = 0;
            var accountNumber = HomeLoanAccountNumber.ToString();
            switch (accountNumber.Length)
            {
                case 1:
                    accountNumber = "00000" + accountNumber;
                    break;
                case 2:
                    accountNumber = "0000" + accountNumber;
                    break;
                case 3:
                    accountNumber = "000" + accountNumber;
                    break;
                case 4:
                    accountNumber = "00" + accountNumber;
                    break;
                case 5:
                    accountNumber = "0" + accountNumber;
                    break;
                default:
                    break;
            }
            accountNumber = "LN-" + accountNumber;
            IAccount account = new Account(accountNumber, accountBalance, customerName, openDate);
            AccountList = new List<IAccount>();
            AccountList.Add(account);
            TransactionLog.Add(new StatementRow(account, account.Balance, account.Balance, account.OpenedDate, "Account opened"));
            return account;
        }

        public IAccount OpenSavingsAccount(string customerName, DateTimeOffset openDate)
        {
            SavingsAccountNumber++;
            var accountBalance = 0;
            var accountNumber = SavingsAccountNumber.ToString();
            switch (accountNumber.Length)
            {
                case 1:
                    accountNumber = "00000" + accountNumber;
                    break;
                case 2:
                    accountNumber = "0000" + accountNumber;
                    break;
                case 3:
                    accountNumber = "000" + accountNumber;
                    break;
                case 4:
                    accountNumber = "00" + accountNumber;
                    break;
                case 5:
                    accountNumber = "0" + accountNumber;
                    break;
                default:
                    break;
            }
            accountNumber = "SV-" + accountNumber;
            IAccount account = new Account(accountNumber, accountBalance, customerName, openDate);
            AccountList.Add(account);
            TransactionLog.Add(new StatementRow(account, account.Balance, account.Balance, account.OpenedDate, "Account opened"));
            return account;
        }

        public void PerformDeposit(IAccount account, decimal amount, string description, DateTimeOffset depositDate)
        {
            int indexofaccount = AccountList.IndexOf(account);
            IAccount updateAccount = new Account(account.AccountNumber, account.Balance + amount, account.CustomerName, account.OpenedDate);
            AccountList[indexofaccount] = updateAccount;
            IStatementRow newstatement = new StatementRow(account, amount, updateAccount.Balance, depositDate, "Deposit Description - " + description);
            TransactionLog.Add(newstatement);
        }

        public void PerformTransfer(IAccount from, IAccount to, decimal amount, string description, DateTimeOffset transferDate)
        {
            if (from.Balance >= amount)
            {
                int indexOfFromAccount = AccountList.IndexOf(from);
                int indexOfToAccount = AccountList.IndexOf(to);
                IAccount updateFromAccount = new Account(from.AccountNumber, from.Balance - amount, from.CustomerName, from.OpenedDate);
                IAccount updateToAccount = new Account(to.AccountNumber, to.Balance + amount, to.CustomerName, to.OpenedDate);
                AccountList[indexOfToAccount] = updateToAccount;
            }
            else
            {
                Console.WriteLine("Transfer not possible due to low balance");
            }
        }

        public void PerformWithdrawal(IAccount account, decimal amount, string description, DateTimeOffset withdrawalDate)
        {
            if (account.Balance > amount)
            {
                int indexofaccount = AccountList.IndexOf(account);
                IAccount updateAccount = new Account(account.AccountNumber, account.Balance - amount, account.CustomerName, account.OpenedDate);
                AccountList[indexofaccount] = updateAccount;
                IStatementRow newstatement = new StatementRow(account, amount, updateAccount.Balance, withdrawalDate, "Withdrawal Description -" + description);
                TransactionLog.Add(newstatement);
            }
            else
            {
                Console.WriteLine("Balance is low for withdrawal");
            }
        }
    }
}
