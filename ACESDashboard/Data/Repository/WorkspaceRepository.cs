using ACESDashboard.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACESDashboard.Data.Repository
{
    public class WorkspaceRepository : IWorkspaceRepository
    {
        public WorkspaceRepository(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public ApplicationDbContext DbContext { get; }

        public async Task<Workspace> ToggleArchivedStateAsync(Workspace workspace)
        {
            workspace.Archived = !workspace.Archived;
            DbContext.Attach(workspace);
            DbContext.Entry(workspace).Property(x => x.Archived).IsModified = true;
            await DbContext.SaveChangesAsync();
            return workspace;
        }

        public async Task<Workspace> CreateAsync(Workspace workspace)
        {
            await DbContext.Workspaces.AddAsync(workspace);
            await DbContext.SaveChangesAsync();
            return workspace;
        }

        public async Task<List<Workspace>> GetAllAsync(bool activeOnly)
        {
            if(activeOnly)
                return await DbContext.Workspaces.Where(x => x.Archived == false).ToListAsync();

            return await DbContext.Workspaces.ToListAsync();
        }

        public async Task<Workspace> GetByIdAsync(int id, bool activeUpdatesOnly = false)
        {
            var workspace = await DbContext.Workspaces.FindAsync(id);

            if (activeUpdatesOnly)
            {
                workspace.Updates = workspace.Updates.Where(x => x.ExpiresAt.Ticks > DateTime.UtcNow.Ticks)
                    .ToList();
            }

            return workspace;
        }            

        public async Task<Workspace> UpdateAsync(Workspace workspace)
        {
            DbContext.Workspaces.Update(workspace);
            await DbContext.SaveChangesAsync();
            return workspace;
        }

        public async Task<Workspace> DeleteAsync(Workspace workspace)
        {
            DbContext.Workspaces.Remove(workspace);
            await DbContext.SaveChangesAsync();
            return workspace;
        }
    }
}
