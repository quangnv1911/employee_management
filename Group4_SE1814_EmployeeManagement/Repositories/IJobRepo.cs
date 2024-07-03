using BusinessObjects;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IJobRepo
    {
        List<Job> GetJobs();
        void InsertJob(Job job);

        void UpdateJob(Job job);

        void DeleteJob(string jobId);

        Job? GetJobById(string id);
        void InsertListJob(List<Job> jobs);
        void ClearTracking(Job job);

        int CountTotalJob();
    }
}
