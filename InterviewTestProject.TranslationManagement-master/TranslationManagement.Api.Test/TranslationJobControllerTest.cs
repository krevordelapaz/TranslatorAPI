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
            _translationJobRepository = ITranslationJobRepositoryMock.GetMock();
        }

        [TearDown]
        public void TearDown()
        {
            _translationJobRepository = null;
        }

        [Test]
        public void GetTranslationJobsTest()
        {
            //Act
            TranslationJob[] translationJobs = _translationJobRepository.GetJobs();

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(translationJobs, Is.Not.Null);
                Assert.That(translationJobs.Count, Is.GreaterThan(0));
            });
        }

        [Test]
        public void GetJobByIdTest()
        {
            //Act
            TranslationJob translationJob = _translationJobRepository.GetJobById(1);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(translationJob, Is.Not.Null);
                Assert.AreEqual(translationJob.Id, 1);    
            });
        }

        [Test]
        public void AddJobTest()
        {
            //Act
            bool isSuccessfulAdding = _translationJobRepository.AddJob(new TranslationJob()
            {
                Id = 12
            });

            //Assert
            Assert.IsTrue(isSuccessfulAdding);
        }

        [Test]
        public void AddJobExistingIdNegativeTest()
        {
            //Act
            bool isSuccessfulAdding = _translationJobRepository.AddJob(new TranslationJob()
            {
                Id = 1
            });

            //Assert
            Assert.IsFalse(isSuccessfulAdding);
        }

        [Test]
        public void UpdateJobStatusTest()
        {
            TranslationJob translationJob = _translationJobRepository.GetJobById(1);

            //Act
            bool isUpdatingSuccessful = _translationJobRepository.UpdateJobStatus(translationJob, "InProgress");

            //Assert
            Assert.IsTrue(isUpdatingSuccessful);
        }
    }
}