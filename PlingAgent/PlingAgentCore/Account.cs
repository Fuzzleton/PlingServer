using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlingAgentCore
{
    class Account
    {
        internal string name;
        internal string passhash;
        internal LinkedList<string> friends;
    }
}
