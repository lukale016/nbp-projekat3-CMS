using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace CMSServer.Controllers;
[Route("api/[controller]")]
public class FileController : Controller
{
    private readonly UnitOfWork _unitOfWork;

    public FileController(UnitOfWork unit)
    {
        _unitOfWork = unit;
    }

    [HttpPost("ReadFile")]
    public async Task<ActionResult> ReadFile([FromBody]FileGetDto dto)
    {
        try
        {
            FileInfoAndData result = await _unitOfWork.FileRepository.ReadFile(dto);
            return File(result.Data, result.File.ContentType);
        }
        catch(ResponseException ex)
        {
            return StatusCode(ex.Status, ex.Message);
        }
    }

    [HttpPost("StoreFile")]
    public async Task<ActionResult> StoreFile([FromForm(Name = "file")] IFormFile file)
    {
        try
        {
            StringValues parent;
            Request.Headers.TryGetValue(CustomHeaderConsts.ParentPathHeader, out parent);
            if (!parent.Any())
                return BadRequest("Missing parent path from headers");

            FilePostDto dto = new FilePostDto(parent.SingleOrDefault(), file);
            await _unitOfWork.FileRepository.StoreFile(dto);
            return Ok("File successfully stored");
        }
        catch (ResponseException ex)
        {
            return StatusCode(ex.Status, ex.Message);
        }
        catch(Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut("UpdateFile")]
    public async Task<ActionResult<StoredFile>> UpdateFile([FromBody]FilePutDto dto)
    {
        try
        {
            return await _unitOfWork.FileRepository.UpdateFile(dto);
        }
        catch (ResponseException ex)
        {
            return StatusCode(ex.Status, ex.Message);
        }
    }

    [HttpDelete("DeleteFile")]
    public async Task<ActionResult> DeleteFile([FromBody]FileDeleteDto dto)
    {
        try
        {
            await _unitOfWork.FileRepository.DeleteFile(dto);
            return Ok("File deleted successfully");
        }
        catch (ResponseException ex)
        {
            return StatusCode(ex.Status, ex.Message);
        }
    }
}
