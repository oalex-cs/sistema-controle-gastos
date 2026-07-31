using GerenciadorFinanceiro.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorFinanceiro.Api.Tests;

internal sealed class TestDatabase : IAsyncDisposable
{
    private readonly string databasePath = Path.Combine(
        Path.GetTempPath(),
        $"gerenciador-financeiro-tests-{Guid.NewGuid():N}.db");

    public async Task<AppDbContext> CreateContextAsync()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite($"Data Source={databasePath}")
            .Options;

        var context = new AppDbContext(options);
        await context.Database.EnsureCreatedAsync();

        return context;
    }

    public ValueTask DisposeAsync()
    {
        if (File.Exists(databasePath))
        {
            File.Delete(databasePath);
        }

        return ValueTask.CompletedTask;
    }
}