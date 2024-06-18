using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    internal interface IEmployeeService
    {
        List<Employee> GetEmployees();
        List<Employee> GetEmployeesManage(int id);
    }
}
