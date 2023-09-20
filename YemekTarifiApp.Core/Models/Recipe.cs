using System.Text.Json.Serialization;

namespace YemekTarifiApp.Core.Models;

public class Recipe:Base
{

    public string CategoryName { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string RecipeContext { get; set; } = null!;
    public User User { get; set; }
    public string UserId { get; set; } = null!;

    public List<Comment> Comments { get; set; } = new List<Comment>();
}