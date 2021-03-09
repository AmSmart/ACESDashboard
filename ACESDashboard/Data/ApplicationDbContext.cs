using ACESDashboard.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ACESDashboard.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<Document> Documents { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Update> Updates { get; set; }
        public DbSet<Workspace> Workspaces { get; set; }

        public async Task SeedData()
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
                                DocumentFileName = "test.png",
                                TimePosted = DateTime.UtcNow
                            }
                        }
                    }
                },
                Updates = new List<Update>
                {
                    new Update
                    {
                        UpdateType = UpdateType.Text,
                        Text = "This a new update, take note",
                        TimePosted = DateTime.UtcNow
                    },
                    new Update
                    {
                        UpdateType = UpdateType.Image,
                        ImageFileName = "test.png",
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
