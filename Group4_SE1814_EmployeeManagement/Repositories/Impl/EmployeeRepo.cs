using BusinessObjects;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Impl
{
    public class EmployeedRepo : IEmployeeRepo
    {
        public List<Employee> GetAllEmployees()
        {
            return EmployeeDAO.GetEmployees();
        }

        public List<Employee> GetEmployeesManage(int id)
        {
            return EmployeeDAO.GetEmployeesManage(id);
        }

        public string? GetEmployeeName(int id)
        {
            return EmployeeDAO.GetEmployeeName(id);
        }

        public Employee? GetEmployeeById(int id)
        {
            return EmployeeDAO.GetEmployeeById(id);
        }

        public void UpdateEmployee(Employee employee)
        {
            EmployeeDAO.UpdateEmployee(employee);
        }

        public void DeleteEmployee(int employeeID)
        {
            EmployeeDAO.DeleteEmployee(employeeID);
        }

        public void InsertEmployee(Employee employee)
        {
            EmployeeDAO.InsertEmployee(employee);
        }

        public async void InsertListEmployee(List<Employee> employees)
        {
            await EmployeeDAO.InsertListEmployee(employees);
        }

        public void ClearTracking(Employee employee)
        {
            EmployeeDAO.ClearTracking(employee);
        }

        public int CountTotalEmployee()
        {
            return EmployeeDAO.CountTotalEmployee();
        }



        public List<Employee> GetEmployeeMaxSalary(DateOnly? from, DateOnly? to)
        {
            return EmployeeDAO.GetEmployeeMaxSalary(from, to);
        }

        public int CountNewEmployee(DateOnly? from, DateOnly? to)
        {
            return EmployeeDAO.CountNewEmployee(from, to);
        }
    }
}
