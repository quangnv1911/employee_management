using BusinessObjects;
using System;
using Repositories;
using Repositories.Impl;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Impl
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepo employeeRepo;

        public EmployeeService()
        {
            employeeRepo = new EmployeedRepo();
        }

        public void DeleteEmployee(int employeeID)
        {
            employeeRepo.DeleteEmployee(employeeID);
        }

        public Employee? GetEmployeeById(int id)
        {
           return employeeRepo.GetEmployeeById(id);
        }

        public string? GetEmployeeName(int id)
        {
            return employeeRepo.GetEmployeeName(id);
        }

        public List<Employee> GetEmployees()
        {
            return employeeRepo.GetAllEmployees();
        }

        public List<Employee> GetEmployeesManage(int id)
        {
            return employeeRepo.GetEmployeesManage(id);
        }

        public void InsertEmployee(Employee employee)
        {
            employeeRepo.InsertEmployee(employee);  
        }

        public void UpdateEmployee(Employee employee)
        {
            employeeRepo.UpdateEmployee(employee);
        }
    }
}
