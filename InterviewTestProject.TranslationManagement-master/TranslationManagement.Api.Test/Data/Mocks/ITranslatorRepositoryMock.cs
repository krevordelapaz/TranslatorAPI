using TranslationManagement.Api.Data.Models;
using TranslationManagement.Api.Data.Repository;
using TranslationManagement.Api.Test.Data.DbContextMock;

namespace TranslationManagement.Api.Test.Data.Mocks
{
    public class ITranslatorRepositoryMock
    {
        public static ITranslatorRepository GetMock()
        {
            AppDbContext dbContextMock = AppDbContextMock.GetMock<Translator, AppDbContext>
                                        (GenerateTestData(), dbContext => dbContext.Translators);
            return new TranslatorRepository(dbContextMock);
        }

        private static List<Translator> GenerateTestData()
        {
            List<Translator> translators = new();
            Random rand = new Random();
            for (int index = 1; index <= 10; index++)
            {
                translators.Add(new Translator
                {
                    Id = index,
                    CreditCardNumber = $"{index}{index}{index}{index}{index}{index}{index}",
                    Name = $"Translator-{index}",
                    Status = "Applicant"
                });
            }
            return translators;
        }
    }
}
