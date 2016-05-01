using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpracheBlog
{

    public class CreateCommand : ICommand
    {
        public ItemIdenitfier Template { get; set; }
        public string Name { get; set; }
        public ItemIdenitfier Location { get; set; }
        public IEnumerable<Field> Fields { get; set; }

        public string Execute()
        {
            TemplateID tid;
            if (Template.Id != Guid.Empty)
            {
                tid = new TemplateID(new ID(Template.Id));
            }
            else
            {
                var ti = Sitecore.Context.Database.GetTemplate(Template.Path);
                tid = new TemplateID(ti.ID);
            }

            var folder = Sitecore.Context.Database.GetItem(Location.ToString());
            if (folder == null)
            {
                throw new ArgumentException("The item " + Location.ToString() + " was not found", "cmd.Location");
            }

            var item = folder.Add(Name, tid);

            item.Editing.BeginEdit();
            foreach (var field in Fields)
            {
                item[field.Name] = field.Value;
            }
            item.Editing.EndEdit();

            return "Created " + item.Paths.Path;
        }
    }

}