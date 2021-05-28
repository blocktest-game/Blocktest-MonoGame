using System;
using Microsoft.Xna.Framework.Input;

namespace Blocktest
{
    public class InputEventArgs : EventArgs
    {
        /// <summary>
        /// Holds the state of the keyboard for the event
        /// </summary>
        public KeyboardState userInput {get; set;}
        public double timePassed { get; set; }
    }
}