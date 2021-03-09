using ACESDashboard.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACESDashboard.Data.Repository
{
    public class UpdateRespository : IUpdateRespository
    {
        public UpdateRespository(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public ApplicationDbContext DbContext { get; }

        public async Task<List<Update>> GetAllForWorkspaceAsync(int workspaceId)
            => await DbContext.Updates.Where(x => x.Workspace.Id == workspaceId)
            .ToListAsync();

        public async Task<Update> GetByIdAsync(int id)
            => await DbContext.Updates.FindAsync(id);

        public async Task<Update> DeleteAsync(Update update)
        {
            DbContext.Updates.Remove(update);
            await DbContext.SaveChangesAsync();
            return update;
        }

        public async Task<Update> CreateAsync(Update update)
        {
            DbContext.Updates.Add(update);
            await DbContext.SaveChangesAsync();
            return update;
        }
    }
}
