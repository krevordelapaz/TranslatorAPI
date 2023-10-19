using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TranslationManagement.Api.Data.Models;

namespace TranslationManagement.Api.Data.Repository
{
    public class TranslatorRepository : ITranslatorRepository
    {
        private readonly AppDbContext _appDbContext;
        public TranslatorRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public bool AddTranslator(Translator translator)
        {
            bool returnValue = false;
            if (_appDbContext.Translators == null)
                return returnValue;

            if (_appDbContext.Translators.Any(x => x.Id == translator.Id))
            {
                return returnValue;
            }

            _appDbContext.Translators.Add(translator);

            return _appDbContext.SaveChanges() > 0;
        }

        public Translator GetTranslatorById(int translatorId)
        {
            if (_appDbContext.Translators == null)
                return null;

            return _appDbContext.Translators.Single(translator => translator.Id == translatorId);
        }

        public Translator[] GetTranslators()
        {
            return _appDbContext.Translators.ToArray();
        }

        public Translator[] GetTranslatorsByName(string name)
        {
            if (_appDbContext.Translators == null)
                return (new List<Translator>()).ToArray();

            return _appDbContext.Translators.Where(translator => translator.Name == name).ToArray();
        }

        public bool UpdateTranslatorStatus(Translator translator, string newStatus)
        {
            bool returnValue = false;
            if (_appDbContext.Translators == null)
                return returnValue;

            translator.Status = newStatus;

            _appDbContext.Translators.Update(translator);

            return _appDbContext.SaveChanges() > 0;
        }
    }

    public interface ITranslatorRepository
    {
        bool AddTranslator(Translator translator);
        Translator GetTranslatorById(int translatorId);
        Translator[] GetTranslators();
        Translator[] GetTranslatorsByName(string name);
        bool UpdateTranslatorStatus(Translator translator, string newStatus);
    }
}
