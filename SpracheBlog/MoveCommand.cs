using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpracheBlog
{

    public class MoveCommand : Command
    {
        public string Item { get; set; }
        public string NewLocation { get; set; }
    }

}