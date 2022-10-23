using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seniordesign.DataClasses_Enums
{
    class LogItem
    {
        public string LogMessage { get; set; }

        public System.DateTime Time { get; set; }

        public bool GoodLog { get; set; } = false;

    }
}
