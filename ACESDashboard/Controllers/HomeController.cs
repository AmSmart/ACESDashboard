using ACESDashboard.Data;
using ACESDashboard.Data.Repository;
using ACESDashboard.Filters;
using ACESDashboard.Models;
using ACESDashboard.Services;
using ACESDashboard.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ACESDashboard.Controllers
{
    [TypeFilter(typeof(ExceptionFilter))]
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

        public async Task<IActionResult> AddAdminToWorkspace(string userId, string workspaceName)
        {            
            if(User.FindFirst(Constants.SuperAdminClaim) is not null)
            {
                if (string.IsNullOrWhiteSpace(workspaceName))
                {
                    return BadRequest("Name cannot be blank");
                }

                var claims = await _dbContext.UserClaims.Where(x => x.ClaimType == Constants.AdminClaim && x.UserId == userId)
                .ToListAsync();
                string workspaceGuid = _dbContext.Workspaces.FirstOrDefault(x => x.Name == workspaceName).Guid.ToString();

                if (claims.FirstOrDefault(x => x.ClaimValue == workspaceGuid) is null)
                {
                    _dbContext.UserClaims.Add(new IdentityUserClaim<string>
                    {
                        UserId = userId,
                        ClaimType = Constants.AdminClaim,
                        ClaimValue = workspaceGuid
                    });
                    await _dbContext.SaveChangesAsync();
                    return Ok();
                }
                string errorMessage = "User is already an admin in this workspace";
                return BadRequest(errorMessage);
            }
            return Unauthorized();
        }

        public async Task<IActionResult> AddDocument([FromForm] IFormFile file, string fileName,
            int workspaceId, string sectionName)
        {   
            if(file == null || string.IsNullOrWhiteSpace(fileName) || string.IsNullOrWhiteSpace(sectionName))
            {
                return RedirectToAction("Workspace", new { id = workspaceId, returnMessage = "F Fields cannot be blank" });
            }

            var workspace = await _workspaceRepository.GetByIdAsync(workspaceId);

            if(User.FindFirst(Constants.SuperAdminClaim) is not null 
                || User.HasClaim(x => x.Type == Constants.AdminClaim && x.Value == workspace.Guid.ToString()))
            {
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
            return Unauthorized();
        }

        public async Task<IActionResult> AddSection(string sectionName, int workspaceId)
        {            

            var workspace = await _workspaceRepository.GetByIdAsync(workspaceId);

            if (User.FindFirst(Constants.SuperAdminClaim) is not null
                || User.HasClaim(x => x.Type == Constants.AdminClaim && x.Value == workspace.Guid.ToString()))
            {
                if (string.IsNullOrWhiteSpace(sectionName))
                {
                    return BadRequest("Name cannot be blank");
                }

                bool nameCollision = workspace.Sections.Select(x => x.Name).Contains(sectionName);

                if (nameCollision)
                {
                    string errorMessage = $"Section named '{sectionName}' already exists";
                    return BadRequest(errorMessage);
                }

                var section = new Section { Name = sectionName, Workspace = workspace };

                await _sectionRepository.CreateAsync(section);
                return Ok();
            }
            return Unauthorized();
        }

        public async Task<IActionResult> AddUpdate(string updateText, DateTime expiryTime, int workspaceId)
        {            
            var workspace = await _workspaceRepository.GetByIdAsync(workspaceId);

            if (User.FindFirst(Constants.SuperAdminClaim) is not null
                || User.HasClaim(x => x.Type == Constants.AdminClaim && x.Value == workspace.Guid.ToString()))
            {
                if (string.IsNullOrWhiteSpace(updateText))
                {
                    return BadRequest("Text cannot be blank");
                }

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
            return Unauthorized();
        }

        public async Task<IActionResult> CreateAdmin(string email, string password, string confirmPassword)
        {
            if(User.FindFirst(Constants.SuperAdminClaim) is not null)
            {
                if (string.IsNullOrWhiteSpace(email) 
                    || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
                {
                    return BadRequest("Fields cannot be blank");
                }

                if (password == confirmPassword)
                {
                    var user = new ApplicationUser { Email = email, UserName = email };
                    var result = await _userManager.CreateAsync(user, password);
                    if (result.Succeeded)
                    {
                        await _userManager.AddClaimAsync(user, new Claim(Constants.AdminClaim, ""));
                        return Ok();
                    }
                    string errors = string.Join(",", result.Errors.Select(x => x.Description));
                    return BadRequest($"Error(s): {errors}");
                }
                return Ok();
            }
            return Unauthorized();
        }

        public async Task<IActionResult> CreateWorkspace(string name, string tag)
        {
            if (User.FindFirst(Constants.SuperAdminClaim) is not null)
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return BadRequest("Name cannot be blank");
                }

                bool nameCollision = _dbContext.Workspaces.Select(x => x.Name).Contains(name);

                if (!nameCollision)
                {
                    var workspace = new Workspace
                    {
                        Name = name,
                        Tag = tag,
                    };

                    await _workspaceRepository.CreateAsync(workspace);
                    return Ok();
                }
                string errorMessage = $"Workspace named {name} already exists";
                return BadRequest(errorMessage);
            }
            return Unauthorized();
        }

        public async Task<IActionResult> DeleteAdmin(string id)
        {
            if (User.FindFirst(Constants.SuperAdminClaim) is not null)
            {
                var user = await _userManager.FindByIdAsync(id);
                await _userManager.DeleteAsync(user);
                return Ok();
            }
            return Unauthorized();
        }

        public async Task<IActionResult> DeleteDocument(int id)
        {
            var document = await _documentRepository.GetByIdAsync(id);
            if (User.FindFirst(Constants.SuperAdminClaim) is not null
                || User.HasClaim(x => x.Type == Constants.AdminClaim && x.Value == document.Section.Workspace.Guid.ToString()))
            {
                _storageService.DeleteFile(document.FileName);
                await _documentRepository.DeleteAsync(document);
                return Ok();
            }
            return Unauthorized();
        }

        public async Task<IActionResult> DeleteSection(int id)
        {
            var section = await _sectionRepository.GetByIdAsync(id);
            if (User.FindFirst(Constants.SuperAdminClaim) is not null
                || User.HasClaim(x => x.Type == Constants.AdminClaim && x.Value == section.Workspace.Guid.ToString()))
            {

                var sectionFileNames = section.Documents.Select(x => x.FileName).ToArray();
                _storageService.DeleteFiles(sectionFileNames);

                await _sectionRepository.DeleteAsync(section);
                return Ok();
            }
            return Unauthorized();
        }

        public async Task<IActionResult> DeleteUpdate(int id)
        {
            var update = await _updateRespository.GetByIdAsync(id);
            if (User.FindFirst(Constants.SuperAdminClaim) is not null
                || User.HasClaim(x => x.Type == Constants.AdminClaim && x.Value == update.Workspace.Guid.ToString()))
            {
                await _updateRespository.DeleteAsync(new Update { Id = id });
                return Ok();
            }
            return Unauthorized();
        }

        public async Task<IActionResult> DeleteWorkspace(int id)
        {
            if (User.FindFirst(Constants.SuperAdminClaim) is not null)
            {
                var workspace = await _workspaceRepository.GetByIdAsync(id);

                var sectionsToDelete = await _dbContext.Sections.Where(x => x.Workspace.Id == id).ToListAsync();
                var updatesToDelete = await _dbContext.Updates.Where(x => x.Workspace.Id == id).ToListAsync();
                var userClaimsToDelete = await _dbContext.UserClaims.Where(x => x.ClaimValue == workspace.Guid.ToString()).ToListAsync();

                _dbContext.Sections.RemoveRange(sectionsToDelete);
                _dbContext.Updates.RemoveRange(updatesToDelete);
                _dbContext.UserClaims.RemoveRange(userClaimsToDelete);
                await _dbContext.SaveChangesAsync();

                await _workspaceRepository.DeleteAsync(workspace);
                return RedirectToAction("Index");
            }
            return Unauthorized();
        }

        public IActionResult DownloadDocument(string fileName, string name, string fileExtension, string contentType)
        {
            var file = _storageService.GetFile(fileName);
            return File(file, contentType, $"{name}{fileExtension}");
        }

        public async Task<IActionResult> EditSection(int id, string name)
        {
            bool nameCollision = _dbContext.Sections.Select(x => x.Name).Contains(name);

            if (!nameCollision)
            {
                var section = await _sectionRepository.GetByIdAsync(id);
                section.Name = name;
                await _sectionRepository.UpdateAsync(section);
                return Ok();
            }
            string errorMessage = $"Section named {name} already exists in this workspace";
            return BadRequest(errorMessage);
        }

        public async Task<IActionResult> EditUpdate(int id, string newUpdateText, DateTime expiryTime)
        {
            var update = await _updateRespository.GetByIdAsync(id);
            if (User.FindFirst(Constants.SuperAdminClaim) is not null
                || User.HasClaim(x => x.Type == Constants.AdminClaim && x.Value == update.Workspace.Guid.ToString()))
            {
                if (string.IsNullOrWhiteSpace(newUpdateText))
                {
                    return BadRequest("Text cannot be blank");
                }

                var newUpdate = new Update
                {
                    Id = id,
                    Text = newUpdateText,
                    ExpiresAt = expiryTime,
                    TimePosted = update.TimePosted
                };

                await _updateRespository.UpdateAsync(newUpdate);
                return Ok();
            }
            return Unauthorized();
        }

        public async Task<IActionResult> EditWorkspace(int id, bool activeOnly)
        {
            var workspace = await _workspaceRepository.GetByIdAsync(id, activeOnly);
            if (User.FindFirst(Constants.SuperAdminClaim) is not null
                || User.HasClaim(x => x.Type == Constants.AdminClaim && x.Value == workspace.Guid.ToString()))
            {
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
            return NotFound();
        }

        public async Task<IActionResult> EditWorkspaceNameAndTag(int id, string name, string tag)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Fields cannot be blank");
            }

            bool nameCollision = _dbContext.Workspaces.Select(x => x.Name).Contains(name);
            if (!nameCollision)
            {
                var workspace = await _workspaceRepository.GetByIdAsync(id);
                workspace.Name = name;
                workspace.Tag = tag;
                await _workspaceRepository.UpdateAsync(workspace);
                return Ok();
            }
            string errorMessage = $"Workspace named {name} already exists";
            return BadRequest(errorMessage);
        }

        public async Task<IActionResult> Index(bool activeOnly = true)
        {
            var workspaces = await _workspaceRepository.GetAllAsync(activeOnly);

            return View(new IndexViewModel { Workspaces = workspaces, ActiveOnly = activeOnly });
        }

        public async Task<IActionResult> ManageAdmins()
        {
            if (User.FindFirst(Constants.SuperAdminClaim) is not null)
            {
                var userClaims = await _dbContext.UserClaims.ToListAsync();
                var workspaces = await _dbContext.Workspaces.ToListAsync();

                var adminIds = userClaims
                    .Where(x => x.ClaimType == Constants.AdminClaim)
                    .Select(x => x.UserId);
                var admins = await _dbContext.Users.Where(x => adminIds.Contains(x.Id)).ToListAsync();
                var adminWorkspaces = new List<string[]> { };

                foreach (var admin in admins)
                {
                    var guids = userClaims.Where(x => x.UserId == admin.Id && x.ClaimType == Constants.AdminClaim)
                        .Select(x => x.ClaimValue)
                        .ToArray();
                    var names = new List<string>();
                    for (int i = 0; i < guids.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(guids[i]))
                        {
                            string name = workspaces.FirstOrDefault(x => x.Guid.ToString() == guids[i]).Name;
                            names.Add(name);
                        }
                    }
                    adminWorkspaces.Add(names.ToArray());
                }

                return View(new ManageAdminsVM { Admins = admins, AdminWorkspaces = adminWorkspaces, AllWorkspaces = workspaces });
            }
            return NotFound();
        }

        public async Task<IActionResult> RemoveAdminFromWorkspace(string userId, string workspaceName)
        {
            if (User.FindFirst(Constants.SuperAdminClaim) is not null)
            {
                var claims = await _dbContext.UserClaims.Where(x => x.ClaimType == Constants.AdminClaim && x.UserId == userId)
                    .ToListAsync();
                string workspaceGuid = _dbContext.Workspaces.FirstOrDefault(x => x.Name == workspaceName).Guid.ToString();
                var claim = claims.FirstOrDefault(x => x.ClaimValue == workspaceGuid);

                if (claim is not null)
                {
                    _dbContext.UserClaims.Remove(claim);
                    await _dbContext.SaveChangesAsync();
                    return Ok();
                }
                string errorMessage = "User is not an admin in this workspace";
                return BadRequest(errorMessage);
            }
            return Unauthorized();
        }

        public async Task<IActionResult> ToggleWorkspaceArchivedState(int id)
        {
            if (User.FindFirst(Constants.SuperAdminClaim) is not null)
            {
                var workspace = await _workspaceRepository.GetByIdAsync(id);
                await _workspaceRepository.ToggleArchivedStateAsync(workspace);
                return RedirectToAction("EditWorkspace", new { Id = id, ActiveOnly = true });
            }
            return Unauthorized();
        }

        [HttpGet("do")]
        public IActionResult Utils()
        {
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
