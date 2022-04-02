using Microsoft.AspNetCore.Mvc;

namespace CMSServer.Controllers;
[Route("api/[controller]")]
public class FolderController : Controller
{
    private readonly UnitOfWork _unitOfWork;

    public FolderController(UnitOfWork unit)
    {
        _unitOfWork = unit;
    }

    [HttpPost("FolderContent")]
    public async Task<ActionResult<FolderGetDto>> RetreiveFolderContent([FromBody]string path)
    {
        try
        {
            return await _unitOfWork.FolderRepository.GetFolderContent(path);
        }
        catch (ResponseException ex)
        {
            return StatusCode(ex.Status, ex.Message);
        }
    }

    [HttpPost("CreateFolder")]
    public async Task<ActionResult> CreateFolder([FromBody]FolderPostDto folder)
    {
        try
        {
            await _unitOfWork.FolderRepository.CreateFolder(folder);
            return Ok("Folder created");
        }
        catch (ResponseException ex)
        {
            return StatusCode(ex.Status, ex.Message);
        }
    }

    [HttpPut("UpdateFolder")]
    public async Task<ActionResult<FolderGetDto>> UpdateFolder([FromBody]FolderPutDto dto)
    {
        try
        {
            return await _unitOfWork.FolderRepository.UpdateFolder(dto);
        }
        catch (ResponseException ex)
        {
            return StatusCode(ex.Status, ex.Message);
        }
    }

    [HttpDelete("DeleteFolder")]
    public async Task<ActionResult> DeleteFolder([FromBody]string path)
    {
        try
        {
            await _unitOfWork.FolderRepository.DeleteFolder(path);
            return Ok("Folder deleted successfully");
        }
        catch (ResponseException ex)
        {
            return StatusCode(ex.Status, ex.Message);
        }
    }
}
