using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eatm
{
    enum NormalUserOperationChoice
    {
        ViewAccount,
        CheckBalance,
        WithdrawAmount,
        ChangePin,
        Logout
    }
    enum AdminOperationChoice
    {
        ViewAllAccount,
        DeleteAccount,
        DepositAmount,
        Logout
    }
    class Eatm
    {
        // Data members of Class Eatm declared. Used for atm menu navigating and error handling. 
        private string _cardInput;
        private string _cardInputError;
        private string _loginChoicePrompt;
        private string _loginChoiceError;
        private string _pinInput;
        private string _pinInputError;
        private string _cardNotFound;
        private string _pinCodeInvalid;
        private string _normalUserMenu;
        private string _menuChoice;
        private string _menuChoiceError;
        private string _withdrawAmount;
        private string _withdrawAmountError;
        private string _adminMenu;
        private string _accountSelectToDelete;
        private string _depositAmount;
        private string _depositAmountError;
        private string _accountSelectToDeposit;
        private int _maxWithdrawAmount;

        private List<Account> _accountList; // Declaring Collection Initializer
        private Dictionary<int, int> _numberOfTrasactions; // Declaring Dictionary Initializer

        public void Init() // Init method of class Eatm is called. 
        {
            // Defining class data members inside Init()
           _loginChoicePrompt = @"Login: 
0 => Admin
1 => Normal User
Enter your choice: ";
            _loginChoiceError = "Wrong choice! Please try again";   // 
            _cardInput = "Enter your card number";
            _cardInputError = "Wrong card number! Try again";
            _pinInput = "Enter your pin code";
            _pinInputError = "Wrong pin code! Try again";
            _cardNotFound = "Card number not found! Please try again";
            _pinCodeInvalid = "Pin code doesn't match!";
            _menuChoice = "Enter your choice";
            _menuChoiceError = "Wrong choice! Please try again";
            _withdrawAmount = "Enter withdraw amount";
            _withdrawAmountError = "Invalid amount! Try again";
            _accountSelectToDelete = "Choose account to delete";
            _depositAmount = "Enter deposit amount";
            _depositAmountError = "Invalid amount! Try again";
            _accountSelectToDeposit = "Choose account to deposit";
            _normalUserMenu = @"Operation: 
0 => View Account
1 => Check Balance
2 => Withdraw Amount
3 => Change Pin
4 => Logout";
            _adminMenu = @"Operation: 
0 => View All Account
1 => Delete account
2 => Deposit amount
3 => Logout
";
            _maxWithdrawAmount = 1000; // user cannot withdraw more than 1000 from bank account. 

            _accountList = new List<Account>  // Collection initializers. Initialize members of the Account class        
            {
                new Account() { FullName = "Jack Shepard", CardNumber = 123, PinCode = 1111, Balance = 20000 },
                new Account() { FullName = "Doug Stamper", CardNumber = 456, PinCode = 2222, Balance = 15000 },
                new Account() { FullName = "Rose Dawson", CardNumber = 789, PinCode = 3333, Balance = 29000 }
            };

            _numberOfTrasactions = new Dictionary<int, int>(); // data member initialized as a Dictionary. 
            foreach (var account in _accountList) // var type account initialized that iterates through the list I created above.
            {
                _numberOfTrasactions.Add(account.CardNumber, 0); // storing all Card Numbers of the 3 clients in the dictionary _numberOfTransactions. max withdrawal is 3x
            }
        } // all Card values have been added.

        public void Start()
        {
            var choiceInput = TakeUserInput(_loginChoicePrompt, _loginChoiceError); // Method TakeUserInput is called. See line 334. // First message is displayed on console.
            if (choiceInput == 0) Admin();                                          //0 => Admin
            else if (choiceInput == 1) NormalUser();                                //1 => Normal User
                                                                                    //Enter your choice: ";

            else
            {
                Console.WriteLine(_loginChoiceError);
                Start();
            }
        }

        private void NormalUser() // Method is called if user selects 1 for Normal User.
        {
            Account account = GetNormalUser(); // calls method GetNormalUser. Return of method = account.
            if (account == null) // if GetNormalUser method returned null, error is printed.
            {
                Console.WriteLine(_cardNotFound);
                NormalUser();
            }
            bool isVarified = PinCheck(account); // PinCheck(parameter is GetNormalUser() return). Method is called.
            if (isVarified) NormalUserOperation(account); // if true then NormalUserOperation() method is called. 
            else
            {
                Console.WriteLine(_pinCodeInvalid); // if false, error is printed and NormalUser() method is called. 
                NormalUser();
            }
        }

        private void NormalUserOperation(Account account) // Method to display menu option to user.
        {
            Console.WriteLine(_normalUserMenu);
            var choice = TakeUserInput(_menuChoice, _menuChoiceError);
            switch (choice) // switch statement starts and case is decided on whether user choose: 0,1,2,3,4
            {
                case (int)NormalUserOperationChoice.ViewAccount: // 0 (View Account)
                    ViewNormalAccountDetails(account); 
                    break;
                case (int)NormalUserOperationChoice.CheckBalance: // 1 (Check Balance)
                    CheckBalance(account);
                    break;
                case (int)NormalUserOperationChoice.WithdrawAmount: // 2 (Withdraw Amount)
                    WithdrawAmount(account);
                    break;
                case (int)NormalUserOperationChoice.ChangePin: // 3 (Change Pin)
                    ChangePin(account);
                    break;
                case (int)NormalUserOperationChoice.Logout: // 4 (Log out)
                    Logout();
                    break;
                default:
                    Console.WriteLine(_menuChoiceError);
                    NormalUserOperation(account);
                    break;
            }
        }

        private void Logout() // Method to logout.
        {
            Console.WriteLine("Logout Successfully");
            Console.WriteLine("------------------");
            Start(); // calls Start() method. Displays menu from the beginning of the program.
        }

        private void ChangePin(Account account) // Method that changes pin number for user
        {
            Console.WriteLine("-----Pin code chnage-----");
            var newPinCode = TakeUserInput(_pinInput, _pinInputError);
            account.PinCode = newPinCode; // Pin code now changed
            Console.WriteLine("Pin chnage successfully");
            Console.WriteLine("-----------------------");
            NormalUserOperation(account); // Back to Menu Options
        }

        private void WithdrawAmount(Account account) // Displays Client Withdraw Ammount details.
        {
            bool transactionStatus = CheckTransactionEligibility(account); // Method  CheckTransactionEligibility() called 
            if (!transactionStatus) NormalUserOperation(account); //Goes back to Menu Operation if false

            var amount = TakeUserInput(_withdrawAmount, _withdrawAmountError); // (Amount, error) = variable amount
            int amountStatus = CheckAmountEligibility(account, amount); // Method CheckAmountEligibility() called. Parameters(object, variable).
            if (amountStatus == 0) NormalUserOperation(account); // if withdraw amount = 0

            _numberOfTrasactions[account.CardNumber] += 1; // adds 1 to the number of transactions. User has a total of 3 times to make withdrawls. 
            account.Balance -= amount; // subtracts withdrawl ammount to overall balance.
            Console.WriteLine("You have successfully withdrawn {0}. Your new account balance is {1}", amount, account.Balance);
            NormalUserOperation(account);
        }

        private int CheckAmountEligibility(Account account, int amount) // Method to check that withdraw amount is not greater than $1000
        {
            if (amount > account.Balance) // if withdraw amount exceeds total Balance amount of client.
            {
                Console.WriteLine("You dont have enough balance to make that transaction"); 
                return 0;
            }
            if (amount > _maxWithdrawAmount) // if greater than $1000
            {
                Console.WriteLine("You can't withdraw more than 1000");
                return 0;
            }
            return amount; //else amount is approved and can be withdrawed.
        }

        private bool CheckTransactionEligibility(Account account) // Method to limit number of transactions to 3
        {
            if (_numberOfTrasactions[account.CardNumber] >= 3)
            {
                Console.WriteLine("-------------------------------------------------------");
                Console.WriteLine("You have reached your daily limit of 3 transactions");
                return false;
            }
            return true;
        }

        private void CheckBalance(Account account) // Displays Client Check Balance details.
        {
            Console.WriteLine("----Balance Check----");
            Console.WriteLine("Current Balance: " + account.Balance);
            Console.WriteLine("----------------------");
            NormalUserOperation(account); // goes back to Menu Choice of user. 
        }

        private void ViewNormalAccountDetails(Account account) // Displays Client Account details. Object account accesses client information from Class Account.
        {
            Console.WriteLine("----Account Details-----");
            Console.WriteLine("Full Name: " + account.FullName);
            Console.WriteLine("Card Number: " + account.CardNumber);
            Console.WriteLine("Pin Code: " + account.PinCode);
            Console.WriteLine("Balance: " + account.Balance);
            Console.WriteLine("----------------------");
            NormalUserOperation(account); // goes back to Menu Choice of user. 
        }

        private bool PinCheck(Account account) // method called from NormalUser(). Parameter: is the card number.
        {
            var pinCode = TakeUserInput(_pinInput, _pinInputError); //Calls TakeUserInput(Parameter: card number, and error). When Method returns, pinCode = user input pin code.
            if (pinCode == account.PinCode) return true; // if pinCode matches initialized pin code for the client.
            return false;
        }

        private Account GetNormalUser()
        {
            var cardNumber = TakeUserInput(_cardInput, _cardInputError); // calls TakeUserInput method with (value and error) parameter.
            foreach (Account normalUser in _accountList) // iterating through 3 clients in _accountList. creating object normalUser from Account class.
                if (cardNumber == normalUser.CardNumber) return normalUser; // if input matches the card number initalized with, return card number.
            return null;
        }

        private void Admin() // User selects 0 for admin.
        {
            Console.WriteLine(_adminMenu); // displays menu outlined from line 79
            var choice = TakeUserInput(_menuChoice, _menuChoiceError);
            switch (choice)
            {
                case (int)AdminOperationChoice.ViewAllAccount: // 0 (View all client accounts)
                    ViewAllAccountDetails();
                    break;
                case (int)AdminOperationChoice.DeleteAccount: // 1 (Deletes client account)
                    if (_accountList.Count > 0) DeleteAccount(); // if there are accounts in the list, delete method is called.
                    Console.WriteLine("No account have to delete");
                    Admin();
                    break;
                case (int)AdminOperationChoice.DepositAmount: // 2 (Displays all client Balances in account)
                    DepositAmount();
                    break;
                case (int)AdminOperationChoice.Logout: // 3 (Logout of Admin menu operation)
                    Logout();
                    break;
                default:
                    Console.WriteLine(_menuChoiceError);
                    Admin();
                    break;
            }
        }

        private void DepositAmount() // Method that displays client full Balance in account.
        {
            int i = 1;
            foreach (var account in _accountList) // iterates through all clients accounts and displays info.
            {
                Console.WriteLine(i++ + ". Full Name: {0} Card Number: {1} Pin Code: {2} Balance: {3}", account.FullName, account.CardNumber, account.PinCode, account.Balance);
            }
            var serial = TakeUserInput(_accountSelectToDeposit, _menuChoiceError) - 1;
            if (serial < 0) DepositAmount(); // checks to see if a negitive was inputed.
            if (_accountList.Count > serial)
            {
                var amount = TakeUserInput(_depositAmount, _depositAmountError); // method calls for admin to input deposit amount
                _accountList[serial].Balance = _accountList[serial].Balance + amount; // balance updated with deposit amount.
                Console.WriteLine("You have successfully deposit {0}. New account balance is {1}", amount, _accountList[serial].Balance);
                Admin(); // Back to Admin menu option
            }
            else
            {
                Console.WriteLine(_menuChoiceError);
                Admin();
            }

        }

        private void DeleteAccount() // Method to delete client account.
        {
            int i = 1;
            foreach (var account in _accountList) // iterates to display all clients for admin to select.
            {
                Console.WriteLine(i++ + ". Full Name: {0} Card Number: {1} Pin Code: {2} Balance: {3}", account.FullName, account.CardNumber, account.PinCode, account.Balance);
            }
            var serial = TakeUserInput(_accountSelectToDelete, _menuChoiceError) - 1;
            if (serial < 0) DeleteAccount(); // checking to see if input is negative
            if (_accountList.Count > serial)
            {
                _accountList.RemoveAt(serial); // removes client account
                Console.WriteLine("Account deleted successfully");
                Admin();
            }
            else
            {
                Console.WriteLine(_menuChoiceError);
                Admin();
            }
        }

        private void ViewAllAccountDetails() // Method that displays accounts, admin chooses which account to deposit funds into.
        {
            Console.WriteLine("----------------------All Account Details-----------------------");
            foreach (var account in _accountList) // iterates through all client account details.
            {
                Console.WriteLine("Full Name: {0} Card Number: {1} Pin Code: {2} Balance: {3}", account.FullName, account.CardNumber, account.PinCode, account.Balance);
            }
            Console.WriteLine("------------------------------------------------------------------\n");
            Admin(); // Back to admin menu options
        }

        private int TakeUserInput(string prompt, string error) // Printing method
        {
            Console.WriteLine(prompt);
            var input = Console.ReadLine(); // prompt assigned to input.
            try
            {
                return Convert.ToInt32(input);// converts input to Int
            }
            catch (Exception)
            {
                Console.WriteLine(error);
                return TakeUserInput(prompt, error);
            }
        }
    }
}
