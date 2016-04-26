using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpracheBlog
{

    public class CreateCommand : Command
    {
        public string Template { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public IEnumerable<Field> Fields { get; set; }
    }

}