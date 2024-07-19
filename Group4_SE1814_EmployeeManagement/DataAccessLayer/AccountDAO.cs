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

        public static bool CheckAccountExist(string email)
        {
            AccountMember? accountMember = new AccountMember();
            EmployeeManagementContext context = new EmployeeManagementContext();

            accountMember = context.AccountMembers.FirstOrDefault(a => a.Email.Equals(email));
            if (accountMember != null)
            {
                return true;
            }
            return false;
        }

        public static string ChangePass(string email)
        {
            AccountMember? accountMember = new AccountMember();
            EmployeeManagementContext context = new EmployeeManagementContext();

            accountMember = context.AccountMembers.FirstOrDefault(a => a.Email.Equals(email));
            if (accountMember != null)
            {
                int length = 10;
                string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+";
                var random = new Random();
                string password = new string(Enumerable.Repeat(chars, length)
                  .Select(s => s[random.Next(s.Length)]).ToArray());
                accountMember.Password = password;
                context.AccountMembers.Update(accountMember);
                context.SaveChanges();
                return password;
            }
            return null;
        }
    }
}
