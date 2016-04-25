using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sprache;

namespace SpracheBlog
{


    /*
    public class FancyCommand
    {
        public static Parser<Guid> ItemID =
            from openBrace in Parse.Char('{')
            from identifier in Parse.CharExcept('}').Many().Text()
            from closeBrace in Parse.Char('}')
            select Guid.Parse(identifier);

        public static char[] Slashes = new char[] { '\\', '/' };
        public static char[] InvalidNameChars = new char[] { '\\', '/', ':', '?', '!', '"', '<', '>', ';', '|', '[', ']' };

        private static Parser<char> Slash =
            from slash in Parse.Chars(Slashes)
            select slash;

        private static Parser<string> PathFragment =
            from leadingSlash in FancyCommand.Slash
            from fragment in Parse.CharExcept(InvalidNameChars).Many().Text()
            select leadingSlash + fragment;

        public static Parser<string> ItemPath =
            from path in FancyCommand.PathFragment.AtLeastOnce()
            select String.Concat(path);

    }
    */
}