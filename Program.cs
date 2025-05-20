using RagService.Services.Interfaces;
using RagService.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

// ─────────────────────────────────────────────────────────────
// 1.  Dependency-Injection registrations
//    • Mock implementations for local dev
//    • All three are singletons because they hold no per-request state
// ─────────────────────────────────────────────────────────────
builder.Services.AddSingleton<IEmbeddingService, MockEmbeddingService>();
builder.Services.AddSingleton<IVectorSearchService, VectorSearchService>();
builder.Services.AddSingleton<ILLMService, MockLLMService>();

// ─────────────────────────────────────────────────────────────
// 2.  MVC + Swagger boilerplate (unchanged)
// ─────────────────────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ─────────────────────────────────────────────────────────────
// 3.  Middleware pipeline
// ─────────────────────────────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
