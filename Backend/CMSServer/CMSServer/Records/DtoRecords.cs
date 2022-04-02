namespace CMSServer.Records;

public record LoginDto(string Username, string Password);
public record FolderPostDto(string Name, string Parent);
public record FolderGetDto(string Path, List<FolderItemDto> Children);
public record FolderPutDto(string OldPath, string NewName);
public record FolderItemDto(string Name, string Type);
public record FilePostDto(string Path, IFormFile File);
public record FileGetDto(string Parent, string FileName);
public record FilePutDto(string Parent, string OldName, string NewName);
public record FileDeleteDto(string Parent, string Name);
public record FileInfoAndData(StoredFile File, byte[] Data);