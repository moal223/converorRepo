using System.Security.Claims;
using converor.api.Dtos.Folder;
using converor.Core.Models;
using converor.EF.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace converor.api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FolderController : ControllerBase
    {
        private readonly IFolderRepo _folderRepo;
        public FolderController(IFolderRepo folderRepo){
            _folderRepo = folderRepo;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFolderDto folder){
            await _folderRepo.InsertAsync(new Folder{Name = folder.Name, ParentFolderId = folder.ParentFolderId,
             ApplicationUserId = User.FindFirstValue("userid")});
            return NoContent();
        }
        [HttpGet]
        public async Task<IActionResult> GetById([FromRoute] int id){
            var folder = await _folderRepo.GetByIdAsync(id);
            return Ok(folder);
        }
    }
}