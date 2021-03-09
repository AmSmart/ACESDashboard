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

        public async Task<Document> Create(Document document)
        {
            DbContext.Documents.Add(document);
            await DbContext.SaveChangesAsync();
            return document;
        }

        public async Task<Document> Delete(Document document)
        {
            DbContext.Documents.Remove(document);
            await DbContext.SaveChangesAsync();
            return document;
        }

        public async Task<Document> GetById(int id)
            => await DbContext.Documents.FindAsync(id);

        public async Task<string> GetFilePath(int id)
            => (await DbContext.Documents.FindAsync(id)).DocumentFilePath;

        public async Task<Document> Update(Document document)
        {
            DbContext.Documents.Update(document);
            await DbContext.SaveChangesAsync();
            return document;
        }
    }
}
