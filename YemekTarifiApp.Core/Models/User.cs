namespace YemekTarifiApp.Core.Models;

public class User:Base
{
    public List<Recipe> Recipes { get; set; } = new List<Recipe>();
    public List<Favorite> Favorites { get; set; } = new List<Favorite>();
    public List<Comment> Comments { get; set; } = new List<Comment>();

}