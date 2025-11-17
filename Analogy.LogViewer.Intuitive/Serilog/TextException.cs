using System;

namespace Analogy.LogViewer.Intuitive.Serilog
{
    internal sealed class TextException : Exception
    {
        private readonly string text = "";

        public TextException()
        {
        }

        public TextException(string message, Exception innerException) : base(message, innerException)
        {
        }
        public TextException(string text)
            : base("This exception type provides ToString() access to details only.")
        {
            this.text = text;
        }

        public override string ToString()
        {
            return text;
        }
    }
}