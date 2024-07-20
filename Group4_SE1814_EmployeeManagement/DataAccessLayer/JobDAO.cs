using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class JobDAO
    {
        public static List<Job> GetJobs()
        {
            EmployeeManagementContext context = new EmployeeManagementContext();
            return context.Jobs.ToList();
        }

        public static void InsertJob(Job job)
        {
            EmployeeManagementContext context = new EmployeeManagementContext();
            context.Jobs.Add(job);
            context.SaveChanges();
        }
        public static List<Job> GetJobMaxMoney()
        {
            EmployeeManagementContext context = new EmployeeManagementContext();
            return context.Jobs.OrderByDescending(e => e.MinSalary).Take(5).ToList();
        }

        public static void UpdateJob(Job job)
        {
            EmployeeManagementContext context = new EmployeeManagementContext();
            context.Jobs.Update(job);
            context.SaveChanges();
        }

        public static void DeleteJob(string jobId)
        {
            EmployeeManagementContext context = new EmployeeManagementContext();
            var job = context.Jobs.FirstOrDefault(x => x.JobId == jobId);
            context.Jobs.Remove(job);
            context.SaveChanges();
        }

        public static Job? GetJobById(string id)
        {
            EmployeeManagementContext context = new EmployeeManagementContext();
            return context.Jobs.FirstOrDefault(j => j.JobId == id);
        }

        public static void InsertListJob(List<Job> jobs)
        {
            EmployeeManagementContext context = new EmployeeManagementContext();
            context.Jobs.AddRange(jobs);
            context.SaveChanges();
        }

        public static void ClearTracking(Job job)
        {
            EmployeeManagementContext context = new EmployeeManagementContext();
            context.Entry(job).State = EntityState.Unchanged;
        }

        public static int CountTotalJob()
        {
            EmployeeManagementContext context = new EmployeeManagementContext();
            return context.Jobs.Count();
        }
    }
}
