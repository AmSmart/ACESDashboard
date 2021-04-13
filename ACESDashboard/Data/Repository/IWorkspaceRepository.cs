using ACESDashboard.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ACESDashboard.Data.Repository
{
    public interface IWorkspaceRepository
    {
        Task<Workspace> ToggleArchivedStateAsync(Workspace workspace);
        Task<Workspace> CreateAsync(Workspace workspace);
        Task<Workspace> DeleteAsync(Workspace workspace);
        Task<List<Workspace>> GetAllAsync(bool activeOnly);
        Task<Workspace> GetByIdAsync(int id, bool activeUpdatesOnly = false);
        Task<Workspace> UpdateAsync(Workspace workspace);
    }
}