using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VengineX.Config
{
    /// <summary>
    /// Represents a section for settings within the game's settings xml file.
    /// </summary>
    public class SettingsSection : IEnumerable<Setting>
    {
        /// <summary>
        /// Name of the section.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// All settings within that section.
        /// </summary>
        public Dictionary<string, Setting> Settings { get; private set; }


        /// <summary>
        /// Smart access to settings values in this section.<br/>
        /// Creates new setting if needed.
        /// </summary>
        /// <param name="key">the setting only key (without section).</param>
        /// <returns>Value of setting with given key.</returns>
        public object this[string key]
        {
            get
            {
                return Settings[key].Value;
            }
            set
            {
                if (Settings.ContainsKey(key))
                {
                    Settings[key].Value = value;
                }
                else
                {
                    Settings.Add(key, new Setting(value.GetType(), key, value));
                }
            }
        }


        public SettingsSection(string Name)
        {
            this.Name = Name;
            Settings = new Dictionary<string, Setting>();
        }


        #region IEnumerable

        public IEnumerator<Setting> GetEnumerator()
        {
            return Settings.Values.GetEnumerator();
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return Settings.Values.GetEnumerator();
        }

        #endregion
    }
}
