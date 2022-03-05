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
    /// Derive from this class and put your own in the <see cref="Game{T}"/>'s type parameter if you need more settings.
    /// </summary>
    public class GameSettings
    {
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


        #region Settings shortcuts

        /// <summary>
        /// <see cref="GameWindowSettings.RenderFrequency"/>
        /// </summary>
        public double RenderFrequency
        {
            get => GetDouble("Display.RenderFrequency");
            set => Set("Display.RenderFrequency", value);
        }


        /// <summary>
        /// <see cref="GameWindowSettings.UpdateFrequency"/>
        /// </summary>
        public double UpdateFrequency
        {
            get => GetDouble("Display.UpdateFrequency");
            set => Set("Display.UpdateFrequency", value);
        }


        /// <summary>
        /// Width of the game window.
        /// </summary>
        public int ResolutionX
        {
            get => GetInt("Window.ResolutionX");
            set => Set("Window.ResolutionX", value);
        }


        /// <summary>
        /// Height of the game window.
        /// </summary>
        public int ResolutionY
        {
            get => GetInt("Window.ResolutionY");
            set => Set("Window.ResolutionY", value);
        }

        #endregion


        public GameSettings()
        {
            _sections = new Dictionary<string, SettingsSection>();
        }


        /// <summary>
        /// Loads the game's settings from xml.<br/>
        /// If settings file is not found, default file is created.<br/>
        /// This function is called in <see cref="Game"/>'s constructor.<br/>
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


        /// <summary>
        /// Fills <see cref="_sections"/> with default settings.<br/>
        /// Override this function if you've overwritten <see cref="GameSettings"/><br/>
        /// and make sure, you're creating all your default settings here.
        /// </summary>
        protected virtual void CreateDefaultSettings()
        {
            RenderFrequency = 0;
            UpdateFrequency = 0;
            ResolutionX = 0;
            ResolutionY = 0;
        }


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


        private object Get(string key)
        {
            string[] tokens = key.Split('.');
            return _sections[tokens[0]][tokens[1]];
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
        private void Set(string key, object value)
        {
            string[] tokens = key.Split('.');

            if (!_sections.ContainsKey(tokens[0]))
            {
                _sections.Add(tokens[0], new SettingsSection(tokens[0]));
            }

            _sections[tokens[0]].Settings[tokens[1]].Value = value;
        }

        #endregion
    }
}
