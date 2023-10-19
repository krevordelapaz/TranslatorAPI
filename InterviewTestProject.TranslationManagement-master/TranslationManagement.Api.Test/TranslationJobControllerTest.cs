using Microsoft.Extensions.Logging;
using Moq;
using TranslationManagement.Api.Controllers;
using TranslationManagement.Api.Data.Models;
using TranslationManagement.Api.Data.Repository;
using TranslationManagement.Api.Test.Data.Mocks;

namespace TranslationManagement.Api.Test
{
    [TestFixture]
    public class TranslationJobControllerTest
    {
        ITranslationJobRepository _translationJobRepository;
        [SetUp]
        public void Setup()
        {
            // Arrange
            _translationJobRepository = ITranslationJobRepositoryMock.GetMock();
            //appDBContextMock.Setup<DbSet<TranslationJob>>(x => x.TranslationJobs).Returns<DbSet<TranslationJob>>(MockHelper.GetFakeTranslationJobs());

            //Act
            //TranslationJobController employeesController = new TranslationJobController(appDBContextMock.Object, transactionControllerLoggerMock.Object);
            //var employees = (await employeesController.GetEmployees()).Value;
        }

        [Test]
        public void GetTranslationJobsTest()
        {
            //Act
            TranslationJob[] lstData = _translationJobRepository.GetJobs();

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(lstData, Is.Not.Null);
                Assert.That(lstData.Count, Is.GreaterThan(0));
            });
        }
    }
}