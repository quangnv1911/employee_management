using BusinessObjects;
using Repositories.Impl;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Globalization;
using OfficeOpenXml.Style;
using OfficeOpenXml;

namespace Services.Impl
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepo departmentRepo;
        public DepartmentService()
        {
            departmentRepo = new DepartmentRepo();
        }
        public List<Department> GetDepartments()
        {
            return departmentRepo.GetDepartments();
        }


        public List<Department> FilterDepartments(string? departmentName,
            string? location, string? managerName, string? sortBy, string? sortOrder)
        {
            List<Department> departments = GetDepartments();
            if (!string.IsNullOrEmpty(departmentName))
            {
                departments = departments
                    .Where(d => d.DepartmentName.Contains(departmentName)).ToList();

            }
            if (!string.IsNullOrEmpty(location))
            {
                departments = departments
                    .Where(d => d.Location.StreetAddress.Contains(location) || d.Location.City.Contains(location) || d.Location.Country.CountryName.Contains(location) ||
                    d.Location.Country.Region.RegionName.Contains(location)).ToList();
            }

            if (!string.IsNullOrEmpty(managerName))
            {
                departments = departments
                    .Where(d => d.Manager.FirstName.Contains(managerName) || d.Manager.LastName.Contains(managerName)).ToList();

            }

            if (!string.IsNullOrEmpty(sortBy) && !string.IsNullOrEmpty(sortOrder))
            {
                switch (sortBy)
                {
                    case "Department Name":
                        departments = sortOrder == "Ascending" ? departments.OrderBy(c => c.DepartmentName).ToList() : departments.OrderByDescending(c => c.DepartmentName).ToList();
                        break;

                    case "Manager Name":
                        departments = sortOrder == "Ascending" ? departments.OrderBy(c => c.Manager.FirstName).ToList() : departments.OrderByDescending(c => c.Manager.FirstName).ToList();
                        break;
                    case "Location(City)":
                        departments = sortOrder == "Ascending" ? departments.OrderBy(c => c.Location.City).ToList() : departments.OrderByDescending(c => c.Location.City).ToList();
                        break;
                    default:
                        break;
                }
            }

            return departments;
        }

        public void InsertDepartment(Department department)
        {
            departmentRepo.InsertDepartment(department);
        }

        public void UpdateDepartment(Department department)
        {
            departmentRepo.UpdateDepartment(department);
        }

        public void DeleteDepartment(int departmentId)
        {
            departmentRepo.DeleteDepartment(departmentId);
        }

        public Department? GetDepartmentById(int id)
        {
            return departmentRepo.GetDepartmentById(id);
        }

        public async Task ExportExcel(string? departmentName, string? location, string? managerName, string? sortBy, string? sortOrder)
        {
            List<Department> departments = FilterDepartments(departmentName,location, managerName,sortBy, sortOrder);

            ExcelPackage.LicenseContext = LicenseContext.Commercial;

            // If you use EPPlus in a noncommercial context
            // according to the Polyform Noncommercial license:
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;


            var stream = new MemoryStream();
            // Tạo một package Excel
            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.TabColor = System.Drawing.Color.Black;
                workSheet.DefaultRowHeight = 12;

                workSheet.Row(1).Height = 20;
                workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Row(1).Style.Font.Bold = true;
                workSheet.Cells[1, 1].Value = "ID";
                workSheet.Cells[1, 2].Value = "Department Name";
                workSheet.Cells[1, 3].Value = "Manager ID";
                workSheet.Cells[1, 4].Value = "Manager Name";
                workSheet.Cells[1, 5].Value = "LocationID ";
                workSheet.Cells[1, 6].Value = "Location";

                //Body of table  
                //  
                int recordIndex = 2;
                foreach (var department in departments)
                {
                    workSheet.Cells[recordIndex, 1].Value = department.DepartmentId;
                    workSheet.Cells[recordIndex, 2].Value = department.DepartmentName;
                    workSheet.Cells[recordIndex, 3].Value = department.ManagerId;
                    workSheet.Cells[recordIndex, 4].Value = department.Manager?.LastName + " " + department.Manager?.FirstName;
                    workSheet.Cells[recordIndex, 5].Value = department.LocationId;
                    workSheet.Cells[recordIndex, 6].Value = department.Location?.StreetAddress + "," + department.Location?.City + "," + department.Location?.Country?.CountryName + "," + department.Location?.Country?.Region?.RegionName;

                    recordIndex++;
                }
                workSheet.Column(1).AutoFit();
                workSheet.Column(2).AutoFit();
                workSheet.Column(3).AutoFit();
                workSheet.Column(4).AutoFit();
                workSheet.Column(5).AutoFit();
                workSheet.Column(6).AutoFit();
                package.SaveAs(new FileInfo("departments.xlsx"));
            }
        }

        public void InsertListDepartment(List<Department> departments)
        {
            departmentRepo.InsertListDepartment(departments);
        }

        public void ImportExcelFile(string filePath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            List<Department> departments = new List<Department>();
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {

                var worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;
                int colCount = worksheet.Dimension.Columns;

                for (int row = 2; row <= rowCount; row++)
                {
                    departments.Add(new Department
                    {
                        DepartmentId = int.Parse(worksheet.Cells[row, 1].Value?.ToString()),
                        DepartmentName = worksheet.Cells[row, 2].Value?.ToString(),
                        ManagerId = worksheet.Cells[row, 3].Value != null ? int.Parse(worksheet.Cells[row, 3].Value.ToString()) : null,
                        LocationId = worksheet.Cells[row, 5].Value?.ToString(),
                    });
                }
            }

            if (departments != null)
            {
                InsertListDepartment(departments);
            }
        }

        public int CountTotalDepartment()
        {
           return departmentRepo.CountTotalDepartment();
        }
    }
}
