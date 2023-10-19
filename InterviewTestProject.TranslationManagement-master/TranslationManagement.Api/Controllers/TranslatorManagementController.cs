using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TranslationManagement.Api.Data.Models;
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
        private AppDbContext _dbContext;

        public TranslatorManagementController(AppDbContext dbContext, ILogger<TranslatorManagementController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpGet]
        public Translator[] GetTranslators()
        {
            return _dbContext.Translators.ToArray();
        }

        [HttpGet]
        public Translator[] GetTranslatorsByName(string name)
        {
            Translator[] translators = _dbContext.Translators.Where(translator => translator.Name == name).ToArray();

            if(translators.Length <= 0 )
            {
                throw new ApplicationException($"Translator/s with name {name} does not exist.");
            }

            return translators;
        }

        [HttpPost]
        public bool AddTranslator(Translator translator)
        {
            _logger.LogInformation($"Adding translator {translator}...");
            _dbContext.Translators.Add(translator);
            return _dbContext.SaveChanges() > 0;
        }
        
        [HttpPost]
        public string UpdateTranslatorStatus(int translatorId, string newStatus = "")
        {
            _logger.LogInformation($"User status update request: {newStatus} for user {translatorId}");

            if (!ControllerHelper.IsTranslationStatusValid(newStatus))
            {
                throw new ArgumentException("unknown status");
            }

            var translator = _dbContext.Translators.Single(translator => translator.Id == translatorId);

            if(translator == null)
                throw new KeyNotFoundException($"Translator with id {translatorId} not existing");

            translator.Status = newStatus;
            bool success = _dbContext.SaveChanges() > 0;

            return success ? ProcessFlow.Updated.GetEnumDescription() : ProcessFlow.UpdatingFailed.GetEnumDescription();
        }
    }
}