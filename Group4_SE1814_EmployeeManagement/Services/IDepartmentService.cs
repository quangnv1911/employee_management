using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IDepartmentService
    {
        List<Department> GetDepartments();

        void InsertDepartment(Department department);

        void UpdateDepartment(Department department);

        void DeleteDepartment(int departmentId);

        Department? GetDepartmentById(int id);
        List<Department> FilterDepartments(string? departmentName,
           string? location, string? managerName, string? sortBy, string? sortOrder);
        Task ExportExcel(string? departmentName,
           string? location, string? managerName, string? sortBy, string? sortOrder);

        void InsertListDepartment(List<Department> departments);
        void ImportExcelFile(string filePath);
        int CountTotalDepartment();
    }
}
