namespace CMSServer.Data.FileRepo;
public interface IFileRepository
{
    Task<StoredFile> GetFile(FileGetDto dto);
    Task<byte[]> ReadFileData(StoredFile file);
    Task<FileInfoAndData> ReadFile(FileGetDto dto);
    Task StoreFile(FilePostDto dto);
    Task<StoredFile> UpdateFile(FilePutDto dto);
    Task DeleteFile(FileDeleteDto dto);
}
