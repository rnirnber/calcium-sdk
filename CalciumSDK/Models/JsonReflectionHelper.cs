using System.Text.Json.Serialization;

namespace CalciumSDK.Models;

[JsonSourceGenerationOptions(
    WriteIndented = true, 
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase
)]
[JsonSerializable(typeof(SceneBlueprint))]
[JsonSerializable(typeof(WarpBlueprint))]
[JsonSerializable(typeof(DialogBlueprint))]
public partial class AppJsonContext : JsonSerializerContext
{
}