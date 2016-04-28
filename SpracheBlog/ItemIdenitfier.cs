using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpracheBlog
{

    public class ItemIdenitfier
    {
        public string Path { get; set; }
        public Guid Id { get; set; }

        public override string ToString()
        {
            if(Id != Guid.Empty)
            {
                return Id.ToString();
            }
            else
            {
                return Path;
            }
        }
    }

}
