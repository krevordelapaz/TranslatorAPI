using TranslationManagement.Api.Data.Models;
using TranslationManagement.Api.Data.Repository;
using TranslationManagement.Api.Test.Data.Mocks;

namespace TranslationManagement.Api.Test.Data.Repository
{
    [TestFixture]
    public class TranslatorRepositoryTest
    {
        ITranslatorRepository _translatorRepository;

        [SetUp]
        public void Setup()
        {
            _translatorRepository = ITranslatorRepositoryMock.GetMock();
        }

        [TearDown]
        public void TearDown()
        {
            _translatorRepository = null;
        }

        [Test]
        public void GetTranslatorsTest()
        {
            //Act
            Translator[] translators = _translatorRepository.GetTranslators();

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(translators, Is.Not.Null);
                Assert.That(translators.Count, Is.GreaterThan(0));
            });
        }

        [Test]
        public void GeTranslatorByIdTest()
        {
            //Act
            Translator translator = _translatorRepository.GetTranslatorById(1);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(translator, Is.Not.Null);
                Assert.That(translator.Id, Is.EqualTo(1));
            });
        }

        [Test]
        public void GeTranslatorsByNameTest()
        {
            //Act
            Translator[] translators = _translatorRepository.GetTranslatorsByName("Translator-1");

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(translators, Is.Not.Null);
                Assert.That(translators.Count, Is.GreaterThan(0));
            });
        }

        [Test]
        public void AddTranslatorTest()
        {
            //Act
            bool isSuccessfulAdding = _translatorRepository.AddTranslator(new Translator()
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
            bool isSuccessfulAdding = _translatorRepository.AddTranslator(new Translator()
            {
                Id = 1
            });

            //Assert
            Assert.IsFalse(isSuccessfulAdding);
        }

        [Test]
        public void UpdateJobStatusTest()
        {
            Translator translator = _translatorRepository.GetTranslatorById(1);

            //Act
            bool isUpdatingSuccessful = _translatorRepository.UpdateTranslatorStatus(translator, "Certified");

            //Assert
            Assert.IsTrue(isUpdatingSuccessful);
        }
    }
}