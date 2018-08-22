using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Tntp.CodingExercise.Models
{
    //public class MongoDbDocumentBase
    //{
    //    public string ID { get; set; }
    //    public DateTime Birthdate { get; set; }
    //    public string DevFirstName { get; set; }
    //    public string DevLastName { get; set; }
    //}

    public class Developer //: MongoDbDocumentBase
    {
        [Key]
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Title { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? DeathDate { get; set; }

        //public List<Award> Awards { get; set; }
        //public List<Contrib> Contribs { get; set; }
    }

    public class Award //: MongoDbDocumentBase
    {
        [Key]
        public Guid ID { get; set; }

        [Required]//[ForeignKey("Developer")]
        public string Developer_ID { get; set; }

        public Developer Developer { get; set; }

        public string Name { get; set; }
        public int Year { get; set; }
        public string By { get; set; }
    }

    public class Contrib //: MongoDbDocumentBase
    {
        [Key]
        public Guid ID { get; set; }

        [Required]//[ForeignKey("Developer")]
        public string Developer_ID { get; set; }

        public Developer Developer { get; set; }

        public string Name { get; set; }
    }

    public class DeveloperDBContext : DbContext
    {
        public int DocumentsRead { get; set; }
        public int DevelopersInserted { get; set; }
        public int AwardsInserted { get; set; }
        public int ContribsInserted { get; set; }

        public DbSet<Developer> Developers { get; set; }
        public DbSet<Award> Awards { get; set; }
        public DbSet<Contrib> Contribs { get; set; }

        public DeveloperDBContext()
        {
            DocumentsRead = 0;
            DevelopersInserted = 0;
            AwardsInserted = 0;
            ContribsInserted = 0;
        }
    }
}