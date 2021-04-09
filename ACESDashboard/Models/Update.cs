using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACESDashboard.Models
{
    public class Update
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime TimePosted { get; set; }

        public DateTime ExpiresAt { get; set; }

        // Reverse Navigation Property
        public virtual Workspace Workspace { get; set; }
    }
}
