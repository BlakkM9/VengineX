using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VengineX.Config
{
    /// <summary>
    /// Represents a settings entry in the xml file, located within a <see cref="SettingsSection"/>.
    /// </summary>
    public class Setting
    {
        /// <summary>
        /// The key for this setting.
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// The value of this setting
        /// </summary>
        public object Value { get; set; }


        /// <summary>
        /// Creates a new setting with given key (without section) and value.
        /// </summary>
        public Setting(string key, object value)
        {
            Key = key;
            Value = value;
        }
    }
}
