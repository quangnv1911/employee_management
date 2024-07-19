using BusinessObjects;
using Repositories;
using Repositories.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Impl
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepo accountRepo;
        public AccountService()
        {
            accountRepo = new AccountRepo();
        }

        public string ChangePass(string email)
        {
            return accountRepo.ChangePass(email);
        }

        public bool CheckAccountExist(string email)
        {
            return accountRepo.CheckAccountExist(email);
        }

        public AccountMember GetAccountByEmailAndPass(string email, string pass)
        {
           return accountRepo.GetAccountByEmailAndPass(email, pass);
        }
    }
}
