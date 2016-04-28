using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpracheBlog
{

    public class MoveCommand : CommandParser
    {
        public ItemIdenitfier Item { get; set; }
        public ItemIdenitfier NewLocation { get; set; }
    }

}