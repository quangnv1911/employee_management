using BusinessObjects;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Impl
{
    public class JobRepo : IJobRepo
    {
        public void ClearTracking(Job job)
        {
            JobDAO.ClearTracking(job);
        }

        public int CountTotalJob()
        {
           return JobDAO.CountTotalJob();
        }

        public void DeleteJob(string jobId)
        {
            JobDAO.DeleteJob(jobId);  
        }

        public Job? GetJobById(string id)
        {
            return JobDAO.GetJobById(id);
        }

        public List<Job> GetJobs()
        {
            return JobDAO.GetJobs();
        }

        public void InsertJob(Job job)
        {
            JobDAO.InsertJob(job);
        }

        public void InsertListJob(List<Job> jobs)
        {
            JobDAO.InsertListJob(jobs);
        }

        public void UpdateJob(Job job)
        {
            JobDAO.UpdateJob(job);
        }
    }
}
