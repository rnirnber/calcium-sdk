using System.Text.Json.Serialization;

namespace CalciumSDK.Models;

[JsonSourceGenerationOptions(
    WriteIndented = true, 
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase
)]
[JsonSerializable(typeof(SceneBlueprint))]
[JsonSerializable(typeof(WarpBlueprint))]
[JsonSerializable(typeof(DialogBlueprint))]
[JsonSerializable(typeof(Config))]
[JsonSerializable(typeof(GlobalVar))]
[JsonSerializable(typeof(RectangleItem))]
public partial class AppJsonContext : JsonSerializerContext
{
}