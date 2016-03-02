using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MarkdownManagerNew.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Groups = new List<Group>();

            Documents = new List<Document>();
        }

        //[Required]
        [Display(Name = "Epost")]
        public string MailAdress { get; set; }

        [Required]
        [Display(Name = "Förnamn")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Efternamn")]
        public string LastName { get; set; }

        [Display(Name = "Dokument")]
        public virtual ICollection<Document> Documents { get; set; }

        [Display(Name = "Grupper")]
        public virtual ICollection<Group> Groups { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<Group> Groups { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Document> Documents { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Document>()
        //    .HasMany(t => t.Groups)
        //     .WithMany(t => t.Documents)
        //    .Map(m =>
        //     {
        //        m.ToTable("Group documents");
        //     m.MapLeftKey("DocumentId");
        //     m.MapRightKey("GroupId");
        //        });

        //    modelBuilder.Entity<Document>()
        //        .HasMany(t => t.Tags)
        //        .WithMany(t => t.Documents)
        //        .Map(m =>
        //        {
        //            m.ToTable("Document tags");
        //            m.MapLeftKey("TagId");
        //            m.MapRightKey("DocumentId");
        //        });

        //    modelBuilder.Entity<

        //     }



        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        //public System.Data.Entity.DbSet<MarkdownManagerNew.Models.ApplicationUser> ApplicationUsers { get; set; }
    }
}