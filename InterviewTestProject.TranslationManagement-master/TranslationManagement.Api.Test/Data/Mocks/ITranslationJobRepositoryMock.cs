using TranslationManagement.Api.Data.Models;
using TranslationManagement.Api.Data.Repository;
using TranslationManagement.Api.Test.Data.DbContextMock;

namespace TranslationManagement.Api.Test.Data.Mocks
{
    public class ITranslationJobRepositoryMock
    {
        public static ITranslationJobRepository GetMock()
        {
            AppDbContext dbContextMock = AppDbContextMock.GetMock<TranslationJob, AppDbContext>(GenerateTestData(), dbContext => dbContext.TranslationJobs);
            return new TranslationJobRepository(dbContextMock);
        }

        private static List<TranslationJob> GenerateTestData()
        {
            List<TranslationJob> lstUser = new();
            Random rand = new Random();
            for (int index = 1; index <= 10; index++)
            {
                lstUser.Add(new TranslationJob
                {
                    Id = index,
                    CustomerName = $"Customer-{index}",
                    OriginalContent = $"Content-{index}"
                });
            }
            return lstUser;
        }
    }
}
