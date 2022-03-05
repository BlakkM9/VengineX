using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace VengineX.Config
{
    public static class SettingsSerializer
    {
        /// <summary>
        /// XML writer settings for serialization.
        /// </summary>
        public static XmlWriterSettings xmlWriterSettings = new XmlWriterSettings()
        {
            Indent = true
        };

        /// <summary>
        /// Serializes the settings dictionary (<see cref="GameSettings._sections"/>) to an XML string.
        /// </summary>
        /// <returns>Resulting XML string.</returns>
        public static string Serialize(Dictionary<string, SettingsSection> settings)
        {
            StringBuilder sb = new StringBuilder();

            using XmlWriter xw = XmlWriter.Create(sb, xmlWriterSettings);
            xw.WriteStartDocument();
            xw.WriteStartElement("Settings");

            foreach (SettingsSection section in settings.Values)
            {
                xw.WriteStartElement("Section");
                xw.WriteAttributeString("name", section.Name);

                foreach (Setting setting in section)
                {
                    xw.WriteStartElement("Setting");

                    xw.WriteElementString("key", setting.Key);
                    xw.WriteElementString("value", setting.Value.ToString());

                    xw.WriteEndElement();
                }

                xw.WriteEndElement();
            }

            xw.WriteEndElement();
            xw.WriteEndDocument();

            xw.Flush();
            xw.Close();

            return sb.ToString();
        }


        /// <summary>
        /// Deserializes the settings from xml string form to Dictionary, ready to use in <see cref="GameSettings"/>
        /// </summary>
        public static Dictionary<string, SettingsSection> Deserialize(string xmlString)
        {
            Dictionary<string, SettingsSection> settings = new Dictionary<string, SettingsSection>();

            XDocument doc = XDocument.Parse(xmlString);
            foreach (XElement section in doc.Elements("Settings"))
            {
                string sectionName = section.Attribute("name").Value;
                settings.Add(sectionName, new SettingsSection(sectionName));

                foreach (XElement setting in section.Elements())
                {
                    
                    string key = setting.Element("key").Value;
                    string strValue = setting.Element("value").Value;

                    // Cast value to correct type (string if failed to parse as int or double)
                    if (int.TryParse(strValue, out int iValue))
                    {
                        settings[sectionName][key] = iValue;
                    }
                    else if (double.TryParse(strValue, out double dValue))
                    {
                        settings[sectionName][key] = dValue;
                    }
                    else
                    {
                        settings[sectionName][key] = strValue;
                    }
                }
            }

            return settings;
        }
    }
}
