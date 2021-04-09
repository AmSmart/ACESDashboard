using ACESDashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACESDashboard.ViewModels
{
    public class EditWorkspaceVM
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public string Name { get; set; }

        public string Tag { get; set; }

        public bool Archived { get; set; }

        public List<Section> Sections { get; set; }

        public List<Update> Updates { get; set; }

        public bool ActiveUpdatesOnly { get; set; }
    }
}
