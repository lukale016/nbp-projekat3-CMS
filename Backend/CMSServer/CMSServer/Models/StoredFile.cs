using MongoDB.Bson.Serialization.Attributes;
using System.Runtime.Serialization;

namespace CMSServer.Models;
[DataContract]
[BsonIgnoreExtraElements]
public class StoredFile
{
    [BsonId]
    [DataMember]
    public string FilePath { get; set; }
    [BsonIgnore]
    public string Name { get => FilePath != null ? FilePath.Split("\\").Last() : string.Empty; }
    [DataMember]
    public string Type { get; set; }
    [DataMember]
    public string ContentType { get; set; }
}
