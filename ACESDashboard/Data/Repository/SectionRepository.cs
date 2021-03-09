using ACESDashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACESDashboard.Data.Repository
{
    public class SectionRepository : ISectionRepository
    {
        public SectionRepository(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public ApplicationDbContext DbContext { get; }

        public async Task<Section> CreateAsync(Section section)
        {
            DbContext.Sections.Add(section);
            await DbContext.SaveChangesAsync();
            return section;
        }
        
        public async Task<Section> UpdateAsync(Section section)
        {
            DbContext.Sections.Update(section);
            await DbContext.SaveChangesAsync();
            return section;
        }
        
        public async Task<Section> DeleteAsync(Section section)
        {
            DbContext.Sections.Remove(section);
            await DbContext.SaveChangesAsync();
            return section;
        }

        public async Task<Section> GetByIdAsync(int id)
            => await DbContext.Sections.FindAsync(id);
    }
}
