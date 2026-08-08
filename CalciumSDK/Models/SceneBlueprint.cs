using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace CalciumSDK.Models;

public class SceneBlueprint
{
    [JsonPropertyName("name")]
    public string name { get; set; }
    
    [JsonPropertyName("on_exit_statements")]
    public Dictionary<string, List<string>> on_exit_statements { get; set; }

    [JsonPropertyName("inn_item_keys")]
    public List<string> inn_item_keys { get; set; }
    
    [JsonPropertyName("monster_numbers")]
    public List<int> monster_numbers { get; set; }
    
    [JsonPropertyName("monsters_exp_min")]
    public List<int> monsters_exp_min { get; set; }
    
    [JsonPropertyName("monsters_exp_max")]
    public List<int> monsters_exp_max { get; set; }
    
    [JsonPropertyName("monsters_gold_min")]
    public List<int> monsters_gold_min { get; set; }

    [JsonPropertyName("monsters_gold_max")]
    public List<int> monsters_gold_max { get; set; }

    
    [JsonPropertyName("monsters_defense_min")]
    public List<int> monsters_defense_min { get; set; }
    
    [JsonPropertyName("monsters_defense_max")]
    public List<int> monsters_defense_max { get; set; }

    [JsonPropertyName("monsters_attack_min")]
    public List<int> monsters_attack_min { get; set; }
    
    [JsonPropertyName("monsters_attack_max")]
    public List<int> monsters_attack_max { get; set; }
    
 
    [JsonPropertyName("monsters_hp_min")]
    public List<int> monsters_hp_min { get; set; }
    
    [JsonPropertyName("monsters_hp_max")]
    public List<int> monsters_hp_max { get; set; }
    
    [JsonPropertyName("monsters_appearance_frequency")]
    public int monsters_appearance_frequency { get; set; }

    [JsonPropertyName("on_entrance_statements")]
    public Dictionary<string, List<string>> on_entrance_statements { get; set; }
    
    [JsonPropertyName("initial_dialog")]
    public string initial_dialog { get; set; }
    
    [JsonPropertyName("dialogs")]
    public List<DialogBlueprint> dialogs { get; set; }

    [JsonPropertyName("warps")]
    public List<WarpBlueprint> warps { get; set; }
    
    [JsonPropertyName("alternative_player_activation_points")]
    public List<Dictionary<string, int>> alternative_player_activation_points { get; set; }

    [JsonPropertyName("alternative_player_deactivation_points")]
    public List<Dictionary<string, int>> alternative_player_deactivation_points { get; set; }
    
    [JsonPropertyName("follower_enabled_statements")]
    public Dictionary<string, List<string>> follower_enabled_statements { get; set; }
    
    [JsonPropertyName("assets_used")]
    public List<int> assets_used { get; set; }
    
    [JsonPropertyName("forbidden_assets")]
    public List<int> forbidden_assets { get; set; }
}