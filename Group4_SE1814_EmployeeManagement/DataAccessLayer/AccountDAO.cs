using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class AccountDAO
    {

        public static AccountMember? GetAccountByEmailAndPass(string email, string pass)
        {

            AccountMember? accountMember = new AccountMember();
            EmployeeManagementContext context = new EmployeeManagementContext();
            accountMember = context.AccountMembers.FirstOrDefault(a => a.Email == email && a.Password == pass);
            return accountMember;
        }
    }
}
