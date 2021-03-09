using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACESDashboard.Models
{
    public class Workspace
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Tag { get; set; }

        public bool Archived { get; set; }

        public virtual List<Section> Sections { get; set; }

        public virtual List<Update> Updates { get; set; }
    }
}
