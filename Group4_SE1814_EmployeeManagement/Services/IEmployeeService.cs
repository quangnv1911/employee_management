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
        List<Employee> FilterEmployee(string? name, string? email, string? phone, string? salary, string? department, string? job, string? sortBy, string? sortOrder);
        Task ExportExcel(string? name, string? email, string? phone, string? salary, string? department, string? job, string? sortBy, string? sortOrder);
        List<Employee> FilterListByRole(string role, List<Employee> employees);
        void InsertListEmployee(List<Employee> employees);
        void ImportExcelFile(string filePath);
        int CountTotalEmployee();
        List<Employee> GetEmployeeMaxSalary(DateOnly? from, DateOnly? to);
        int CountNewEmployee(DateOnly? from, DateOnly? to);
    }
}
