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
    
    [JsonPropertyName("background_forgiveness_threshold")]
    public int background_forgiveness_threshold { get; set; }
    
    [JsonPropertyName("alpha_step_one")]
    public List<int> alpha_step_one { get; set; }

    [JsonPropertyName("alpha_step_two")]
    public List<int> alpha_step_two { get; set; }
    
    [JsonPropertyName("alpha_step_three")]
    public List<int> alpha_step_three { get; set; }
    
    [JsonPropertyName("alpha_step_four")]
    public List<int> alpha_step_four { get; set; }
    
    [JsonPropertyName("alpha_step_five")]
    public List<int> alpha_step_five { get; set; }
    
    [JsonPropertyName("dark_mode_color")]
    public List<int> dark_mode_color { get; set; }

    [JsonPropertyName("true_alpha")]
    public List<int> true_alpha { get; set; }
    
}