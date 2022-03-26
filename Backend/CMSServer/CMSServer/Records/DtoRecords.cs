namespace CMSServer.Records;

public record LoginDto(string Username, string Password);
public record FolderPostDto(string Name, string Parent);
public record FolderGetDto(string Path, List<FolderItemDto> Children);
public record FolderItemDto(string Name, string Type);
