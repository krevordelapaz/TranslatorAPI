using TranslationManagement.Api.Data.Models;

namespace TranslationManagement.Api.Test.Mocks
{
    public static class MockHelper
    {
        public static List<TranslationJob> GetFakeTranslationJobs()
        {
            return new List<TranslationJob>()
            {
                new TranslationJob
                {
                    Id = 1,
                    CustomerName = "Customer1",
                    OriginalContent = "Test1",
                },
                new TranslationJob
                {
                    Id = 2,
                    CustomerName = "Customer2",
                    OriginalContent = "Test2",
                }
            };
        }
    }
}
