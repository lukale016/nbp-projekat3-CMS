using System.Text.Json.Serialization;

namespace CMSServer.Models;
public class User
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    [JsonIgnore]
    public string RootDir { get => $"{Username}.root"; }
}
