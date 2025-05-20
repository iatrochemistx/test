using System.Threading.Tasks;

namespace RagService.Services.Interfaces
{
    /// <summary>
    /// Converts arbitrary text into a float-vector embedding.
    /// </summary>
    public interface IEmbeddingService
    {
        Task<float[]> EmbedAsync(string text);
    }
}
