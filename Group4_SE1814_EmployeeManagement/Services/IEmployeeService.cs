using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IEmployeeService
    {
        List<Employee> GetEmployees();
        List<Employee> GetEmployeesManage(int id);
        string? GetEmployeeName(int id);
        Employee? GetEmployeeById(int id);
        void UpdateEmployee(Employee employeeID);
        void DeleteEmployee(int employeeID);
        void InsertEmployee(Employee employee);
    }
}
