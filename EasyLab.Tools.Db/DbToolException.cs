using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyLab.Tools.Db
{
    public class DbToolException : Exception
    {
        public DbToolException() { }

        public DbToolException(string message) : base(message) { }
    }
}
