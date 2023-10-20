using Microsoft.EntityFrameworkCore;
using TranslationManagement.Api.Data.Models;

namespace TranslationManagement.Api
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public AppDbContext()
        {

        }

        //Virtual to support unit testing
        public virtual DbSet<TranslationJob> TranslationJobs { get; set; }
        public virtual DbSet<Translator> Translators { get; set; }
    }
}