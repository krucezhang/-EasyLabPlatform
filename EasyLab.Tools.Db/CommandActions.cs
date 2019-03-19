using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyLab.Tools.Db
{
    /// <summary>
    /// Represents the command type
    /// </summary>
    public enum CommandActions
    {
        Install = 0,
        Uninstall = 1,
        Config = 2,
        Elevate = 3
    }
}
