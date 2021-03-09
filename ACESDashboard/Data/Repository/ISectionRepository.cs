using ACESDashboard.Models;
using System.Threading.Tasks;

namespace ACESDashboard.Data.Repository
{
    public interface ISectionRepository
    {
        Task<Section> CreateAsync(Section section);
        Task<Section> DeleteAsync(Section section);
        Task<Section> GetByIdAsync(int id);
        Task<Section> UpdateAsync(Section section);
    }
}