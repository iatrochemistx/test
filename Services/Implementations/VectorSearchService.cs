using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using RagService.Services.Interfaces;
using RagService.Models;

namespace RagService.Services.Implementations
{
    /// <summary>
    /// Loads every text file in the ./data folder, embeds once at startup,
    /// and serves cosine-similarity search entirely in memory.
    /// </summary>
    public sealed class VectorSearchService : IVectorSearchService
    {
        private readonly IEmbeddingService _embedder;
        private readonly List<(DocumentModel Doc, float[] Vec, float Norm)> _index = new();

        /// <param name="embedder">Injected embedding service (mock or OpenAI).</param>
        public VectorSearchService(IEmbeddingService embedder)
        {
            _embedder = embedder;
            LoadDocumentsAsync().GetAwaiter().GetResult();   // block once at startup
        }

        /* ---------- public API ---------- */

        public async Task<List<DocumentModel>> GetTopDocumentsAsync(
            string query, int topK = 3)
        {
            var qVec = await _embedder.EmbedAsync(query);
            return GetTopDocumentsInternal(qVec, topK);
        }

        public Task<List<DocumentModel>> GetTopDocumentsAsync(
            float[] queryVector, int topK = 3)
            => Task.FromResult(GetTopDocumentsInternal(queryVector, topK));

        /* ---------- internal helpers ---------- */

        private List<DocumentModel> GetTopDocumentsInternal(float[] qVec, int k)
        {
            var qNorm = VectorNorm(qVec);
            // cosine similarity = (v·w) / (||v|| * ||w||)
            var ranked = _index
                .Select(tuple => new
                {
                    Doc = tuple.Doc,
                    Score = Dot(qVec, tuple.Vec) / (qNorm * tuple.Norm)
                })
                .OrderByDescending(x => x.Score)
                .Take(k)
                .Select(x => x.Doc)
                .ToList();

            return ranked;
        }

        private async Task LoadDocumentsAsync()
        {
            var dataDir = Path.Combine(AppContext.BaseDirectory, "data");
            if (!Directory.Exists(dataDir))
                return; // nothing to index

            var files = Directory.EnumerateFiles(dataDir, "*.*", SearchOption.TopDirectoryOnly)
                                 .Where(f => f.EndsWith(".txt", StringComparison.OrdinalIgnoreCase) ||
                                             f.EndsWith(".md",  StringComparison.OrdinalIgnoreCase));

            int id = 0;
            foreach (var filePath in files)
            {
                var text = await File.ReadAllTextAsync(filePath);
                var vec  = await _embedder.EmbedAsync(text);
                var doc  = new DocumentModel
                {
                    Id        = id++,
                    FileName  = Path.GetFileName(filePath),
                    Text      = text,
                    Vector    = vec
                };
                _index.Add((doc, vec, VectorNorm(vec)));
            }
        }

        /* ---------- math helpers ---------- */

        private static float Dot(float[] a, float[] b)
        {
            double sum = 0;
            for (int i = 0; i < a.Length; i++)
                sum += a[i] * b[i];
            return (float)sum;
        }

        private static float VectorNorm(float[] v)
        {
            double sumSq = 0;
            for (int i = 0; i < v.Length; i++)
                sumSq += v[i] * v[i];
            return (float)Math.Sqrt(sumSq);
        }

        Task<List<DocumentModel>> IVectorSearchService.GetTopDocumentsAsync(string query, int topK)
        {
            throw new NotImplementedException();
        }

        Task<List<DocumentModel>> IVectorSearchService.GetTopDocumentsAsync(float[] queryVector, int topK)
        {
            throw new NotImplementedException();
        }
    }
}
