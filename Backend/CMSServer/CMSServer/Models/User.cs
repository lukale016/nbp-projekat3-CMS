using MongoDB.Bson.Serialization.Attributes;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace CMSServer.Models;

[DataContract]
[BsonIgnoreExtraElements]
public class User
{
    [BsonId]
    [DataMember]
    public string Username { get; set; }
    [DataMember]
    public string Password { get; set; }
    [DataMember]
    public string Name { get; set; }
    [DataMember]
    public string Surname { get; set; }
    [BsonIgnore]
    public string RootDir { get => $"{Username}.root"; }
}
