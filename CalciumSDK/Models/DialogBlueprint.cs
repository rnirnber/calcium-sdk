using System.Text.Json.Serialization;

namespace CalciumSDK.Models;

public class DialogBlueprint
{
    [JsonPropertyName("x")]
    public int x { get; set; }

    [JsonPropertyName("y")]
    public int y { get; set; }

    [JsonPropertyName("required_dir")]
    public string required_dir { get; set; }

    [JsonPropertyName("txt")]
    public string txt { get; set; }

    [JsonPropertyName("on_finished_callback_statements")]
    public Dictionary<string, List<string>> on_finished_callback_statements { get; set; }

    [JsonPropertyName("eligibility_failure_msg")]
    public string eligibility_failure_msg { get; set; }

    [JsonPropertyName("eligibility_statements")]
    public Dictionary<string, List<string>> eligibility_statements { get; set; }
    
    [JsonPropertyName("on_eligibility_failed_statements")]
    public Dictionary<string, List<string>> on_eligibility_failed_statements { get; set; }
}