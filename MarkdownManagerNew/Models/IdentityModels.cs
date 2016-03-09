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

            Files = new List<File>();
        }

        //[Display(Name = "Epost")]
        //public string MailAdress { get; set; }

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

        [Display(Name = "Filer")]
        public virtual ICollection<File> Files { get; set; }

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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Document>()
                .HasMany(t => t.Users)
                .WithMany(u => u.Documents);

            modelBuilder.Entity<Group>()
                .HasMany(t => t.Users)
                .WithMany(u => u.Groups);

            modelBuilder.Entity<ApplicationUser>()
            .HasMany(t => t.Roles);

            var user = modelBuilder.Entity<ApplicationUser>()
                .ToTable("AspNetUsers");
            user.HasMany(u => u.Roles).WithRequired().HasForeignKey(ur => ur.UserId);
            user.HasMany(u => u.Claims).WithRequired().HasForeignKey(uc => uc.UserId);
            user.HasMany(u => u.Logins).WithRequired().HasForeignKey(ul => ul.UserId);
            user.Property(u => u.UserName).IsRequired();

            modelBuilder.Entity<IdentityUserRole>()
                .HasKey(r => new { r.UserId, r.RoleId })
                .ToTable("AspNetUserRoles");

            modelBuilder.Entity<IdentityUserLogin>()
                .HasKey(l => new { l.UserId, l.LoginProvider, l.ProviderKey })
                .ToTable("AspNetUserLogins");

            modelBuilder.Entity<IdentityUserClaim>()
                .ToTable("AspNetUserClaims");

            var role = modelBuilder.Entity<IdentityRole>()
                .ToTable("AspNetRoles");
            role.Property(r => r.Name).IsRequired();
            role.HasMany(r => r.Users).WithRequired().HasForeignKey(ur => ur.RoleId);

            //modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            //modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            //modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });
        }


        //    modelBuilder.Entity<Document>()
        //    .HasMany(t => t.Groups)
        //    .WithMany(g => g.Documents);

        //    modelBuilder.Entity<Document>()
        //    .HasMany(t => t.Tags)
        //    .WithMany(t => t.Documents);

        

        //    modelBuilder.Entity<Group>()
        //        .HasMany(g => g.Users)
        //        .WithMany(u => u.Groups);

        //    modelBuilder.Entity<File>()
        //        .HasRequired<ApplicationUser>(f => f.Creator)
        //        .WithMany(u => u.Files)
        //        .HasForeignKey(f => f.CreatorId);

        //    modelBuilder.Entity<File>()
        //        .HasRequired<Document>(g => g.Document)
        //        .WithMany(d => d.Files)
        //        .HasForeignKey(f => f.DocumentId);

        

        //}

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }



        //public System.Data.Entity.DbSet<MarkdownManagerNew.Models.ApplicationUser> ApplicationUsers { get; set; }
    }
}