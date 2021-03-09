using ACESDashboard.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ACESDashboard.Data.Repository
{
    public interface IWorkspaceRepository
    {
        Task<Workspace> ArchiveWorkspaceAsync(Workspace workspace);
        Task<Workspace> CreateAsync(Workspace workspace);
        Task<List<Workspace>> GetAllAsync();
        Task<Workspace> GetByIdAsync(int id);
        Task<Workspace> UnarchiveWorkspaceAsync(Workspace workspace);
        Task<Workspace> UpdateWorkspaceAsync(Workspace workspace);
    }
}