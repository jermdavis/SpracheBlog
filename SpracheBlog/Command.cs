using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprache;

namespace SpracheBlog
{

    public class Command
    {
        public static IEnumerable<char> InvalidNameCharacters = new List<char> { '\\', '/', ':', '?', '"', '<', '>', '|', '[', ']', ' ', '!' };

        public static Parser<string> ItemName =
            Parse.CharExcept(InvalidNameCharacters).Many().Text().Token();

        public static Parser<Field> Field =
            from name in Parse.CharExcept(new char[] { '=', ' ' }).Many().Text()
            from equalSign in Parse.Char('=').Token()
            from openQuote in Parse.Char('"')
            from value in Parse.CharExcept('"').Many().Text()
            from closeQuote in Parse.Char('"') 
            select new Field() { Name = name, Value = value };

        // create <template:id/path> named <name> under [<location:id/path> with <property="value">[,etc]]
        public static Parser<Command> CreateCommand =
            from cmd in Parse.IgnoreCase("create").Token()
            from template in Parse.AnyChar.Until(Parse.WhiteSpace).Text().Token()
            from named in Parse.IgnoreCase("named").Token()
            from itemName in ItemName.Token()
            from under in Parse.IgnoreCase("under").Token()
            from location in Parse.CharExcept(' ').Many().Text()
            from fieldValues in (
                from with in Parse.IgnoreCase("with").Token()
                from fields in (
                    from first in Field
                    from rest in Parse.Char(',').Token().Then(_ => Field).Many()
                    select first.Concatenate(rest))
                select fields
                ).Optional()
            select new CreateCommand() { Template = template, Name = itemName, Location = location, Fields = fieldValues.GetOrElse(new List<Field>()) };

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