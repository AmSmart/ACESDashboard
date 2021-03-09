using ACESDashboard.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ACESDashboard.Data.Repository
{
    public interface IUpdateRespository
    {
        Task<Update> CreateAsync(Update update);
        Task<Update> DeleteAsync(Update update);
        Task<List<Update>> GetAllForWorkspaceAsync(int workspaceId);
        Task<Update> GetByIdAsync(int id);
    }
}