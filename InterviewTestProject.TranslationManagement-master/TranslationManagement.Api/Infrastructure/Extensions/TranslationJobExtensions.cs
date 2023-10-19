using TranslationManagement.Api.Infrastructure.Helpers;
using TranslationManagement.Api.Infrastructure.Models;

namespace TranslationManagement.Api.Infrastructure.Extensions
{
    public static class TranslationJobExtensions
    {
        public static void InitializeJob(this TranslationJob job)
        {
            job.Status = "New";
            ControllerHelper.SetJobPrice(job);
        }
    }
}
