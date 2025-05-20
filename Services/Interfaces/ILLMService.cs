using RagService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RagService.Services.Interfaces
{
    /// <summary>
    /// Generates a natural-language answer to <paramref name="query"/>,
    /// grounding the response in the supplied <paramref name="contextDocs"/>.
    /// </summary>
    public interface ILLMService
    {
        /// <param name="query">The user’s original question.</param>
        /// <param name="contextDocs">
        ///     Full document objects (title, text, vector, etc.) that were retrieved
        ///     as the most relevant context for the query.
        /// </param>
        Task<string> GenerateAsync(string query, IEnumerable<DocumentModel> contextDocs);
    }
}
