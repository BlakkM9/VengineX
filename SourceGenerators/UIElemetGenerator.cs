using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace VengineX.SourceGenerators
{
    [Generator]
    public class UIElemetGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context) { }

        public void Execute(GeneratorExecutionContext context)
        {
            // Find defs file and update dictionary
            UIElementCompiler.UpdateDefinitions(context);

            // Find all gui xml files
            IEnumerable<AdditionalText> guiFiles = context.AdditionalFiles.Where(
                f => f.Path.EndsWith(".xml") && Path.GetFileName(f.Path) != "defs.xml");

            foreach (AdditionalText file in guiFiles)
            {
                SourceText content = file.GetText(context.CancellationToken);

                // Compile the xml file to a partial class.
                string output = UIElementCompiler.Compile(content);

                SourceText sourceText = SourceText.From(output, Encoding.UTF8);

                string fileName = Path.GetFileNameWithoutExtension(file.Path);

                context.AddSource($"{fileName}.g.cs", sourceText);
            }
        }
    }
}
