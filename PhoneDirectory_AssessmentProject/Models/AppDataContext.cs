using System.Data.Entity;

namespace PhoneDirectory_AssessmentProject.Models
{
    class AppDataContext: DbContext
    {
        public AppDataContext(): base("PhoneDirectoryDB")
        {

        }

        public DbSet<PhoneDirectory> PhoneDirectories { get; set; }
    }
}
