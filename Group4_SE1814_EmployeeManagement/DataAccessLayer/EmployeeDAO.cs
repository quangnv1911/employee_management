using BusinessObjects;
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

           return context.Employees.Where(e => e.ManagerId == employeeID).ToList();
        }

        public static List<Employee> GetEmployees()
        {
            EmployeeManagementContext context = new EmployeeManagementContext();

            return context.Employees.ToList();
        }
    }
}
