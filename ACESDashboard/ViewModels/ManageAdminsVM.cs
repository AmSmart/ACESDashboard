using ACESDashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACESDashboard.ViewModels
{
    public class ManageAdminsVM
    {
        public List<ApplicationUser> Admins { get; set; }

        public List<string[]> AdminWorkspaces { get; set; }

        public List<Workspace> AllWorkspaces { get; set; }
    }
}
