﻿using ACESDashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACESDashboard.ViewModels
{
    public class IndexViewModel
    {
        public List<Workspace> Workspaces { get; set; }

        public bool ActiveOnly { get; set; }
    }
}
