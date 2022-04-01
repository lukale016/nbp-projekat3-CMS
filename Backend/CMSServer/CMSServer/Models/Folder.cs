using MongoDB.Bson.Serialization.Attributes;
using System.Runtime.Serialization;

namespace CMSServer.Models;
[DataContract]
[BsonIgnoreExtraElements]
public class Folder
{
    [BsonId]
    [DataMember]
    public string FolderPath { get; set; }
    [BsonIgnore]
    public string Name { get => FolderPath != null ? FolderPath.Split("\\").Last() : string.Empty; }
    [DataMember]
    public string Parent { get; set; }
    [DataMember]
    public List<string> ChildFolders { get; set; }
    [DataMember]
    public List<StoredFile> ChildFiles { get; set; }
}
