using ACESDashboard.Models;
using System.Threading.Tasks;

namespace ACESDashboard.Data.Repository
{
    public interface IDocumentRepository
    {
        Task<Document> CreateAsync(Document document);
        Task<Document> DeleteAsync(Document document);
        Task<Document> GetByIdAsync(int id);
        Task<string> GetFileNameAsync(int id);
        Task<Document> UpdateAsync(Document document);
    }
}