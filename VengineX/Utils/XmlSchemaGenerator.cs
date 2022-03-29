using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using VengineX.SourceGenerators;

namespace VengineX.Utils
{
    // TODO call this automatically when defs changed from within source code generator
    /// <summary>
    /// Generates an xml schema for UI xml files, based on the defs.<br/>
    /// This class makes heavy usage of reflection, do only use it to generate the schema once when defs are changed.
    /// </summary>
    public static class XmlSchemaGenerator
    {

        public static void Generate()
        {
            XmlSchema uiSchema = new XmlSchema();

            UIElementCompiler.UpdateDefinitions(File.ReadAllText("res/gui/defs.xml"));
            foreach (KeyValuePair<string, string> kvp in UIElementCompiler.TypeDefinitions)
            {
                string elementName = kvp.Key;
                string typeName = kvp.Value;

                XmlSchemaElement element = new XmlSchemaElement();
                element.Name = elementName;

                XmlSchemaComplexType elementType = new XmlSchemaComplexType();

                Type classType = Assembly.GetEntryAssembly().GetType(typeName);

                if (classType == null)
                {
                    classType = typeof(XmlSchemaGenerator).Assembly.GetType(typeName);
                }

                foreach (PropertyInfo property in classType.GetProperties())
                {
                    Console.WriteLine(property.Name);
                    XmlSchemaAttribute attrib = new XmlSchemaAttribute();
                    attrib.Name = property.Name;
                    elementType.Attributes.Add(attrib);
                }

                element.SchemaType = elementType;
                uiSchema.Items.Add(element);
                //uiSchema.
            }



            // Create an XmlSchemaSet to compile the customer schema.
            XmlSchemaSet schemaSet = new XmlSchemaSet();
            schemaSet.ValidationEventHandler += new ValidationEventHandler(ValidationCallback);
            schemaSet.Add(uiSchema);
            schemaSet.Compile();

            foreach (XmlSchema schema in schemaSet.Schemas())
            {
                uiSchema = schema;
            }

            // Write the complete schema to the Console.
            uiSchema.Write(Console.Out);
        }

        static void ValidationCallback(object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Warning)
                Console.Write("WARNING: ");
            else if (args.Severity == XmlSeverityType.Error)
                Console.Write("ERROR: ");

            Console.WriteLine(args.Message);
        }
    }
}
