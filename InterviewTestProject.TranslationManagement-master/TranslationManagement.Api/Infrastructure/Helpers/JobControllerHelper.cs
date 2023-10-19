using System;
using TranslationManagement.Api.Infrastructure.Constants;
using TranslationManagement.Api.Infrastructure.Enums;
using TranslationManagement.Api.Infrastructure.Models;

namespace TranslationManagement.Api.Infrastructure.Helpers
{
    public static class ControllerHelper
    {
        /// <summary>
        /// Checks if the job status is valid
        /// </summary>
        /// <param name="newJobStatus">Job status</param>
        /// <returns></returns>
        public static bool IsJobStatusValid(string newJobStatus)
        {
            JobStatus jobStatus;
            bool isJobStatusValid = Enum.TryParse(newJobStatus, out jobStatus);
            return isJobStatusValid;
        }

        /// <summary>
        /// Checks if the translation status is valid
        /// </summary>
        /// <param name="newTranslationStatus">Translation status</param>
        /// <returns></returns>
        public static bool IsTranslationStatusValid(string newTranslationStatus)
        {
            TranslationStatus translationStatus;
            bool isTranslationStatusValaid = Enum.TryParse(newTranslationStatus, out translationStatus);
            return isTranslationStatusValaid;
        }

        /// <summary>
        /// Sets the price for the translation job
        /// </summary>
        /// <param name="job">Translation Job</param>
        public static void SetJobPrice(TranslationJob job)
        {
            job.Price = job.OriginalContent.Length * ConstantValues.PricePerCharacter;
        }
    }
}
