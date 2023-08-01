using Microsoft.EntityFrameworkCore;
using Server.Data;

namespace Tests.ServerTests.Helpers;
internal static class HelperMethods
{
    public static DbContextOptions<AppDbContext> GenerateOptions()
    {
        return new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("testDb")
            .Options;
    }
}