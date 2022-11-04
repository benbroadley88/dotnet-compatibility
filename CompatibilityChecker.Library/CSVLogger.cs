namespace CompatibilityChecker.Library
{
    using System;
    using System.Collections.Generic;
    using static System.Net.Mime.MediaTypeNames;
    using System.Text;
    using System.Linq;
    using System.IO;

    public class CSVLogger : IMessageLogger
    {
        private readonly string mFilename;

        private List<Message> mMessages = new List<Message>();

        public CSVLogger(string filename)
        {
            mFilename = filename;
        }

        public virtual void Report(Message message)
        {
            mMessages.Add(message);
            Console.Error.WriteLine(message);
        }

        public void Write()
        {
            var csv = new StringBuilder();

            var csvHeaderLine = string.Join(",", 
                //"Severity",  // Commented because it's not useful in this usecase
                //"Descriptor Title", // can be inferred from rule id
                "Descriptor Rule Id",
                "Descriptor Category");
            var argumentColsRequired = mMessages.Select(message => message.Arguments.Length).Max();

            for (var i = 1; i <= argumentColsRequired; i++)
            {
                csvHeaderLine += $", Argument {i}";
            }

            csv.AppendLine(csvHeaderLine);

            foreach (var message in mMessages)
            {
                var argsStrings = new List<string>();
                foreach (var argKVP in message.Arguments)
                {
                    argsStrings.Add($"{argKVP.key} = {argKVP.value.Replace(", ", "|")}");
                }

                var csvLine = string.Join(",",
                    //message.Severity,  // always "error" so not useful.
                    //message.Descriptor.Title, // inferred from ruleID
                    message.Descriptor.RuleId,
                    message.Descriptor.Category);

                // message.Descriptor.Description is always empty.

                if (argsStrings.Count > 0)
                {
                    csvLine += $",{string.Join(",", argsStrings)}";
                }

                csv.AppendLine(csvLine);
            }

            File.WriteAllText(mFilename, csv.ToString());
        }

        public static bool ValidateFilename(string filename) => !string.IsNullOrEmpty(filename);
    }
}
