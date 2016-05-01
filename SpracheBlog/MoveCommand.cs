using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpracheBlog
{

    public class MoveCommand : ICommand
    {
        public ItemIdenitfier Item { get; set; }
        public ItemIdenitfier NewLocation { get; set; }

        public string Execute()
        {
            var folder = Sitecore.Context.Database.GetItem(NewLocation.ToString());
            if (folder == null)
            {
                throw new ArgumentException("The item " + NewLocation.ToString() + " was not found", "cmd.NewLocation");
            }

            var item = Sitecore.Context.Database.GetItem(Item.ToString());
            if (item == null)
            {
                throw new ArgumentException("The item " + Item.ToString() + " was not found", "cmd.Item");
            }

            item.MoveTo(folder);

            return "Moved " + Item.ToString();
        }
    }

}