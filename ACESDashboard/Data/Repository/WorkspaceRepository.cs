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

        public async Task<List<Workspace>> GetAllAsync() 
            => await DbContext.Workspaces.ToListAsync();

        public async Task<Workspace> CreateAsync(Workspace workspace)
        {
            await DbContext.Workspaces.AddAsync(workspace);
            await DbContext.SaveChangesAsync();
            return workspace;
        }

        public async Task<Workspace> GetByIdAsync(int id)
            => await DbContext.Workspaces.FindAsync(id);

        public async Task<Workspace> UpdateWorkspaceAsync(Workspace workspace)
        {
            DbContext.Workspaces.Update(workspace);
            await DbContext.SaveChangesAsync();
            return workspace;
        }

        public async Task<Workspace> ArchiveWorkspaceAsync(Workspace workspace)
        {
            workspace.Archived = true;
            DbContext.Workspaces.Update(workspace);
            await DbContext.SaveChangesAsync();
            return workspace;
        }

        public async Task<Workspace> UnarchiveWorkspaceAsync(Workspace workspace)
        {
            workspace.Archived = false;
            DbContext.Workspaces.Update(workspace);
            await DbContext.SaveChangesAsync();
            return workspace;
        }

    }
}
