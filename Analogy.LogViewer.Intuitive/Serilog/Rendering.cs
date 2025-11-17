namespace Analogy.LogViewer.Intuitive.Serilog
{
    internal sealed class Rendering
    {
        public string Name { get; }
        public string Format { get; }
        public string Rendered { get; }

        public Rendering(string name, string format, string rendered)
        {
            Name = name;
            Format = format;
            Rendered = rendered;
        }
    }
}