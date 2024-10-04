using MinecraftBlazorSuite.Models;
using Newtonsoft.Json;

namespace MinecraftBlazorSuite.Services;

public static class MinecraftProfileService
{
    private static readonly HttpClient HttpClient = CreateHttpClient();
    
    private static HttpClient CreateHttpClient()
    {
        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/42.0.2311.135 Safari/537.36 Edge/12.246");
        
        client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
        return client;
    }
        
    private const string MojangProfileUrl = "https://api.mojang.com/users/profiles/minecraft/";

    public static async Task<MojangProfileUser> GetMinecraftProfileAsync(string username)
    {
        HttpResponseMessage response = await HttpClient.GetAsync(MojangProfileUrl + username);
        if (!response.IsSuccessStatusCode) 
            return new MojangProfileUser();
        
        string jsonResponse = await response.Content.ReadAsStringAsync();
        MojangProfileUser? mojangUser = JsonConvert.DeserializeObject<MojangProfileUser>(jsonResponse);
            
        return mojangUser;
    }
}