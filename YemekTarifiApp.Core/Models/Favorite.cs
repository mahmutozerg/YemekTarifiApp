using System.Text.Json.Serialization;

namespace YemekTarifiApp.Core.Models;

public class Favorite:Base
{
    public string RecipeId { get; set; }
    [JsonIgnore]public User User { get; set; }
    public string UserId { get; set; }
}