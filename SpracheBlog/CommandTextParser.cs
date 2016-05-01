using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprache;

namespace SpracheBlog
{

    public class CommandTextParser
    {
        public static IEnumerable<char> InvalidNameCharacters = new List<char> { '\\', '/', ':', '?', '"', '<', '>', '|', '[', ']', ' ', '!' };

        public static Parser<string> ItemName =
            Parse.CharExcept(InvalidNameCharacters).AtLeastOnce().Text().Token();

        public static Parser<ItemIdenitfier> ItemId =
            from openBrace in Parse.Char('{')
            from id in Parse.LetterOrDigit.Or(Parse.Char('-')).Repeat(36).Text()
            from closeBrase in Parse.Char('}')
            select new ItemIdenitfier() { Path = string.Empty, Id = Guid.Parse(id) };

        public static Parser<string> PathSegment =
            from slash in Parse.Chars(new char[] { '\\', '/' })
            from name in ItemName
            select name;

        public static Parser<ItemIdenitfier> ItemPath =
            from parts in (
                from firstSegment in PathSegment
                from otherSegments in PathSegment.Many()
                select firstSegment.Concatenate(otherSegments)
            )
            from trailingSlash in Parse.Char('\\').Optional()
            select new ItemIdenitfier() { Id=Guid.Empty, Path = "/" + string.Join("/", parts) };

        public static Parser<ItemIdenitfier> PathOrID =
            ItemPath
            .XOr(ItemId).Token();

        public static Parser<string> QuotedFieldName =
            from openQuote in Parse.Char('"')
            from value in Parse.CharExcept('"').Many().Text()
            from closeQuote in Parse.Char('"')
            select value;

        public static Parser<string> UnquotedFieldName =
            from name in Parse.CharExcept(new char[] { '=', ' ' }).Many().Text()
            select name;

        public static Parser<Field> Field =
            from name in QuotedFieldName.XOr(UnquotedFieldName)
            from equalSign in Parse.Char('=').Token()
            from openQuote in Parse.Char('"')
            from value in Parse.CharExcept('"').Many().Text()
            from closeQuote in Parse.Char('"') 
            select new Field() { Name = name, Value = value };

        // create <template:id/path> named <name> under [<location:id/path> with <property="value">[,etc]]
        public static Parser<ICommand> CreateCommand =
            from cmd in Parse.IgnoreCase("create").Token()
            from template in PathOrID
            from named in Parse.IgnoreCase("named").Token()
            from itemName in ItemName.Token()
            from under in Parse.IgnoreCase("under").Token()
            from location in PathOrID
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
        public static Parser<ICommand> MoveCommand =
            from cmd in Parse.IgnoreCase("move").Token()
            from item in PathOrID
            from to in Parse.IgnoreCase("to").Token()
            from newLocation in PathOrID
            select new MoveCommand() { Item = item, NewLocation = newLocation };

        //delete <item:id/path>
        public static Parser<ICommand> DeleteCommand =
            from cmd in Parse.IgnoreCase("delete").Token()
            from item in PathOrID
            select new DeleteCommand() { Item = item };

        public static Parser<ICommand> Any =
            CreateCommand
            .XOr(MoveCommand)
            .XOr(DeleteCommand);
    }

}