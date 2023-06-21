using Client.Static;
using Shared.Models.Blog;
using System.Net.Http.Json;

namespace Client.Services;

internal sealed class InMemoryDataBaseCache
{
    private readonly HttpClient httpClient;

    public InMemoryDataBaseCache(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    private List<Category> categories = null;
    internal List<Category> Categories
    {
        get
        {
            return categories;
        }
        set
        {
            categories = value;
            NotifyCategoriesDataChanged();
        }
    }

    private bool gettinCategoriesFromDbAnCaching = false;

    internal async Task GetCategoriesFromDatabaseAndCache()
    {
        if (!gettinCategoriesFromDbAnCaching)
        {
            gettinCategoriesFromDbAnCaching = true;
            categories = await httpClient.GetFromJsonAsync<List<Category>>(ApiEndpoints.s_categories);
            gettinCategoriesFromDbAnCaching = false;
        }
    }

    internal event Action OnCategoriesDataChanged;
    private void NotifyCategoriesDataChanged() => OnCategoriesDataChanged?.Invoke();
}
