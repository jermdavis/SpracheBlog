using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpracheBlog
{

    public class DeleteCommand : ICommand
    {
        public ItemIdenitfier Item { get; set; }

        public string Execute()
        {
            var itm = Sitecore.Context.Database.GetItem(Item.ToString());
            if (itm == null)
            {
                throw new ArgumentException("The item " + Item.ToString() + " was not found", "cmd.Item");
            }

            itm.Delete();

            return "Deleted " + Item.ToString();
        }
    }

}