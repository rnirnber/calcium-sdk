using System.Text.Json.Serialization;

namespace CalciumSDK.Models;

public class GlobalVar
{
    [JsonPropertyName("key")]
    public string key { get; set; }
    [JsonPropertyName("value")]
    public string value { get; set; }
    [JsonPropertyName("data_type")]
    public string data_type { get; set; }
}
public class Config
{
    [JsonPropertyName("compilation_targets")]
    public List<string> compilation_targets { get; set; }
    
    [JsonPropertyName("new_game_player_x")]
    public int new_game_player_x { get; set; }
    
    [JsonPropertyName("new_game_player_y")]
    public int new_game_player_y { get; set; }
    
    [JsonPropertyName("new_game_screen_x")]
    public int new_game_screen_x { get; set; }
    
    [JsonPropertyName("new_game_screen_y")]
    public int new_game_screen_y { get; set; }
    
    [JsonPropertyName("new_game_player_dir")]
    public int new_game_player_dir { get; set; }
    
    [JsonPropertyName("global_vars")]
    public List<GlobalVar> global_vars { get; set; }
    
}