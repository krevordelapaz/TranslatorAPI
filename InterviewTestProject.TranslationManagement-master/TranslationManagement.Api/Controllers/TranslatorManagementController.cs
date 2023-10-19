using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TranslationManagement.Api.Data.Models;
using TranslationManagement.Api.Data.Repository;
using TranslationManagement.Api.Infrastructure.Enums;
using TranslationManagement.Api.Infrastructure.Extensions;
using TranslationManagement.Api.Infrastructure.Helpers;

namespace TranslationManagement.Api.Controllers
{
    [ApiController]
    [Route("api/TranslatorsManagement/[action]")]
    public class TranslatorManagementController : ControllerBase
    {
        private readonly ILogger<TranslatorManagementController> _logger;
        private readonly ITranslatorRepository _translatorRepository;

        public TranslatorManagementController(ITranslatorRepository translatorRepository, ILogger<TranslatorManagementController> logger)
        {
            _translatorRepository = translatorRepository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetTranslators()
        {
            return Ok(_translatorRepository.GetTranslators());
        }

        [HttpGet]
        public IActionResult GetTranslatorsByName(string name)
        {
            Translator[] translators = _translatorRepository.GetTranslatorsByName(name);

            if(translators.Length <= 0 )
            {
                return BadRequest($"Translator/s with name {name} does not exist.");
            }

            return Ok(translators);
        }

        [HttpPost]
        public IActionResult AddTranslator(Translator translator)
        {
            _logger.LogInformation($"Adding translator {translator}...");
            bool isSuccessful = _translatorRepository.AddTranslator(translator);

            return Ok(isSuccessful);
        }
        
        [HttpPost]
        public IActionResult UpdateTranslatorStatus(int translatorId, string newStatus = "")
        {
            _logger.LogInformation($"User status update request: {newStatus} for user {translatorId}");

            if (!ControllerHelper.IsTranslationStatusValid(newStatus))
            {
                return BadRequest("Status Invalid");
            }

            Translator translator = _translatorRepository.GetTranslatorById(translatorId);

            if(translator == null)
                return BadRequest($"Translator with id {translatorId} not existing");

            bool isUpdateSuccessful = _translatorRepository.UpdateTranslatorStatus(translator, newStatus);

            if (isUpdateSuccessful)
            {
                return Ok(ProcessFlow.Updated.GetEnumDescription());
            }
            else
            {
                return BadRequest(ProcessFlow.UpdatingFailed.GetEnumDescription());
            }
        }
    }
}