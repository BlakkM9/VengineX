using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using VengineX.Debugging.Logging;
using VengineX.Resources;
using VengineX.UI.Elements;
using VengineX.UI.Layouts;

namespace VengineX.UI.Serialization
{
    /// <summary>
    /// Class that is used to serialize UI from XML
    /// </summary>
    public class UISerializer
    {
        private static readonly string ENGINE_UI_NAMESPACE = "VengineX.UI";
        private static readonly BindingFlags ANY_INSTANCED = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
        
        /// <summary>
        /// Assembly of the game. This is received as the entry assembly in the constructor.
        /// </summary>
        private readonly Assembly _gameAssembly;

        /// <summary>
        /// Assembly of the VengineX engine.
        /// </summary>
        private readonly Assembly _engineAssembly;

        /// <summary>
        /// All full type names in <see cref="ENGINE_UI_NAMESPACE"/>.
        /// </summary>
        private readonly string[] _engineUITypeNames;

        /// <summary>
        /// All full type names in the users provided ui namespace.
        /// </summary>
        private readonly string[]? _gameUITypeNames;


        /// <summary>
        /// Creates a new UI serializer that also takes ui elements in <paramref name="uiNamespace"/> into account.<br/>
        /// </summary>
        /// <param name="uiNamespace">Namespace where all your <see cref="UIElement"/> are located.</param>
        public UISerializer(string? uiNamespace)
        {
            Assembly? engineAssembly = GetType().Assembly;
            Assembly? gameAssembly = Assembly.GetEntryAssembly();


            if (gameAssembly == null || engineAssembly == null)
            {
                throw new Exception("This constructor must not be called from unmanaged code!");
            }

            _engineAssembly = engineAssembly;
            _gameAssembly = gameAssembly;

            // Fill arrays with all the full type names

            // Engine UIElements
            List<string> engineUITypeNames = new List<string>();
            foreach (Type type in _engineAssembly.GetTypes())
            {
                if (type.FullName.Contains(ENGINE_UI_NAMESPACE)) { engineUITypeNames.Add(type.FullName); }
            }
            _engineUITypeNames = engineUITypeNames.ToArray();

            // Game UIElements
            if (uiNamespace != null)
            {
                List<string> gameUITypes = new List<string>();
                foreach (Type type in _gameAssembly.GetTypes())
                {
                    if (type.FullName.Contains(uiNamespace)) { gameUITypes.Add(type.FullName); }
                }
                _gameUITypeNames = gameUITypes.ToArray();
            }
        }


        /// <summary>
        /// Loads the given <see cref="LoadableUITemplate"/> from given file path.
        /// </summary>
        /// <param name="filePath">The file path to load this UI from.</param>
        /// <param name="parent">The element to attach the loaded UI as child.</param>
        /// <returns>The loaded <see cref="LoadableUITemplate"/></returns>
        public T LoadFromXML<T>(UIElement parent, string filePath) where T : LoadableUITemplate
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            XDocument doc = XDocument.Load(filePath);

            sw.Stop();
            Logger.Log(Severity.Debug, Tag.Profiling, $"Loaded from file: {sw.ElapsedMilliseconds}ms");

            // Create the root element
            Type type = typeof(T);

            sw.Restart();

            T rootElement = ProcessNode<T>(null, parent, doc.Root);

            sw.Stop();
            Logger.Log(Severity.Debug, Tag.Profiling, $"Inflated ui tree: {sw.ElapsedMilliseconds}ms");

            parent.UpdateLayout();

            MethodInfo initMethod = typeof(T).GetMethod("Initialized", ANY_INSTANCED);
            initMethod.Invoke(rootElement, null);



            return rootElement;
        }


        /// <summary>
        /// Processes the given node recursively.
        /// To process the root element itself, pass <see langword="null"/> as <paramref name="rootUIElement"/>.
        /// </summary>
        private T ProcessNode<T>(T? rootUIElement, UIElement uiParent, XElement xmlElement) where T : UIElement
        {
            // Create instance of element
            Type? type = StringToType(xmlElement.Name.LocalName);

            if (type == null) { throw new Exception(xmlElement.Name.LocalName + " is missing in the current assemblies!"); }


            if (Activator.CreateInstance(type, new object[] { uiParent }) is not UIElement uiChild)
            {
                throw new Exception("Failed to create instance of " + type.FullName + "! Check if it is inheriting from UIElement.");
            }


            if (rootUIElement == null)
            {
                // The created element was the root
                rootUIElement = (T)uiChild;
            }


            // Set properties of this element
            SetProperties(rootUIElement, type, uiChild, xmlElement);

            // Process child nodes of this node
            foreach (XElement e in xmlElement.Elements())
            {
                ProcessNode(rootUIElement, uiChild, e);
            }

            return rootUIElement;
        }


