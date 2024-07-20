using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class EmployeeDAO
    {
        // Lấy danh sách những nhân viên thuộc quyền quản lí của người đăng kí
        public static List<Employee> GetEmployeesManage(int employeeID)
        {
            EmployeeManagementContext context = new EmployeeManagementContext();

            return context.Employees.Where(e => e.ManagerId == employeeID)
                 .Include(e => e.Job).Include(e => e.Manager).Include(e => e.Department)
                 .AsNoTracking()
                 .ToList();
        }

        public static int CountNewEmployee(DateOnly? from, DateOnly? to)
        {
            EmployeeManagementContext context = new EmployeeManagementContext();

            var listEmployee = context.Employees.Include(d => d.Department).ToList();
            if (from != null)
            {
                listEmployee = listEmployee.Where(d => d.HireDate >= from).ToList();
            }
            if (to != null)
            {
                listEmployee = listEmployee.Where(d => d.HireDate <= to).ToList();
            }
            return listEmployee.Count;
        }

        public static List<Employee> GetEmployees()
        {
            EmployeeManagementContext context = new EmployeeManagementContext();

            return context.Employees
                .Include(e => e.Job).Include(e => e.Manager).Include(e => e.Department)
                .AsNoTracking()
                .ToList();
        }

        public static List<Employee> GetEmployeeMaxSalary(DateOnly? from, DateOnly? to)
        {
            EmployeeManagementContext context = new EmployeeManagementContext();

            var listEmployee = context.Employees.Include(d => d.Department).ToList();
            if(from != null)
            {
                listEmployee = listEmployee.Where(d => d.HireDate >= from).ToList();
            }
            if(to != null)
            {
                listEmployee = listEmployee.Where(d => d.HireDate <= to).ToList();  
            }
            return listEmployee
                .OrderByDescending(d => d.Salary).Take(5).ToList();
        }

        public static Employee? GetEmployeHighestSalary(DateOnly? from, DateOnly? to)
        {
            EmployeeManagementContext context = new EmployeeManagementContext();

            var listEmployee = context.Employees.Include(d => d.Department).ToList();
            if (from != null)
            {
                listEmployee = listEmployee.Where(d => d.HireDate >= from).ToList();
            }
            if (to != null)
            {
                listEmployee = listEmployee.Where(d => d.HireDate <= to).ToList();
            }
            return listEmployee
                .OrderByDescending(d => d.Salary).FirstOrDefault();
        }

        public static List<Employee> GetEmployeeMaxSalary()
        {
            EmployeeManagementContext context = new EmployeeManagementContext();

            return context.Employees.Include(d => d.Department)
                .OrderByDescending(d => d.Salary).Take(5).ToList();
        }

        public static string? GetEmployeeName(int employeeID)
        {
            EmployeeManagementContext context = new EmployeeManagementContext();

            return context.Employees.Where(e => e.EmployeeId == employeeID).Select(e => e.FirstName).First();
        }

        public static Employee? GetEmployeeById(int employeeID)
        {
            EmployeeManagementContext context = new EmployeeManagementContext();
            return context.Employees
                .Include(e => e.Job).Include(e => e.Manager).Include(e => e.Department).FirstOrDefault(e => e.EmployeeId == employeeID);
        }

        public static void DeleteEmployee(int employeeID)
        {
            EmployeeManagementContext context = new EmployeeManagementContext();
            var employee = context.Employees.FirstOrDefault(e => e.EmployeeId == employeeID);
            context.Employees.Remove(employee);
            context.SaveChanges();
        }

        public static void InsertEmployee(Employee employee)
        {
            EmployeeManagementContext context = new EmployeeManagementContext();
            context.Employees.Add(employee);
            context.SaveChanges();
        }

        public static void UpdateEmployee(Employee employeeUpdate)
        {
            EmployeeManagementContext context = new EmployeeManagementContext();
            Employee employee = context.Employees.FirstOrDefault(e => e.EmployeeId == employeeUpdate.EmployeeId);
            if (employee != null)
            {
                employee = employeeUpdate;
                context.SaveChanges();
            }
        }

        public static async Task InsertListEmployee(List<Employee> employees)
        {
            EmployeeManagementContext context = new EmployeeManagementContext();
            await context.Employees.AddRangeAsync(employees);
            await context.SaveChangesAsync();
        }

        public static void ClearTracking(Employee employee)
        {
            EmployeeManagementContext context = new EmployeeManagementContext();
            context.Entry(employee).State = EntityState.Unchanged;
        }

        public static int CountTotalEmployee()
        {
            EmployeeManagementContext context = new EmployeeManagementContext();
            return context.Employees.Count();
        }
    }
}
