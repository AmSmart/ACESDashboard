using ACESDashboard.Data;
using ACESDashboard.Data.Repository;
using ACESDashboard.Models;
using ACESDashboard.Services;
using ACESDashboard.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ACESDashboard.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IDocumentRepository _documentRepository;
        private readonly ILogger<HomeController> _logger;
        private readonly ISectionRepository _sectionRepository;
        private readonly IStorageService _storageService;
        private readonly IUpdateRespository _updateRespository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWorkspaceRepository _workspaceRepository;

        public HomeController(ILogger<HomeController> logger,
            IWorkspaceRepository workspaceRepository,
            ApplicationDbContext dbContext,
            UserManager<ApplicationUser> userManager,
            IStorageService storageService,
            IUpdateRespository updateRespository,
            IDocumentRepository documentRepository,
            ISectionRepository sectionRepository)
        {
            _logger = logger;
            _workspaceRepository = workspaceRepository;
            _dbContext = dbContext;
            _userManager = userManager;
            _storageService = storageService;
            _updateRespository = updateRespository;
            _documentRepository = documentRepository;
            _sectionRepository = sectionRepository;
        }

        public async Task<IActionResult> AddDocument([FromForm] IFormFile file, string fileName, int workspaceId, string sectionName)
        {
            var workspace = await _workspaceRepository.GetByIdAsync(workspaceId);
            var section = workspace.Sections.FirstOrDefault(x => x.Name == sectionName);
            string fileExtension = Path.GetExtension(file.FileName);

            string storedFileName = await _storageService.SaveFile(file, fileExtension);

            var document = new Document
            {
                Name = fileName,
                Section = section,
                FileContentType = file.ContentType,
                FileExtension = fileExtension,
                FileName = storedFileName,
                TimePosted = DateTime.UtcNow
            };

            await _documentRepository.CreateAsync(document);
            return RedirectToAction("Workspace", new { id = workspaceId, activeOnly = true });
        }

        public async Task<IActionResult> AddSection(string sectionName, int workspaceId)
        {
            var workspace = await _workspaceRepository.GetByIdAsync(workspaceId);

            bool nameCollision = workspace.Sections.Select(x => x.Name).Contains(sectionName);

            if (nameCollision)
            {
                // TODO: Return View with error            
            }

            var section = new Section { Name = sectionName, Workspace = workspace };

            await _sectionRepository.CreateAsync(section);
            return Ok();
        }

        public async Task<IActionResult> AddUpdate(string updateText, DateTime expiryTime, int workspaceId)
        {
            var workspace = await _workspaceRepository.GetByIdAsync(workspaceId);
            var update = new Update
            {
                Text = updateText,
                ExpiresAt = expiryTime,
                Workspace = workspace,
                TimePosted = DateTime.UtcNow,
            };

            await _updateRespository.CreateAsync(update);
            return Ok();
        }

        public async Task<IActionResult> CreateWorkspace(string name, string tag)
        {
            var workspace = new Workspace
            {
                Name = name,
                Tag = tag,
            };

            await _workspaceRepository.CreateAsync(workspace);
            return Ok();
        }

        public async Task<IActionResult> DeleteDocument(int id)
        {
            var document = await _documentRepository.GetByIdAsync(id);
            _storageService.DeleteFile(document.FileName);
            await _documentRepository.DeleteAsync(document);
            return Ok();
        }

        public async Task<IActionResult> DeleteSection(int id)
        {
            var section = await _sectionRepository.GetByIdAsync(id);

            var sectionFileNames = section.Documents.Select(x => x.FileName).ToArray();
            _storageService.DeleteFiles(sectionFileNames);

            await _sectionRepository.DeleteAsync(section);
            return Ok();
        }

        public async Task<IActionResult> DeleteUpdate(int id)
        {
            await _updateRespository.DeleteAsync(new Update { Id = id });
            return Ok();
        }

        public IActionResult DownloadDocument(string fileName, string name, string fileExtension, string contentType)
        {
            var file = _storageService.GetFile(fileName);
            return File(file, contentType, $"{name}{fileExtension}");
        }

        public async Task<IActionResult> EditUpdate(int id, string newUpdateText, DateTime expiryTime, DateTime postedAt)
        {
            var update = new Update
            {
                Id = id,
                Text = newUpdateText,
                ExpiresAt = expiryTime,
                TimePosted = postedAt
            };

            await _updateRespository.UpdateAsync(update);
            return Ok();

        }

        public async Task<IActionResult> EditWorkspace(int id, bool activeOnly)
        {
            var workspace = await _workspaceRepository.GetByIdAsync(id, activeOnly);
            var model = new EditWorkspaceVM
            {
                ActiveUpdatesOnly = activeOnly,
                Archived = workspace.Archived,
                Guid = workspace.Guid,
                Id = workspace.Id,
                Name = workspace.Name,
                Sections = workspace.Sections,
                Tag = workspace.Tag,
                Updates = workspace.Updates.OrderByDescending(x => x.TimePosted).ToList()
            };
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Index(bool activeOnly = true)
        {
            var workspaces = await _workspaceRepository.GetAllAsync(activeOnly);

            return View(new IndexViewModel { Workspaces = workspaces, ActiveOnly = activeOnly });
        }
        
        [HttpGet("do")]
        public async Task<IActionResult> Utils()
        {
            //var user = User.FindFirst("");
            var user = await _userManager.FindByIdAsync("8f4e774d-b853-4aa5-964b-0766be235803");
            //await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim("SuperAdminClaim", ""));
            var x = User.FindFirst(Constants.SuperAdminClaim);
            return Ok("Done");
        }

        public async Task<IActionResult> Workspace(int id, bool activeOnly)
        {
            var workspace = await _workspaceRepository.GetByIdAsync(id, activeOnly);
            var model = new WorkspaceViewModel
            {
                ActiveUpdatesOnly = activeOnly,
                Archived = workspace.Archived,
                Guid = workspace.Guid,
                Id = workspace.Id,
                Name = workspace.Name,
                Sections = workspace.Sections,
                Tag = workspace.Tag,
                Updates = workspace.Updates.OrderByDescending(x => x.TimePosted).ToList()
            };
            return View(model);
        }
    }
}
