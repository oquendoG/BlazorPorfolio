namespace Client.Static;

internal static class ApiEndpoints
{
#if DEBUG
    internal const string ServerBaseUrl = "https://localhost:7155";
#else
    internal const string ServerBaseUrl = "https://wilsonserver.azurewebsites.net";
#endif

    internal readonly static string s_categories =  Path.Combine(ServerBaseUrl, "api", "categories");
    internal readonly static string s_imageUpload = Path.Combine(ServerBaseUrl, "api", "imageupload");
}
