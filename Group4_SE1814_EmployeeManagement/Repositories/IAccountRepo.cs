﻿using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IAccountRepo
    {
        AccountMember? GetAccountByEmailAndPass(string email, string pass);
        bool CheckAccountExist(string email);
        string ChangePass(string email);
    }
}
