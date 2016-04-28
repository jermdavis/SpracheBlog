using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpracheBlog
{

    public class CreateCommand : CommandParser
    {
        public ItemIdenitfier Template { get; set; }
        public string Name { get; set; }
        public ItemIdenitfier Location { get; set; }
        public IEnumerable<Field> Fields { get; set; }
    }

}