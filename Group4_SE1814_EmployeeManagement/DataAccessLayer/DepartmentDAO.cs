using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class DepartmentDAO
    {
        public static List<Department> GetDepartments()
        {
            EmployeeManagementContext context = new EmployeeManagementContext();

            return context.Departments.Include(d => d.Manager).Include(d => d.Location).ThenInclude(d => d.Country).ToList();
        }

        public static Department? GetDepartmentById(int id)
        {
            EmployeeManagementContext context = new EmployeeManagementContext();
            return context.Departments.Include(d => d.Manager).Include(d => d.Location).ThenInclude(d => d.Country).FirstOrDefault(d => d.DepartmentId == id);
        }

        public static void InsertDepartment(Department department)
        {
            EmployeeManagementContext context = new EmployeeManagementContext();
            context.Departments.Add(department);
            context.SaveChanges();
        }

        public static void UpdateDepartment(Department department)
        {
            EmployeeManagementContext context = new EmployeeManagementContext();
            context.Departments.Update(department);
            context.SaveChanges();
        }

        public static void DeleteDepartment(int departmentId)
        {
            EmployeeManagementContext context = new EmployeeManagementContext();
            var department = context.Departments.FirstOrDefault(d => d.DepartmentId == departmentId);
            context.Departments.Remove(department);
            context.SaveChanges();
        }

        public static void InsertListDepartment(List<Department> departments)
        {
            EmployeeManagementContext context = new EmployeeManagementContext();
            context.Departments.AddRange(departments);
            context.SaveChanges();
        }

        public static void ClearTracking(Department department)
        {
            EmployeeManagementContext context = new EmployeeManagementContext();
            context.Entry(department).State = EntityState.Unchanged;
        }

        public static int CountTotalDepartment()
        {
            EmployeeManagementContext context = new EmployeeManagementContext();
            return context.Departments.Count();
        }
    }
}
