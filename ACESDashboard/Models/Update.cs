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

        public UpdateType UpdateType { get; set; }

        public string Text { get; set; }

        public string ImageFileName { get; set; }

        public DateTime TimePosted { get; set; }

        // Reverse Navigation Property
        public virtual Workspace Workspace { get; set; }
    }

    public enum UpdateType
    {
        Text,
        Image
    }
}
