using BusinessObjects;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Impl
{
    public class EmployeeRepo : IEmployeeRepo
    {
        public List<Employee> GetAllEmployees()
        {
            return EmployeeDAO.GetEmployees();
        }

        public List<Employee> GetEmployeesManage(int id)
        {
            return EmployeeDAO.GetEmployeesManage(id);
        }
    }
}
