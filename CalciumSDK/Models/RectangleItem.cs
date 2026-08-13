using System.Text.Json.Serialization;

namespace CalciumSDK.Models;

public class RectangleItem
{
    [JsonPropertyName("start")]
    public int start { get; set; }
    
    [JsonPropertyName("end")]
    public int end { get; set; }
    
    [JsonPropertyName("y")]
    public int y { get; set; }
}