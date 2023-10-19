using Microsoft.Extensions.Logging;
using Moq;
using TranslationManagement.Api.Controllers;

namespace TranslationManagement.Api.Test
{
    [TestFixture]
    public class TranslationJobControllerTest
    {
        Mock<ILogger<TranslationJobController>> transactionControllerLoggerMock = new Mock<ILogger<TranslationJobController>>();

        [SetUp]
        public void Setup()
        {
            // Arrange
            var appDBContextMock = new Mock<AppDbContext>();
            //appDBContextMock.Setup<DbSet<TranslationJob>>(x => x.TranslationJobs).Returns<DbSet<TranslationJob>>(MockHelper.GetFakeTranslationJobs());

            //Act
            //TranslationJobController employeesController = new TranslationJobController(appDBContextMock.Object, transactionControllerLoggerMock.Object);
            //var employees = (await employeesController.GetEmployees()).Value;
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}