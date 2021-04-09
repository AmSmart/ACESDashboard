using ACESDashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACESDashboard.Data.Repository
{
    public class DocumentRepository : IDocumentRepository
    {
        public DocumentRepository(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public ApplicationDbContext DbContext { get; }

        public async Task<Document> CreateAsync(Document document)
        {
            DbContext.Documents.Add(document);
            await DbContext.SaveChangesAsync();
            return document;
        }

        public async Task<Document> DeleteAsync(Document document)
        {
            DbContext.Documents.Remove(document);
            await DbContext.SaveChangesAsync();
            return document;
        }

        public async Task<Document> GetByIdAsync(int id)
            => await DbContext.Documents.FindAsync(id);

        public async Task<string> GetFileNameAsync(int id)
            => (await DbContext.Documents.FindAsync(id)).FileName;

        public async Task<Document> UpdateAsync(Document document)
        {
            DbContext.Documents.Update(document);
            await DbContext.SaveChangesAsync();
            return document;
        }
    }
}
