using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IDepartmentRepo
    {
        List<Department> GetDepartments();
        void InsertDepartment(Department department);

        void UpdateDepartment(Department department);

        void DeleteDepartment(int departmentId);

        Department? GetDepartmentById(int id);
        void InsertListDepartment(List<Department> departments);
        void ClearTracking(Department department);
        int CountTotalDepartment();
    }
}
