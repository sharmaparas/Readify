using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadifyBank.Interfaces;

namespace ReadifyBank
{
    class Program
    {
        static void Main(string[] args)
        {
            ReadifyBank Bank = new ReadifyBank();
            //Just some sample accounts
            List<Account> SeedAcc = new List<Account> {
                new Account("SV-100000", 5000, "PARAS", DateTimeOffset.Now.AddMonths(-1)),
                new Account("SV-200000", 23000, "JAMES", DateTimeOffset.Now.AddMonths(-25)),
                new Account("LN-100000", 10000, "BEN", DateTimeOffset.Now.AddYears(-1)),
                new Account("LN-100000", 50000, "SAHIL", DateTimeOffset.Now.AddYears(-3))
            };
            foreach (Account a in SeedAcc)
            {
                Bank.AccountList.Add(a);
            }
            var end = false;
            while (!end)
            {
                //I have not yet created a database so please enter some accounts info when you start.
                Console.WriteLine("Welcome to ReadifyBank");
                Console.WriteLine(new string('-', 35));
                Console.WriteLine("Select an option \n1) Open Home Loan Account \n2) Open Savings Account \n3) Existing account \n4) Exit");
                Console.WriteLine(new string('-', 35));
                int Choice = 0;
                int.TryParse(Console.ReadLine(), out Choice);
                Console.Clear();
                switch (Choice)
                {
                    case 1:
                        {
                            Console.WriteLine("Enter Customer Name");
                            var Name = Console.ReadLine();
                            var Date = DateTimeOffset.Now;
                            var acc = Bank.OpenHomeLoanAccount(Name, Date);
                            Console.WriteLine("Account successfully opened");
                            DisplayAccount(acc);
                        }
                        break;
                    case 2:
                        {
                            Console.WriteLine("Enter Customer Name");
                            var Name = Console.ReadLine();
                            var Date = DateTimeOffset.Now;
                            var acc = Bank.OpenSavingsAccount(Name, Date);
                            Console.WriteLine("Account successfully opened");
                            DisplayAccount(acc);
                        }
                        break;
                    case 3:
                        {
                            // I have kept it simple currently so you have to enter the full account number like LN-000001
                            Console.WriteLine("Enter account number");
                            string AccNum = Console.ReadLine();
                            if (Bank.AccountList.Where(a => a.AccountNumber == AccNum).Any())
                            {
                                AccountOptions(AccNum, Bank);
                            }
                            else
                            {
                                Console.WriteLine("Account Number \"{0}\" could not be found", AccNum);
                            }
                        }
                        break;
                    case 4:
                        end = true;
                        break;
                }
            }

        }

        private static void DisplayAccount(IAccount acc)
        {
            Console.WriteLine("Customer Name:    {0}", acc.CustomerName);
            Console.WriteLine("Account Number:   {0}", acc.AccountNumber);
            Console.WriteLine("Account Balance:  {0}", acc.Balance);
            Console.WriteLine("Account Opened on {0}", acc.OpenedDate);
        }

        private static void AccountOptions(string accNum, ReadifyBank Bank)
        {

            var end = false;
            while (!end)
            {
                try
                {
                    var account = Bank.AccountList.Where(a => a.AccountNumber == accNum).First();
                    Console.WriteLine("Customer Details");
                    Console.WriteLine(new string('-', 35));
                    Console.WriteLine("1) Deposit \n2) Withdrawal \n3) Transfer \n4) Balance \n5) Calculate Interest \n6) Mini Statement \n7) Close Account \n8) Back");
                    Console.WriteLine(new string('-', 35));
                    int Choice = 0;
                    int.TryParse(Console.ReadLine(), out Choice);
                    Console.Clear();
                    switch (Choice)
                    {
                        case 1:
                            Console.WriteLine("Enter deposit amount");
                            decimal deposit = 0;
                            if (decimal.TryParse(Console.ReadLine(), out deposit))
                            {
                                Console.WriteLine("Enter discription");
                                var discription = Console.ReadLine();
                                Bank.PerformDeposit(account, deposit, discription, DateTimeOffset.Now);
                                Console.Clear();
                            }
                            else
                            {
                                Console.WriteLine("Deposit not possible amount is invalid");
                            }
                            break;
                        case 2:
                            Console.WriteLine("Enter withdrawal amount");
                            decimal withdrawal = 0;
                            if (decimal.TryParse(Console.ReadLine(), out withdrawal))
                            {
                                Console.WriteLine("Enter discription");
                                var discription = Console.ReadLine();
                                Bank.PerformWithdrawal(account, withdrawal, discription, DateTimeOffset.Now);
                                Console.Clear();
                            }
                            else
                            {
                                Console.WriteLine("Withdrawal not possible amount is invalid");
                            }
                            break;
                        case 3:
                            Console.WriteLine("Enter account number to which you wish to transfer funds");
                            var accnum2 = Console.ReadLine();
                            if (Bank.AccountList.Where(a => a.AccountNumber == accnum2).Any())
                            {
                                Console.WriteLine("Enter transfer amount");
                                decimal transferamt = 0;
                                if (decimal.TryParse(Console.ReadLine(), out transferamt))
                                {
                                    Console.WriteLine("Enter discription");
                                    var discription = Console.ReadLine();
                                    var account2 = Bank.AccountList.Where(a => a.AccountNumber == accnum2).First();
                                    Bank.PerformTransfer(account, account2, transferamt, discription, DateTimeOffset.Now);
                                }
                                else
                                {
                                    Console.WriteLine("Transfer not possible invalid amount");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid account number");
                            }
                            break;
                        case 4:
                            DisplayAccount(account);
                            break;
                        case 5:
                            var interest = Bank.CalculateInterestToDate(account, DateTimeOffset.Now);
                            Console.WriteLine("Interest to date for {0} - {1}", account.AccountNumber, interest);
                            break;
                        case 6:
                            IEnumerable<IStatementRow> ministatement = Bank.GetMiniStatement(account);
                            foreach (IStatementRow sr in ministatement)
                            {
                                Console.WriteLine("Account Number - {0} \nDate - {1} \nAmount - {2} \nBalance -{3} \nDiscription - {4}", sr.Account.AccountNumber, sr.Date, sr.Amount, sr.Balance, sr.Description);
                                Console.WriteLine(new string('*', 35));
                            }
                            Console.Clear();
                            break;
                        case 7:
                            //Press y for yes
                            Console.WriteLine("Are you sure you want to delete this account?\nYes(Y)");
                            var choice = Console.ReadLine();
                            if (choice.ToUpper().Equals("Y"))
                            {
                                IEnumerable<IStatementRow> statement = Bank.CloseAccount(account, DateTimeOffset.Now);
                                foreach(IStatementRow sr in statement)
                                {
                                    Console.WriteLine("Account Number - {0} \nDate - {1} \nAmount - {2} \nBalance -{3} \nDiscription - {4}", sr.Account.AccountNumber, sr.Date, sr.Amount, sr.Balance, sr.Description);
                                    Console.WriteLine(new string('*', 35));
                                }
                                Console.WriteLine("Account closed");
                                end = true;
                                Console.Clear();
                            }
                            break;
                        case 8:
                            end = true;
                            break;

                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message.ToString());
                }
            }
        }
    }
}
