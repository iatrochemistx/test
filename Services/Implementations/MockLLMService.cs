using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RagService.Services.Interfaces;
using RagService.Models;



namespace RagService.Services.Implementations
{
    /// <summary>
    /// Simple placeholder that fabricates an answer by echoing the query
    /// and listing the titles of the supplied documents.  No real LLM calls.
    /// </summary>
    public sealed class MockLLMService : ILLMService
    {
        public Task<string> GenerateAsync(string query, IEnumerable<DocumentModel> contextDocs)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"[MOCK ANSWER] You asked: \"{query}\".");
            sb.Append("I found these docs: ");
            sb.Append(string.Join(", ", contextDocs.Select(d => d.FileName)));
            sb.Append(".");
            return Task.FromResult(sb.ToString());
        }        
    }
}
