using System.Collections.Generic;
using System.Threading.Tasks;
using RegService.Models;

namespace RagService.Services.Interfaces
{
    /// <summary>
    /// Generates a natural-language answer grounded in the provided documents.
    /// </summary>
    public interface ILLMService
    {
        /// <param name="query">User’s original query.</param>
        /// <param name="contextDocs">Full retrieved documents for grounding.</param>
        Task<string> GenerateAsync(string query, IEnumerable<DocumentModel> contextDocs);
    }
}
