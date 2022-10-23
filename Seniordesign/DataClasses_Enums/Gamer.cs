﻿using Seniordesign.DataClasses_Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seniordesign.DataClasses_Enums
{
    class Gamer
    {
        public string Name { get; set; }
        public string Computer_Name { get; set; }
        public string Mac_Address { get; set; }
        public string IP_Address { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Game_Executable { get; set; }
        public List<Process> Processes { get; set; } = new List<Process>();

        public bool Connected { get; set; } = false;

        public List<LogItem> ExceptionLog { get; set; } = new List<LogItem>();
    }
}
