﻿using Client.Static;
using Shared.Models.Blog;
using System.Net.Http.Json;

namespace Client.Shared.Services;

internal sealed class InMemoryDataBaseCache
{
    private readonly HttpClient httpClient;

    public InMemoryDataBaseCache(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    #region Categories
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

    internal async Task GetCategoriesFromDatabaseAndCache(bool withPosts)
    {
        if (gettinCategoriesFromDbAnCaching)
        {
            return;
        }

        List<Category> categoriesDb = null;
        gettinCategoriesFromDbAnCaching = true;

        if (categories is not null)
        {
            categories = null;
        }

        if (withPosts)
        {
            categoriesDb = await httpClient.GetFromJsonAsync<List<Category>>(ApiEndpoints.s_categoriesWithPosts);
        }
        else
        {
            categoriesDb = await httpClient.GetFromJsonAsync<List<Category>>(ApiEndpoints.s_categories);
        }

        categories = [.. categoriesDb.OrderByDescending(cat => cat.Id)];

        if (withPosts)
        {
            GetPostsFromCachedCategories();
        }

        gettinCategoriesFromDbAnCaching = false;
    }

    private void GetPostsFromCachedCategories()
    {
        List<Post> postsFromCategories = [];

        foreach ((Category category, Post post) in categories
                 .Where(category => category.Posts.Count != 0)
                        .SelectMany(category => category.Posts.Select(post => (category, post))))
        {
            post.Category = new()
            {
                Id = category.Id,
                ThumbnailImage = category.ThumbnailImage,
                Name = category.Name,
                Description = category.Description,
                Posts = null
            };
            postsFromCategories.Add(post);
        }

        posts = [.. postsFromCategories.OrderByDescending(pst => pst.Id)];
    }

    internal async Task<Category> GetCategoryById(Guid Id, bool withPosts)
    {
        if (categories is null)
        {
            await GetCategoriesFromDatabaseAndCache(withPosts);
        }

        Category category = categories.Find(cat => cat.Id == Id);
        if (category.Posts is null && withPosts)
        {
            category = await httpClient.GetFromJsonAsync<Category>($"{ApiEndpoints.s_categoriesWithPosts}/{category.Id}");
        }

        return category;
    }

    internal async Task<Category> GetCategoryByName(string name, bool withPosts, bool nameToLowerFromUrl)
    {
        if (categories is null)
        {
            await GetCategoriesFromDatabaseAndCache(withPosts);
        }

        Category category = null;
        if (nameToLowerFromUrl)
        {
            category = Categories.Find(cat => cat.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
        }

        if (category.Posts is null && !withPosts)
        {
            category = category = await httpClient.GetFromJsonAsync<Category>($"{ApiEndpoints.s_categoriesWithPosts}/{category.Id}");
        }

        return category;
    }

    internal event Action OnCategoriesDataChanged;
    private void NotifyCategoriesDataChanged()
    {
        OnCategoriesDataChanged?.Invoke();
    }
    #endregion

    #region Posts
    private List<Post> posts = null;
    public List<Post> Posts
    {
        get => posts;
        set
        {
            posts = value;
            NotifyPostsDataChanged();
        }
    }

    internal async Task<Post> GetPostById(Guid Id)
    {
        if (posts is null)
        {
            await GetPostsFromDatabaseAndCache();
        }

        return posts.Find(pst => pst.Id == Id);
    }

    internal async Task<PostDTO> GetPostDtoById(Guid Id)
    {
        PostDTO postDTO = await httpClient
            .GetFromJsonAsync<PostDTO>($"{ApiEndpoints.s_posts}/{Id}");

        return postDTO;
    }

    private bool gettingPostsFromDatabaseAndCaching = false;
    public async Task GetPostsFromDatabaseAndCache()
    {
        if (gettingPostsFromDatabaseAndCaching)
        {
            return;
        }

        gettingPostsFromDatabaseAndCaching = true;
        if (posts is not null)
        {
            posts = null;
        }

        List<Post> postFromDatabase = await httpClient
            .GetFromJsonAsync<List<Post>>(ApiEndpoints.s_posts);

        posts = [.. postFromDatabase.OrderByDescending(posts => posts.Id)];

        gettingPostsFromDatabaseAndCaching = false;
    }

    internal event Action OnPostsDataChanged;
    private void NotifyPostsDataChanged()
    {
        OnPostsDataChanged?.Invoke();
    }

    #endregion
}
