using ACESDashboard.Extensions;
using ACESDashboard.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ACESDashboard.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<Document> Documents { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Update> Updates { get; set; }
        public DbSet<Workspace> Workspaces { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Workspace>()
                .Property(x => x.Guid)
                .HasValueGenerator<SequentialGuidValueGenerator>();

            builder.Entity<Section>()
                .HasMany(x => x.Documents)
                .WithOne(x => x.Section)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ApplyUtcDateTimeConverter();
        }

        public async Task SeedDataAsync()
        {
            var workspace = new Workspace
            {
                Archived = false,
                Name = "Test",
                Tag = "Just Testing",
                Sections = new List<Section>
                {
                    new Section
                    {
                        Name = "Test Section",
                        Documents = new List<Document>
                        {
                            new Document
                            {
                                Name = "Test Document",
                                FileName = "test.png",
                                TimePosted = DateTime.UtcNow
                            }
                        }
                    }
                },
                Updates = new List<Update>
                {
                    new Update
                    {
                        Text = "This a new update, take note",
                        TimePosted = DateTime.UtcNow
                    },
                    new Update
                    {
                        Text = "Note the image",
                        TimePosted = DateTime.UtcNow
                    }
                }
            };

            Workspaces.Add(workspace);
            await SaveChangesAsync();
        }
    }
}
