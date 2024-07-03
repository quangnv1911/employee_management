using BusinessObjects;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Impl
{
    public class DepartmentRepo : IDepartmentRepo
    {
        public void ClearTracking(Department department)
        {
           DepartmentDAO.ClearTracking(department);
        }

        public int CountTotalDepartment()
        {
            return DepartmentDAO.CountTotalDepartment();
        }

        public void DeleteDepartment(int departmentId)
        {
            DepartmentDAO.DeleteDepartment(departmentId);
        }

        public Department? GetDepartmentById(int id)
        {
            return DepartmentDAO.GetDepartmentById(id);
        }

        public List<Department> GetDepartments()
        {
            return DepartmentDAO.GetDepartments();
        }

        public void InsertDepartment(Department department)
        {
            DepartmentDAO.InsertDepartment(department);
        }

        public void InsertListDepartment(List<Department> departments)
        {
            DepartmentDAO.InsertListDepartment(departments);
        }

        public void UpdateDepartment(Department department)
        {
            DepartmentDAO.UpdateDepartment(department);
        }
    }
}
