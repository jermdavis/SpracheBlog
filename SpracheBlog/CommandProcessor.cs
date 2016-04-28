using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprache;
using Sitecore.Data.Items;
using Sitecore.Data;

namespace SpracheBlog
{

    public class CommandProcessor
    {
        public void Run(IEnumerable<string> commands)
        {
            foreach(string command in commands)
            {
                Run(command);
            }
        }

        public string Run(string command)
        {
            if(string.IsNullOrWhiteSpace(command))
            {
                return string.Empty;
            }

            var result = CommandParser.Any.TryParse(command);

            if(!result.WasSuccessful)
            {
                throw new ArgumentException("Failed to parse '" + command + "' - '" + result.Message + "'", "command");
            }

            return dispatchResult(result);
        }

        private string dispatchResult(IResult<CommandParser> result)
        {
            if (result.Value is DeleteCommand)
            {
                return processDelete(result.Value as DeleteCommand);
            }
            else if (result.Value is MoveCommand)
            {
                return processMove(result.Value as MoveCommand);
            }
            else if (result.Value is CreateCommand)
            {
                return processCreate(result.Value as CreateCommand);
            }
            else
            {
                throw new ArgumentException("Unexpected result type: " + result.Value.GetType().Name, "result");
            }
        }

        private string processDelete(DeleteCommand cmd)
        {
            var itm = Sitecore.Context.Database.GetItem(cmd.Item.ToString());
            if(itm == null)
            {
                throw new ArgumentException("The item " + cmd.Item.ToString() + " was not found", "cmd.Item");
            }

            itm.Delete();

            return "Deleted " + cmd.Item.ToString();
        }

        private string processMove(MoveCommand cmd)
        {
            var folder = Sitecore.Context.Database.GetItem(cmd.NewLocation.ToString());
            if(folder == null)
            {
                throw new ArgumentException("The item " + cmd.NewLocation.ToString() + " was not found", "cmd.NewLocation");
            }

            var item = Sitecore.Context.Database.GetItem(cmd.Item.ToString());
            if (item == null)
            {
                throw new ArgumentException("The item " + cmd.Item.ToString() + " was not found", "cmd.Item");
            }

            item.MoveTo(folder);

            return "Moved " + cmd.Item.ToString();
        }

        private string processCreate(CreateCommand cmd)
        {
            TemplateID tid;
            if(cmd.Template.Id != Guid.Empty)
            {
                tid = new TemplateID(new ID(cmd.Template.Id));
            }
            else
            {
                var ti = Sitecore.Context.Database.GetTemplate(cmd.Template.Path);
                tid = new TemplateID(ti.ID);
            }

            var folder = Sitecore.Context.Database.GetItem(cmd.Location.ToString());
            if(folder == null)
            {
                throw new ArgumentException("The item " + cmd.Location.ToString() + " was not found", "cmd.Location");
            }

            var item = folder.Add(cmd.Name, tid);

            item.Editing.BeginEdit();
            foreach(var field in cmd.Fields)
            {
                item[field.Name] = field.Value;
            }
            item.Editing.EndEdit();

            return "Created " + item.Paths.Path;
        }
    }

}