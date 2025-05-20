using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using RagService.Services.Interfaces;

namespace RagService.Services.Implementations
{
    /// <summary>
    /// Generates a deterministic 384-dim float vector for any input text.
    /// Used for local development to avoid real OpenAI calls.
    /// </summary>
    public sealed class MockEmbeddingService : IEmbeddingService
    {
        private const int Dim = 384;

        public Task<float[]> EmbedAsync(string text)
        {
            // SHA-256 hash → stable seed
            var hash = SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(text));
            int seed = BitConverter.ToInt32(hash, 0);

            var rng  = new Random(seed);
            var vec  = new float[Dim];

            for (int i = 0; i < Dim; i++)
                vec[i] = (float)(rng.NextDouble() * 2.0 - 1.0); // range [-1,1)

            return Task.FromResult(vec);
        }
    }
}
