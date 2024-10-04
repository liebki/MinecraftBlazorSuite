using System.Text.Json.Serialization;

namespace MinecraftBlazorSuite.Models;

public class MojangProfileUser
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    
    [JsonPropertyName("username")]
    public string Username { get; set; }
}