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
using Microsoft.AspNetCore.Mvc;
namespace Services.Impl
{
    public class JobService : IJobService
    {
        private readonly IJobRepo jobRepo;

        public JobService()
        {
            jobRepo = new JobRepo();
        }

        public int CountTotalJob()
        {
            return jobRepo.CountTotalJob();
        }

        public void DeleteJob(string jobId)
        {
            jobRepo.DeleteJob(jobId);
        }

        public async Task ExportExcel(string? title, string? minSalary, string? maxSalary, string? sortBy, string? sortOrder)
        {
            List<Job> jobs = FilterJob(title, minSalary, maxSalary, sortBy, sortOrder);


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
                workSheet.Cells[1, 2].Value = "Job Title";
                workSheet.Cells[1, 3].Value = "Min Salary";
                workSheet.Cells[1, 4].Value = "Max Salary";

                //Body of table  
                //  
                int recordIndex = 2;
                foreach (var job in jobs)
                {
                    workSheet.Cells[recordIndex, 1].Value = job.JobId;
                    workSheet.Cells[recordIndex, 2].Value = job.JobTitle;
                    workSheet.Cells[recordIndex, 3].Value = job.MinSalary;
                    workSheet.Cells[recordIndex, 4].Value = job.MaxSalary;

                    recordIndex++;
                }
                workSheet.Column(1).AutoFit();
                workSheet.Column(2).AutoFit();
                workSheet.Column(3).AutoFit();
                workSheet.Column(4).AutoFit();
                workSheet.Column(5).AutoFit();
                package.SaveAs(new FileInfo("jobs.xlsx"));
            }

        }

        public List<Job> FilterJob(string? title, string? minSalary, string? maxSalary, string? sortBy, string? sortOrder)
        {
            List<Job> jobs = GetJobs();

            if (!string.IsNullOrEmpty(title))
            {
                jobs = jobs.Where(j => j.JobTitle.Contains(title)).ToList();
            }

            if (!string.IsNullOrEmpty(minSalary))
            {
                jobs = jobs.Where(j => j.MinSalary >= int.Parse(minSalary)).ToList();
            }

            if (!string.IsNullOrEmpty(maxSalary))
            {
                jobs = jobs.Where(j => j.MaxSalary <= int.Parse(maxSalary)).ToList();
            }


            if (!string.IsNullOrEmpty(sortBy) && !string.IsNullOrEmpty(sortOrder))
            {
                switch (sortBy)
                {
                    case "Name":
                        jobs = sortOrder == "Ascending" ? jobs.OrderBy(c => c.JobTitle).ToList() : jobs.OrderByDescending(c => c.JobTitle).ToList();
                        break;

                    case "Min Salary":
                        jobs = sortOrder == "Ascending" ? jobs.OrderBy(c => c.MinSalary).ToList() : jobs.OrderByDescending(c => c.MinSalary).ToList();
                        break;
                    case "Max Salary":
                        jobs = sortOrder == "Ascending" ? jobs.OrderBy(c => c.MaxSalary).ToList() : jobs.OrderByDescending(c => c.MaxSalary).ToList();
                        break;
                }
            }

            return jobs;
        }

        public Job? GetJobById(string id)
        {
            return jobRepo.GetJobById(id);
        }

        public List<Job> GetJobs()
        {
            return jobRepo.GetJobs();
        }

        public void ImportExcelFile(string filePath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            List<Job> jobs = new List<Job>();
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                
                var worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;
                int colCount = worksheet.Dimension.Columns;

                for (int row = 2; row <= rowCount; row++)
                {
                    jobs.Add(new Job
                    {
                        JobId = worksheet.Cells[row, 1].Value?.ToString(),
                        JobTitle = worksheet.Cells[row, 2].Value?.ToString(),
                        MinSalary = worksheet.Cells[row, 3].Value != null ? int.Parse(worksheet.Cells[row, 3].Value.ToString()) : 0,
                        MaxSalary = worksheet.Cells[row, 4].Value != null ? int.Parse(worksheet.Cells[row, 4].Value.ToString()) : 0
                    });
                }
            }

            if(jobs != null)
            {
                InsertListJob(jobs);
            }


        }

        public void InsertJob(Job job)
        {
            jobRepo.InsertJob(job);
        }

        public void InsertListJob(List<Job> jobs)
        {
            jobRepo.InsertListJob(jobs);
        }

        public void UpdateJob(Job job)
        {
            jobRepo.UpdateJob(job);
        }
    }
}
