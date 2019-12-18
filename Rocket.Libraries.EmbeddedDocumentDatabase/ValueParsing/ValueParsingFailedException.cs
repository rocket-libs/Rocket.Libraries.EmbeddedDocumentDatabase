using System;
using System.Collections.Generic;
using System.Text;

namespace Rocket.Libraries.EmbeddedDocumentDatabase.ValueParsing
{
    public class ValueParsingFailedException : Exception
    {
        public ValueParsingFailedException(string message)
            : base(message)
        {
        }

        public ValueParsingFailedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}