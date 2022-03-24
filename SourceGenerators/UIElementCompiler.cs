using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace VengineX.SourceGenerators
{
    public static class UIElementCompiler
    {
        /// <summary>
        /// Mapping of class name to full namespace names, contained in defs.xml.
        /// </summary>
        public static Dictionary<string, string> TypeDefinitions = new Dictionary<string, string>();

        /// <summary>
        /// Mapping of resource strings (first expression before '.') to full class names
        /// </summary>
        public static Dictionary<string, string> ResourceDefinitions = new Dictionary<string, string>();

        public static string Compile(SourceText content) => Compile(content.ToString());


        public static string Compile(string content)
        {
            XElement rootElement = XDocument.Parse(content).Root;
            string className = rootElement.Name.LocalName;
            string nameSpace = rootElement.Attribute("Namespace").Value;

            string fieldsCode = string.Empty;
            string initCode = string.Empty;

            ProcessAttributes(rootElement, "this", ref initCode);

            int index = 0;
            foreach (XElement element in rootElement.Elements())
            {
                ProcessElement(element, "this", ref index, ref fieldsCode, ref initCode);
            }

            //initCode = string.Join("\n", Definitions.Select(pair => $"{pair.Key} => {pair.Value}"));

            return $@"// Generated code

namespace {nameSpace}
{{
    public partial class {className} : VengineX.UI.Elements.Basic.CompositeElement
    {{{fieldsCode}
        protected override void InitializeChildren()
        {{{initCode}
        }}
    }}
}}
";
        }

        private static void ProcessElement(XElement element, string parentName, ref int index, ref string fieldsCode, ref string initCode)
        {
            string elementFullType = ProcessTypeOrValue(element.Name.LocalName);

            string elementName = element.Attribute("Name")?.Value;

            if (elementName != null)
            {
                // Named element (create private field in root class)

                // Add underscore because private member
                elementName = "_" + elementName;

                fieldsCode += $@"
        private {elementFullType} {elementName};";

                // Create instance of element object, attached to parent and saved in field
                initCode += $@"
            {elementName} = new {elementFullType}({parentName});";
            }
            else
            {
                // Unnamed element, generate name
                elementName = "e" + index;

                index++;

                // Create anonym instance of element object, attached to parent.
                initCode += $@"
            {elementFullType} {elementName} = new {elementFullType}({parentName});";
            }



            // Process Attributes
            ProcessAttributes(element, elementName, ref initCode);

            // Process children
            foreach (XElement child in element.Elements())
            {
                ProcessElement(child, elementName, ref index, ref fieldsCode, ref initCode);
            }
        }

        private static void ProcessAttributes(XElement element, string elementName, ref string initCode)
        {
            foreach (XAttribute attr in element.Attributes())
            {
                if (attr.Name.LocalName == "Namespace") { continue; }
                if (attr.Name.LocalName == "Name") { continue; }

                else if (attr.Name == "Layout")
                {
                    string[] values = attr.Value.Split(' ');
                    string fullLayoutName = ProcessTypeOrValue(values[0]);
                    string[] parameters = values.Skip(1).ToArray();

                    initCode += $@"
            {elementName}.{attr.Name} = new {fullLayoutName}({CreateContructorParameters(parameters)});";
                }
                else
                {
                    string value = attr.Value;

                    if (value.StartsWith("'") && value.EndsWith("'"))
                    {
                        //String, repalce '' with ""

                        string strValue = "\"" + value.Substring(1, value.Length - 2) + "\"";

                        initCode += $@"
            {elementName}.{attr.Name} = {strValue};";
                    }
                    else
                    {
                        string[] values = value.Split(' ');

                        if (values.Length == 1)
                        {
                            // Only a single value property.
                            string result = ProcessTypeOrValue(attr.Value);

                            initCode += $@"
            {elementName}.{attr.Name} = {result};";
                        }
                        else
                        {
                            // Multi value property, only numbers allowed here currently!

                            initCode += $@"
            {elementName}.{attr.Name} = new OpenTK.Mathematics.Vector{values.Length}({CreateContructorParameters(values)});";
                        }
                    }
                }
            }
        }


        private static string CreateContructorParameters(string[] parameters)
        {
            string parametersString = string.Empty;

            for (int i = 0; i < parameters.Length; i++)
            {

                parametersString += ProcessTypeOrValue(parameters[i]);

                if (i < parameters.Length - 1) { parametersString += ", "; }
            }

            return parametersString;
        }


        private static string ProcessTypeOrValue(string val)
        {
            if (TypeDefinitions.TryGetValue(val, out string fullType))
            {
                // Just a normal type like "Image"
                return fullType;
            }


            string[] types = val.Split('.');
            if (types.Length == 2)
            {
                // Some type like an enum "HorizontalAlignment.Center"

                string enumType = types[0];
                string enumValue = types[1];

                if (TypeDefinitions.TryGetValue(enumType, out string fullEnumName))
                {
                    return fullEnumName + "." + enumValue;
                }
            }

            if (float.TryParse(val, NumberStyles.Any, new CultureInfo("en-US"), out float value))
            {
                // Just a normal number, converting to float.
                return value.ToString() + "f";
            }

            if (bool.TryParse(val, out bool boolVal))
            {
                return boolVal.ToString().ToLower();
            }

            // Nothing left, only resource string valid now
            string resourceType = val.Split('.')[0];
            return $"VengineX.Resources.ResourceManager.GetResource<{ResourceDefinitions[resourceType]}>(\"{val}\")";
        }


        public static void UpdateDefinitions(string defs)
        {
            TypeDefinitions.Clear();
            ResourceDefinitions.Clear();

            XDocument doc = XDocument.Parse(defs);


            foreach (XElement element in doc.Root.Element("Types").Elements())
            {
                foreach (XElement value in element.Elements())
                {
                    TypeDefinitions.Add(value.Name.LocalName, element.Name.LocalName + "." + value.Name.LocalName);
                }
            }

            foreach (XElement element in doc.Root.Element("Resources").Elements())
            {
                ResourceDefinitions.Add(element.Value, element.Name.LocalName);
            }
        }

        public static void UpdateDefinitions(GeneratorExecutionContext context)
        {
            AdditionalText defsFile = context.AdditionalFiles
                .Where(f => Path.GetFileName(f.Path) == "defs.xml")
                .FirstOrDefault();

            SourceText content = defsFile.GetText(context.CancellationToken);
            UpdateDefinitions(content.ToString());
        }
    }
}