        /// <summary>
        /// Searches in <see cref="_engineUITypeNames"/> and <see cref="_gameUITypeNames"/> for<br/>
        /// the given type name and returns the type itself.<br/>
        /// Returns null if no type was found. Types in game will be preferred over engine types.
        /// </summary>
        private Type? StringToType(string type)
        {
            Type? foundType = null;


            // Search in venginex first
            foreach (string fullType in _engineUITypeNames)
            {
                if (fullType.EndsWith("." + type))
                {
                    foundType = _engineAssembly.GetType(fullType);
                    break;
                }
            }


            // Search in game second (so venginex ones gets overwritten, but log warning if so)
            if (_gameUITypeNames != null)
            {
                foreach (string fullType in _gameUITypeNames)
                {
                    if (fullType.EndsWith("." + type))
                    {
                        Type gameType = _gameAssembly.GetType(fullType);
                        
                        if (foundType != null && gameType != null)
                        {
                            Logger.Log(Severity.Info, $"Your UIElement {gameType.FullName} will overwrite VengineX's {foundType.FullName}");
                        }

                        foundType = gameType;
                        return foundType;
                    }
                }
            }

            return foundType;
        }


        /// <summary>
        /// Sets all the properties for the current element and the name fields in root element.
        /// </summary>
        private void SetProperties<T>(T rootUIElement, Type type, UIElement element, XElement xmlElement) where T : UIElement
        {
            foreach (XAttribute attr in xmlElement.Attributes())
            {
                string attributeName = attr.Name.LocalName;

                // Set the field in the root to this element
                if (attributeName == "Name")
                {
                    FieldInfo? targetField = typeof(T).GetField(attr.Value, ANY_INSTANCED);

                    if (targetField == null)
                    {
                        throw new Exception($"Failed find field '{attr.Value}' in root element ({rootUIElement.GetType().Name}).");
                    }

                    targetField.SetValue(rootUIElement, element);
                }
                else
                {
                    PropertyInfo targetProperty = type.GetProperty(attributeName);
                    targetProperty.SetValue(element, Convert(attr.Value, targetProperty.PropertyType));
                }
            }
        }


        /// <summary>
        /// Tries to converts a string value to the given type.
        /// </summary>
        private object? Convert(string value, Type targetType)
        {
            // Check if enum
            if (targetType.IsEnum)
            {
                // Convert to enum (works also for C# code like enum syntax: EnumName.EnumValue but also for EnumValue
                return Enum.Parse(targetType, value.Replace(targetType.Name + ".", ""), true);
            }

            // Check if resource string
            if (typeof(IResource).IsAssignableFrom(targetType))
            {
                // Get from resource manager
                MethodInfo method = typeof(ResourceManager).GetMethod("GetResource", BindingFlags.Public | BindingFlags.Static);
                method = method.MakeGenericMethod(targetType);
                return method.Invoke(null, new object[] { value });
            }

            // Check if Layout
            if (targetType == typeof(Layout))
            {
                string[] values = value.Split(' ');

                // Find the layout type
                Type layoutType = StringToType(values[0]);

                // Get constructor. Only one constructor allowed for layouts.
                ConstructorInfo targetConstructor = layoutType.GetConstructors()[0];

                // Convert given parameters to constructors types
                ParameterInfo[] ctrParameters = targetConstructor.GetParameters();

                object[] parameters = new object[ctrParameters.Length];

                for (int i = 0; i < ctrParameters.Length; i++)
                {
                    parameters[i] = Convert(values[i + 1], ctrParameters[i].ParameterType);
                }

                return Activator.CreateInstance(layoutType, parameters);
            }

            // Try to cast
            try
            {
                return System.Convert.ChangeType(value, targetType);
            }
            // Create instance of object with given value as constructor values (seperated by a single space)
            catch (InvalidCastException _)
            {
                // Converts parameters to float, so only works currently if there is a constructor only taking floats.
                string[] values = value.Split(' ');
                object[] parameters = new object[values.Length];

                for (int i = 0; i < values.Length; i++)
                {
                    parameters[i] = float.Parse(values[i]);
                }

                return Activator.CreateInstance(targetType, parameters);
            }
        }
    }
}
