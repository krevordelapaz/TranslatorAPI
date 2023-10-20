using System.Linq;
using TranslationManagement.Api.Data.Models;

namespace TranslationManagement.Api.Data.Repository
{
    public class TranslationJobRepository : ITranslationJobRepository
    {
        private readonly AppDbContext _appDbContext;
        public TranslationJobRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public bool AddJob(TranslationJob job)
        {
            bool returnValue = false;
            if (_appDbContext.TranslationJobs == null)
                return returnValue;

            if (_appDbContext.TranslationJobs.Any(x => x.Id == job.Id))
            {
                return returnValue; 
            }

            _appDbContext.SaveChanges();

            returnValue = true;

            return returnValue;
        }

        public TranslationJob GetJobById(int jobId)
        {
            if (_appDbContext.TranslationJobs == null)
                return null;

            return _appDbContext.TranslationJobs.Single(job => job.Id == jobId);
        }

        public TranslationJob[] GetJobs()
        {
            return _appDbContext.TranslationJobs.ToArray();
        }

        public bool UpdateJobStatus(TranslationJob job, string newStatus)
        {
            bool returnValue = false;
            if (_appDbContext.TranslationJobs == null)
                return returnValue;

            job.Status = newStatus;

            _appDbContext.TranslationJobs.Update(job);

            _appDbContext.SaveChanges();

            returnValue = true; 

            return returnValue;
        }
    }

    public interface ITranslationJobRepository
    {
        bool AddJob(TranslationJob job);
        TranslationJob GetJobById(int jobId);
        TranslationJob[] GetJobs();
        bool UpdateJobStatus(TranslationJob job, string newStatus);
    }
}
