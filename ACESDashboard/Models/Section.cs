using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACESDashboard.Models
{
    public class Section
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual List<Document> Documents { get; set; }

        // Reverse Navigation Property
        public virtual Workspace Workspace { get; set; }
    }
}
