using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.Core;

namespace VengineX.Config
{
    /// <summary>
    /// Base class for settings that store all settings for the game (not keymap).<br/>
    /// </summary>
    public abstract class GameSettingsBase
    {
        /// <summary>
        /// Handler when a setting is changed.
        /// </summary>
        /// <param name="key">Key of the setting.</param>
        /// <param name="value">Value of the setting.</param>
        public delegate void SettingChangedEventHandler(string key, object value);

        /// <summary>
        /// This event occurs when a setting is changed (via <see cref="Set(string, object)"/>).
        /// </summary>
        public event SettingChangedEventHandler? SettingChanged;

        /// <summary>
        /// Path to the settings file.<br/>
        /// File is moved when settings path is changed.
        /// </summary>
        public virtual string SettingsPath
        {
            get => _settingsPath;
            set
            {
                // Move settings file to new path
                File.Move(_settingsPath, value);
                _settingsPath = value;
            }
        }
        private string _settingsPath = AppDomain.CurrentDomain.BaseDirectory + "/Settings.xml";


        /// <summary>
        /// Dictionary holding all the sections with the section name as key.
        /// </summary>
        private Dictionary<string, SettingsSection> _sections;


        public GameSettingsBase()
        {
            _sections = new Dictionary<string, SettingsSection>();
        }


        /// <summary>
        /// Loads the game's settings from xml.<br/>
        /// If settings file is not found, default file is created.<br/>
        /// This function is called in <see cref="Game{T}"/>'s constructor.<br/>
        /// </summary>
        public void LoadOrDefault()
        {
            // Create settings file if not existing.
            if (!File.Exists(SettingsPath))
            {
                CreateDefaultSettings();
                Save();
            }

            string xmlString = File.ReadAllText(SettingsPath);
            _sections = SettingsSerializer.Deserialize(xmlString);
        }



        protected abstract void CreateDefaultSettings();


        /// <summary>
        /// Saves the settings that are in <see cref="_sections"/> to the settings file.
        /// </summary>
        public void Save()
        {
            string xmlString = SettingsSerializer.Serialize(_sections);
            File.WriteAllText(SettingsPath, xmlString);
        }


        #region Settings access with casting

        /// <summary>
        /// Gets the setting with given key (section.key) and tries to cast it to bool.
        /// </summary>
        public bool GetBool(string key)
        {
            return (bool)Get(key);
        }


        /// <summary>
        /// Gets the setting with given key (section.key) and tries to cast it to int.
        /// </summary>
        public int GetInt(string key)
        {
            return (int)Get(key);
        }


        /// <summary>
        /// Gets the setting with given key (section.key) and tries to cast it to double.
        /// </summary>
        public double GetDouble(string key)
        {
            return (double)Get(key);
        }


        /// <summary>
        /// Gets the setting with given key (section.key) and tries to cast it to string.
        /// </summary>
        public string GetString(string key)
        {
            return (string)Get(key);
        }


        /// <summary>
        /// Gets the setting with given key (section.key) without casting.
        /// </summary>
        public object Get(string key)
        {
            string[] tokens = key.Split('.');
            return _sections[tokens[0]][tokens[1]];
        }


        /// <summary>
        /// Sets the setting for given key (section.key) to given value.
        /// </summary>
        public void Set(string key, bool value)
        {
            Set(key, (object)value);
        }


        /// <summary>
        /// Sets the setting for given key (section.key) to given value.
        /// </summary>
        public void Set(string key, int value)
        {
            Set(key, (object)value);
        }


        /// <summary>
        /// Sets the setting for given key (section.key) to given value.
        /// </summary>
        public void Set(string key, double value)
        {
            Set(key, (object)value);
        }


        /// <summary>
        /// Sets the setting for given key (section.key) to given value.
        /// </summary>
        public void Set(string key, string value)
        {
            Set(key, (object)value);
        }


        /// <summary>
        /// Sets the setting for given key (section.key) to given value.<br/>
        /// Creates new sections if needed.
        /// </summary>
        public void Set(string key, object value)
        {
            string[] tokens = key.Split('.');

            if (!_sections.ContainsKey(tokens[0]))
            {
                _sections.Add(tokens[0], new SettingsSection(tokens[0]));
            }

            _sections[tokens[0]][tokens[1]] = value;

            SettingChanged?.Invoke(key, value);
        }

        #endregion
    }
}
