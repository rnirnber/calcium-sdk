namespace CalciumSDK.Models;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

public class WarpBlueprint
{
    [JsonPropertyName("scene_number")]
    public int scene_number { get; set; }
    
    [JsonPropertyName("x")]
    public int x { get; set; }
    
    [JsonPropertyName("y")]
    public int y { get; set; }
    
    [JsonPropertyName("after_player_x")]
    public int after_player_x { get; set; }

    [JsonPropertyName("after_player_y")]
    public int after_player_y { get; set; }
    
    [JsonPropertyName("after_screen_x")]
    public int after_screen_x { get; set; }
    
    [JsonPropertyName("after_screen_y")]
    public int after_screen_y { get; set; }
    
    [JsonPropertyName("after_player_dir")]
    public string after_player_dir { get; set; }
    
    [JsonPropertyName("allowed_conditions")]
    public Dictionary<string, List<string>> allowed_conditions { get; set; }
    
    [JsonPropertyName("on_condition_failed_msg")]
    public string on_condition_failed_msg { get; set; }
    
    [JsonPropertyName("on_finished_statements")]
    public Dictionary<string, List<string>> on_finished_statements { get; set; }
    
    [JsonPropertyName("on_failed_statements")]
    public Dictionary<string, List<string>> on_failed_statements { get; set; }
}