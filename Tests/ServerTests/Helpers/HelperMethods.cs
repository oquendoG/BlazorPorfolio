using Microsoft.EntityFrameworkCore;
using Server.Data;

namespace Tests.ServerTests.Helpers;
internal static class HelperMethods
{
    internal static DbContextOptions<AppDbContext> GenerateOptions()
    {
        return new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase($"testDb_{Guid.NewGuid()}")
            .Options;
    }
}