using BusinessObjects;
using System;
using Repositories;
using Repositories.Impl;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer;

namespace Services.Impl
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepo employeeRepo;
        private readonly IJobRepo jobRepo;
        private readonly IDepartmentRepo departmentRepo;
        public EmployeeService()
        {
            employeeRepo = new EmployeedRepo();
            jobRepo = new JobRepo();
            departmentRepo = new DepartmentRepo();
        }

        public int CountTotalEmployee()
        {
            return employeeRepo.CountTotalEmployee();
        }

        public void DeleteEmployee(int employeeID)
        {
            employeeRepo.DeleteEmployee(employeeID);
        }

        public async Task ExportExcel(string? name, string? email, string? phone, string? salary, string? department, string? job, string? sortBy, string? sortOrder)
        {
            List<Employee> employees = FilterEmployee(name, email, phone, salary, department, job, sortBy, sortOrder);

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
                workSheet.Cells[1, 1].Value = "Employee ID";
                workSheet.Cells[1, 2].Value = "First Name";
                workSheet.Cells[1, 3].Value = "Last Name";
                workSheet.Cells[1, 4].Value = "Email";
                workSheet.Cells[1, 5].Value = "Phone";
                workSheet.Cells[1, 6].Value = "Hire Date";
                workSheet.Cells[1, 7].Value = "JobID";
                workSheet.Cells[1, 8].Value = "Job";

                workSheet.Cells[1, 9].Value = "Salary";
                workSheet.Cells[1, 10].Value = "Commission PCT";
                workSheet.Cells[1, 11].Value = "ManagerID";
                workSheet.Cells[1, 12].Value = "Manager";
                workSheet.Cells[1, 13].Value = "DepartmentId";
                workSheet.Cells[1, 14].Value = "Department";

                //Body of table  
                //  
                int recordIndex = 2;
                foreach (var employee in employees)
                {
                    workSheet.Cells[recordIndex, 1].Value = employee.EmployeeId;
                    workSheet.Cells[recordIndex, 2].Value = employee.FirstName;
                    workSheet.Cells[recordIndex, 3].Value = employee.LastName;
                    workSheet.Cells[recordIndex, 4].Value = employee.Email;
                    workSheet.Cells[recordIndex, 5].Value = employee.Phone;
                    workSheet.Cells[recordIndex, 6].Value = employee.HireDate?.ToString("yyyy-MM-dd");

                    workSheet.Cells[recordIndex, 7].Value = employee.Job.JobId;
                    workSheet.Cells[recordIndex, 8].Value = employee.Job.JobTitle;
                    workSheet.Cells[recordIndex, 9].Value = employee.Salary;
                    workSheet.Cells[recordIndex, 10].Value = employee.CommissionPct;
                    workSheet.Cells[recordIndex, 11].Value = employee.Manager?.EmployeeId;
                    workSheet.Cells[recordIndex, 12].Value = employee.Manager?.LastName + " " + employee.Manager?.FirstName;
                    workSheet.Cells[recordIndex, 13].Value = employee.Department?.DepartmentId;
                    workSheet.Cells[recordIndex, 14].Value = employee.Department?.DepartmentName;


                    recordIndex++;
                }
                workSheet.Column(1).AutoFit();
                workSheet.Column(2).AutoFit();
                workSheet.Column(3).AutoFit();
                workSheet.Column(4).AutoFit();
                workSheet.Column(5).AutoFit();
                workSheet.Column(6).AutoFit();
                workSheet.Column(7).AutoFit();
                workSheet.Column(8).AutoFit();
                workSheet.Column(9).AutoFit();
                workSheet.Column(10).AutoFit();
                workSheet.Column(11).AutoFit();
                workSheet.Column(12).AutoFit();
                workSheet.Column(13).AutoFit();
                workSheet.Column(14).AutoFit();
                package.SaveAs(new FileInfo("employees.xlsx"));
            }
        }

        public List<Employee> FilterEmployee(string? name, string? email, string? phone, string? salary, string? department, string? job, string? sortBy, string? sortOrder)
        {
            List<Employee> employees = GetEmployees();
            if (!string.IsNullOrEmpty(name))
            {
                employees = employees
                    .Where(d => d.FirstName.Contains(name) || d.LastName.Contains(name)).ToList();

            }
            if (!string.IsNullOrEmpty(email))
            {
                employees = employees
                    .Where(d => d.Email.Contains(email)).ToList();
            }

            if (!string.IsNullOrEmpty(phone))
            {
                employees = employees
                    .Where(d => d.Phone.Contains(phone)).ToList();
            }

            if (!string.IsNullOrEmpty(salary))
            {
                employees = employees
                    .Where(d => d.Salary == double.Parse(salary)).ToList();
            }

            if (!string.IsNullOrEmpty(department))
            {
                employees = employees
                    .Where(d => d.Department.DepartmentName.Contains(department)).ToList();
            }

            if (!string.IsNullOrEmpty(job))
            {
                employees = employees
                    .Where(d => d.Job.JobTitle.Contains(job)).ToList();
            }

            if (!string.IsNullOrEmpty(sortBy) && !string.IsNullOrEmpty(sortOrder))
            {
                switch (sortBy)
                {
                    case "First Name":
                        employees = sortOrder == "Ascending" ? employees.OrderBy(c => c.FirstName).ToList() : employees.OrderByDescending(c => c.FirstName).ToList();
                        break;

                    case "Last Name":
                        employees = sortOrder == "Ascending" ? employees.OrderBy(c => c.LastName).ToList() : employees.OrderByDescending(c => c.LastName).ToList();
                        break;
                    case "Email":
                        employees = sortOrder == "Ascending" ? employees.OrderBy(c => c.Email).ToList() : employees.OrderByDescending(c => c.Email).ToList();
                        break;
                    case "Phone":
                        employees = sortOrder == "Ascending" ? employees.OrderBy(c => c.Phone).ToList() : employees.OrderByDescending(c => c.Phone).ToList();
                        break;
                    case "Hire Date":
                        employees = sortOrder == "Ascending" ? employees.OrderBy(c => c.HireDate).ToList() : employees.OrderByDescending(c => c.HireDate).ToList();
                        break;
                    case "Salary":
                        employees = sortOrder == "Ascending" ? employees.OrderBy(c => c.Salary).ToList() : employees.OrderByDescending(c => c.Salary).ToList();
                        break;
                    case "Commission PCT":
                        employees = sortOrder == "Ascending" ? employees.OrderBy(c => c.CommissionPct).ToList() : employees.OrderByDescending(c => c.CommissionPct).ToList();
                        break;
                    case "Department":
                        employees = sortOrder == "Ascending" ? employees.OrderBy(c => c.Department.DepartmentName).ToList() : employees.OrderByDescending(c => c.Department.DepartmentName).ToList();
                        break;
                    case "Job":
                        employees = sortOrder == "Ascending" ? employees.OrderBy(c => c.Job.JobTitle).ToList() : employees.OrderByDescending(c => c.Job.JobTitle).ToList();
                        break;
                    case "Manager":
                        employees = sortOrder == "Ascending" ? employees.OrderBy(c => c.Manager.FirstName).ToList() : employees.OrderByDescending(c => c.Manager.FirstName).ToList();
                        break;
                    default:
                        break;

                }
            }

            return employees;
        }

        public List<Employee> FilterListByRole(string role, List<Employee> employees)
        {
            if (role == "admin")
            {
                return employees;
            }
            employees = employees.Where(e => e.Manager.EmployeeId == int.Parse(role)).ToList();
            return employees;
        }

        public Employee? GetEmployeeById(int id)
        {
            return employeeRepo.GetEmployeeById(id);
        }

        public string? GetEmployeeName(int id)
        {
            return employeeRepo.GetEmployeeName(id);
        }

        public List<Employee> GetEmployees()
        {
            return employeeRepo.GetAllEmployees();
        }


        public List<Employee> GetEmployeesManage(int id)
        {
            return employeeRepo.GetEmployeesManage(id);
        }

        public void ImportExcelFile(string filePath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            List<Employee> employees = new List<Employee>();
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;
                int colCount = worksheet.Dimension.Columns;
                EmployeeManagementContext context = new EmployeeManagementContext();
               
                for (int row = 2; row <= rowCount; row++)
                {
                    
                    Employee employee =  new Employee
                    {
                        EmployeeId = int.Parse(worksheet.Cells[row, 1].Value?.ToString() ?? "0"),
                        FirstName = worksheet.Cells[row, 2].Value?.ToString(),
                        LastName = worksheet.Cells[row, 3].Value?.ToString(),
                        Email = worksheet.Cells[row, 4].Value?.ToString(),
                        Phone = worksheet.Cells[row, 5].Value?.ToString(),
                        HireDate = DateOnly.Parse(worksheet.Cells[row, 6].Value?.ToString() ?? ""),
                        JobId = worksheet.Cells[row, 7].Value?.ToString() ?? null,
                        //Job = new Job
                        //{
                        //    JobId = int.Parse(worksheet.Cells[row, 7].Value?.ToString() ?? "0"),
                        //    JobTitle = worksheet.Cells[row, 8].Value?.ToString()
                        //},
                        //Job = job,
                        Salary = double.Parse(worksheet.Cells[row, 9].Value?.ToString() ?? "0"),
                        CommissionPct = worksheet.Cells[row, 10].Value != null ? double.Parse(worksheet.Cells[row, 10].Value.ToString()) : null,
                        //Manager = manager,
                        //Department = department
                        ManagerId = int.Parse(worksheet.Cells[row, 11].Value?.ToString() ?? null),
                        //Manager = new Employee
                        //{
                        //    EmployeeId = int.Parse(worksheet.Cells[row, 11].Value?.ToString() ?? "0"),
                        //    LastName = worksheet.Cells[row, 12].Value?.ToString().Split(' ')[0],
                        //    FirstName = worksheet.Cells[row, 12].Value?.ToString().Split(' ')[1]
                        //},
                        //Department = new Department
                        //{
                        //    DepartmentId = int.Parse(worksheet.Cells[row, 13].Value?.ToString() ?? "0"),
                        //    DepartmentName = worksheet.Cells[row, 14].Value?.ToString()
                        //}
                        DepartmentId = int.Parse(worksheet.Cells[row, 13].Value?.ToString() ?? null)
                    };
                    
                    context.Entry(employee).State = EntityState.Detached;
                    //if (job != null)
                    //{
                    //    jobRepo.ClearTracking(job);
                    //}
                    ////if (department != null)
                    ////{
                    ////    departmentRepo.ClearTracking(department);
                    ////}
                    //if (manager != null)
                    //{
                    //    employeeRepo.ClearTracking(manager);
                    //}
                    employees.Add(employee);
                }
            }

            if (employees != null)
            {
                InsertListEmployee(employees);
            }
        }

        public void InsertEmployee(Employee employee)
        {
            employeeRepo.InsertEmployee(employee);
        }

        public void InsertListEmployee(List<Employee> employees)
        {
            employeeRepo.InsertListEmployee(employees);
        }

        public void UpdateEmployee(Employee employee)
        {
            employeeRepo.UpdateEmployee(employee);
        }
    }
}
