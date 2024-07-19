using BusinessObjects;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Impl
{
    public class AccountRepo : IAccountRepo
    {
        public string ChangePass(string email)
        {
            return AccountDAO.ChangePass(email);
        }

        public bool CheckAccountExist(string email)
        {
            return AccountDAO.CheckAccountExist(email);
        }

        public AccountMember? GetAccountByEmailAndPass(string email, string pass)
        {
            return AccountDAO.GetAccountByEmailAndPass(email, pass);
        }
    }
}
