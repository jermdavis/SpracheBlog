using System;
using System.Collections.Generic;
using Sprache;

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

            var result = CommandTextParser.Any.TryParse(command);

            if(!result.WasSuccessful)
            {
                throw new ArgumentException("Failed to parse '" + command + "' - '" + result.Message + "'", "command");
            }

            return result.Value.Execute();
        }
    }

}