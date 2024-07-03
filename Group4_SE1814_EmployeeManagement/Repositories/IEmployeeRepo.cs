using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IEmployeeRepo
    {
        List<Employee> GetAllEmployees();
        List<Employee> GetEmployeesManage(int id);
        string? GetEmployeeName(int id);

        Employee? GetEmployeeById(int id);

        void UpdateEmployee(Employee employee);
        void DeleteEmployee(int employeeID);
        void InsertEmployee(Employee employee);
        void InsertListEmployee(List<Employee> employees);
        void ClearTracking(Employee employee);
        int CountTotalEmployee();
    }
}
