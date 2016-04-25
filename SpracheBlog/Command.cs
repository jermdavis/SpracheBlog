using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprache;

namespace SpracheBlog
{

    public class Field
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class CreateCommand : Command
    {
        public string Template { get; set; }
        public string Location { get; set; }
        public IEnumerable<Field> Fields { get; set; }
    }

    public class MoveCommand : Command
    {
        public string Item { get; set; }
        public string NewLocation { get; set; }
    }

    public class DeleteCommand : Command
    {
        public string Item { get; set; }
    }

    public class Command
    {
        public static Parser<Field> Field =
            from name in Parse.AnyChar.Until(Parse.Char('=')).Text()
            from openQuote in Parse.Char('"')
            from value in Parse.AnyChar.Until(Parse.Char('"')).Text()
            select new Field() { Name = name, Value = value };

        private static IEnumerable<T> Concatenate<T>(T first, IEnumerable<T> rest)
        {
            yield return first;
            foreach(T item in rest)
            {
                yield return item;
            }
        }

        // create <template:id/path> under <location:id/path> with <property=value>[,etc]
        public static Parser<Command> CreateCommand =
            from cmd in Parse.IgnoreCase("create").Token()
            from template in Parse.AnyChar.Until(Parse.WhiteSpace).Text().Token()
            from under in Parse.IgnoreCase("under").Token()
            from location in Parse.AnyChar.Until(Parse.WhiteSpace).Text().Token()
            from with in Parse.IgnoreCase("with").Token()
            from fields in ( 
                from first in Field
                from rest in Parse.Char(',').Then(_ => Field).Many()
                select Concatenate(first, rest) )
            select new CreateCommand() { Template = template, Location = location, Fields = fields };

        // move <item:id/path> to <location:id/path>
        public static Parser<Command> MoveCommand =
            from cmd in Parse.IgnoreCase("move").Token()
            from item in Parse.AnyChar.Until(Parse.WhiteSpace).Text().Token()
            from to in Parse.IgnoreCase("to").Token()
            from newLocation in Parse.AnyChar.Many().Text()
            select new MoveCommand() { Item = item, NewLocation = newLocation };

        //delete <item:id/path>
        public static Parser<Command> DeleteCommand =
            from cmd in Parse.IgnoreCase("delete").Token()
            from item in Parse.AnyChar.Many().Text()
            select new DeleteCommand() { Item = item };

        public static Parser<Command> Any =
            CreateCommand
            .XOr(MoveCommand)
            .XOr(DeleteCommand);
    }

}