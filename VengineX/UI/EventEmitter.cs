using OpenTK.Windowing.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VengineX.UI.Elements.Basic;

namespace VengineX.UI
{
    /// <summary>
    /// Class that manages events and event state for <see cref="Element"/>.
    /// </summary>
    public class EventEmitter
    {
        private readonly Element _element;

        /// <summary>
        /// Handler for generic ui events.
        /// </summary>
        public delegate void UIEventHandler(Element sender);

        /// <summary>
        /// Handler mouse move events.
        /// </summary>
        public delegate void MouseMoveEventHandler(Element sender, MouseMoveEventArgs args);

        /// <summary>
        /// Handler mouse button events.
        /// </summary>
        public delegate void MouseButtonEventHandler(Element sender, MouseButtonEventArgs args);

        /// <summary>
        /// Handler wheel events.
        /// </summary>
        public delegate void MouseWheelEventHandler(Element sender, MouseWheelEventArgs args);

        /// <summary>
        /// Handler keyboard events.
        /// </summary>
        public delegate void KeyboardKeyEventHandler(Element sender, KeyboardKeyEventArgs args);

        /// <summary>
        /// Handles text input events.
        /// </summary>
        public delegate void TextInputEventHandler(Element sender, TextInputEventArgs args);

        /// <summary>
        /// The mouse cursor entered this UI element.
        /// </summary>
        public event MouseMoveEventHandler? Entered;

        /// <summary>
        /// The mouse cursor left this UI element.
        /// </summary>
        public event MouseMoveEventHandler? Left;

        /// <summary>
        /// Any mousebutton was pressed while above this element.
        /// </summary>
        public event MouseButtonEventHandler? MouseButtonPressed;

        /// <summary>
        /// Any mousebutton was released while above this element.
        /// </summary>
        public event MouseButtonEventHandler? MouseButtonReleased;

        /// <summary>
        /// Occus when this element was clicked with any mouse button.
        /// </summary>
        public event MouseButtonEventHandler? Clicked;

        /// <summary>
        /// Occurs when this a scroll happened on this element.
        /// </summary>
        public event MouseWheelEventHandler? Scrolled;

        /// <summary>
        /// Occurs when this element gains keyboard focus.
        /// </summary>
        public event UIEventHandler? GainedFocus;

        /// <summary>
        /// Occurs when this element lost keyboard focus.
        /// </summary>
        public event UIEventHandler? LostFocus;

        /// <summary>
        /// Occurs when this elemet receives a text input.
        /// </summary>
        public event TextInputEventHandler? TextInput;

        /// <summary>
        /// Occurs when this element receives a key press.<br/>
        /// Use <see cref="TextInput"/> if you want to use the<br/>
        /// event argument as string input.
        /// </summary>
        public event KeyboardKeyEventHandler? KeyPressed;

        /// <summary>
        /// Occurs when this element receives a key release.
        /// </summary>
        public event KeyboardKeyEventHandler? KeyReleased;

        /// <summary>
        /// Wether or not this element has currently keyboard focus.
        /// </summary>
        public bool Focused { get; internal set; }

        /// <summary>
        /// Wether or not the mouse cursor is currently over this UI element.
        /// </summary>
        public bool MouseOver { get; internal set; }

        /// <summary>
        /// Wether or not any mouse button is down on this element.
        /// </summary>
        public bool MouseDown { get; internal set; }

        /// <summary>
        /// Saves if a click started inside this element.<br/>
        /// It is also a valid click if you drag out and reenter the element.
        /// </summary>
        public bool ClickInitiated { get; internal set; }

        /// <summary>
        /// Creates a new event emitter for the given UI element.
        /// </summary>
        public EventEmitter(Element element)
        {
            _element = element;
        }

        public void InvokeEntered(MouseMoveEventArgs args) => Entered?.Invoke(_element, args);

        public void InvokeLeft(MouseMoveEventArgs args) => Left?.Invoke(_element, args);

        public void InvokeMouseButtonPressed(MouseButtonEventArgs args) => MouseButtonPressed?.Invoke(_element, args);

        public void InvokeMouseButtonReleased(MouseButtonEventArgs args) => MouseButtonReleased?.Invoke(_element, args);

        public void InvokeClicked(MouseButtonEventArgs args) => Clicked?.Invoke(_element, args);

        public void InvokeScrolled(MouseWheelEventArgs args) => Scrolled?.Invoke(_element, args);

        public void InvokeGainedFocus() => GainedFocus?.Invoke(_element);

        public void InvokeLostFocus() => LostFocus?.Invoke(_element);

        public void InvokeKeyPressed(KeyboardKeyEventArgs args) => KeyPressed?.Invoke(_element, args);

        public void InvokeKeyReleased(KeyboardKeyEventArgs args) => KeyReleased?.Invoke(_element, args);

        public void InvokeTextInput(TextInputEventArgs args) => TextInput?.Invoke(_element, args);
    }
}
