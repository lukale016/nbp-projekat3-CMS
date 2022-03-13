namespace CMSServer.Models;
public class Folder
{
    public string Path { get; set; }
    public string Parent { get; set; }
    public List<string> ChildFolders { get; set; }
    public StoredFile ChildFiles { get; set; }
}
