using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpracheBlog
{

    public class DeleteCommand : Command
    {
        public ItemIdenitfier Item { get; set; }
    }

}