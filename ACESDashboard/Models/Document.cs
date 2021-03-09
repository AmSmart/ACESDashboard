﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACESDashboard.Models
{
    public class Document
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string DocumentFileName { get; set; }

        public DateTime TimePosted { get; set; }

        // Reverse Navigation Property
        public virtual Section Section { get; set; }
    }
}
