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
            employeeRepo = new EmployeeRepo();
        }
        public List<Employee> GetEmployees()
        {
            return employeeRepo.GetAllEmployees();
        }

        public List<Employee> GetEmployeesManage(int id)
        {
            return employeeRepo.GetEmployeesManage(id);
        }
    }
}
