using ACESDashboard.Data.Repository;
using ACESDashboard.Models;
using ACESDashboard.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ACESDashboard.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWorkspaceRepository _workspaceRepository;

        public HomeController(ILogger<HomeController> logger, IWorkspaceRepository workspaceRepository)
        {
            _logger = logger;
            _workspaceRepository = workspaceRepository;
        }

        public async Task<IActionResult> Index()
        {
            var workspaces = await _workspaceRepository.GetAllAsync();
            return View(new IndexViewModel { Workspaces = workspaces });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Workspace(int id)
        {
            var model = await _workspaceRepository.GetByIdAsync(id);
            return View(model);
        }
    }
}
