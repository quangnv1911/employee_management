using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IJobService
    {
        List<Job> GetJobs();
        void InsertJob(Job job);

        void UpdateJob(Job job);

        void DeleteJob(string jobId);

        Job? GetJobById(string id);

        List<Job> FilterJob(string? title, string? minSalary, string? maxSalary, string? sortBy, string? sortOrder);
        Task ExportExcel(string? title, string? minSalary, string? maxSalary, string? sortBy, string? sortOrder);

        void ImportExcelFile(string filePath);
        void InsertListJob(List<Job> jobs);
        int CountTotalJob();

        List<Job> GetJobMaxMony();
    }
}
