﻿using BusinessObjects;
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
                .ToList();
        }

        public static List<Employee> GetEmployees()
        {
            EmployeeManagementContext context = new EmployeeManagementContext();

            return context.Employees
                .Include(e => e.Job).Include(e => e.Manager).Include(e => e.Department)
                .ToList();
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
            if(employee != null)
            {
                employee = employeeUpdate;
                context.SaveChanges();
            }
        }
    }
}
