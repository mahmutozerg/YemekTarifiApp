using System.Text.Json.Serialization;

namespace YemekTarifiApp.Core.Models;

public class Comment:Base
{
    public string Title { get; set; }
    public string Content { get; set; }
    public int UpVote { get; set; }
    public int DownVote { get; set; }
    public Recipe Recipe { get; set; }
    public string RecipeId { get; set; }

    public User User { get; set; }
    public string UserId { get; set; }
}