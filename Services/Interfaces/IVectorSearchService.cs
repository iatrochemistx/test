using RagService.Services.Interfaces;
using RagService.Models;



namespace RagService.Services.Interfaces
{
    /// <summary>
    /// Finds the top-K most similar documents to either a raw query or a pre-computed vector.
    /// </summary>
    public interface IVectorSearchService
    {
        /// <summary>
        /// Convenience overload: embed <paramref name="query"/> internally, then return top-K docs.
        /// </summary>
        Task<List<DocumentModel>> GetTopDocumentsAsync(string query, int topK = 3);

        /// <summary>
        /// Pure similarity search using a pre-computed embedding vector.
        /// </summary>
        Task<List<DocumentModel>> GetTopDocumentsAsync(float[] queryVector, int topK = 3);
    }
}
