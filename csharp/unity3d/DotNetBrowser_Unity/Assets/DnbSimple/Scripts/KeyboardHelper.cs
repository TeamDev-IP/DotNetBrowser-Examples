#region Copyright

// Copyright © 2024, TeamDev. All rights reserved.
// 
// Redistribution and use in source and/or binary forms, with or without
// modification, must retain the above copyright notice and the following
// disclaimer.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
// A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
// OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
// LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
// THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

#endregion

using System.Collections.Generic;
using DotNetBrowser.Input.Keyboard;
using DotNetBrowser.Input.Keyboard.Events;
using UnityEngine;
using UnityKeyCode = UnityEngine.KeyCode;
using DnbKeyCode = DotNetBrowser.Input.Keyboard.Events.KeyCode;

namespace Assets.Scripts
{
    internal class KeyboardHelper
    {
        private readonly IKeyboard keyboard;

        #region Key codes mapping

        private readonly IReadOnlyDictionary<UnityKeyCode, DnbKeyCode>
            keyCodesMapping =
                new Dictionary<UnityKeyCode, DnbKeyCode>
                {
                    //   <para>Not assigned (never returned as the result of a keystroke).</para>
                    {UnityKeyCode.None, DnbKeyCode.Unknown},

                    //   <para>The backspace key.</para>
                    {
                        UnityKeyCode.Backspace,
                        DnbKeyCode.Back
                    },

                    //   <para>The tab key.</para>
                    {
                        UnityKeyCode.Tab,
                        DnbKeyCode.Tab
                    },

                    //   <para>The Clear key.</para>
                    {
                        UnityKeyCode.Clear,
                        DnbKeyCode.Clear
                    },

                    //   <para>Return key.</para>
                    {
                        UnityKeyCode.Return,
                        DnbKeyCode.Return
                    },

                    //   <para>Pause on PC machines.</para>
                    {UnityKeyCode.Pause, DnbKeyCode.Pause},

                    //   <para>Escape key.</para>
                    {UnityKeyCode.Escape, DnbKeyCode.Escape},

                    //   <para>Space key.</para>
                    {UnityKeyCode.Space, DnbKeyCode.Space},
                    /*
                    //   <para>Exclamation mark key '!'.</para>
                    {UnityEngine.KeyCode.Exclaim,},*/

                    //   <para>Double quote key '"'.</para>
                    {UnityKeyCode.DoubleQuote, DnbKeyCode.Oem7},

                    /*
                    //   <para>Hash key '#'.</para>
                    {UnityEngine.KeyCode.Hash,},
                    
                    //   <para>Dollar sign key '$'.</para>
                    {UnityEngine.KeyCode.Dollar,},
                    
                    //   <para>Percent '%' key.</para>
                    {UnityEngine.KeyCode.Percent,},
                    
                    //   <para>Ampersand key '&amp;'.</para>
                    {UnityEngine.KeyCode.Ampersand,},*/

                    //   <para>Quote key '.</para>
                    {UnityKeyCode.Quote, DnbKeyCode.Oem7},

                    //   <para>Left Parenthesis key '('.</para>
                    {UnityKeyCode.LeftParen, DnbKeyCode.Vk9},

                    //   <para>Right Parenthesis key ')'.</para>
                    {UnityKeyCode.RightParen, DnbKeyCode.Vk0},
                    /*
                    //   <para>Asterisk key '*'.</para>
                    {UnityEngine.KeyCode.Asterisk,},*/

                    //   <para>Plus key '+'.</para>
                    {
                        UnityKeyCode.Plus,
                        DnbKeyCode.OemPlus
                    },

                    //   <para>Comma ',' key.</para>
                    {
                        UnityKeyCode.Comma,
                        DnbKeyCode.OemComma
                    },

                    //   <para>Minus '-' key.</para>
                    {
                        UnityKeyCode.Minus,
                        DnbKeyCode.OemMinus
                    },

                    //   <para>Period '.' key.</para>
                    {
                        UnityKeyCode.Period,
                        DnbKeyCode.OemPeriod
                    },

                    //   <para>Slash '/' key.</para>
                    {UnityKeyCode.Slash, DnbKeyCode.Oem5},

                    //   <para>The '0' key on the top of the alphanumeric keyboard.</para>
                    {
                        UnityKeyCode.Alpha0,
                        DnbKeyCode.Vk0
                    },

                    //   <para>The '1' key on the top of the alphanumeric keyboard.</para>
                    {
                        UnityKeyCode.Alpha1,
                        DnbKeyCode.Vk1
                    },

                    //   <para>The '2' key on the top of the alphanumeric keyboard.</para>
                    {
                        UnityKeyCode.Alpha2,
                        DnbKeyCode.Vk2
                    },

                    //   <para>The '3' key on the top of the alphanumeric keyboard.</para>
                    {
                        UnityKeyCode.Alpha3,
                        DnbKeyCode.Vk3
                    },

                    //   <para>The '4' key on the top of the alphanumeric keyboard.</para>
                    {
                        UnityKeyCode.Alpha4,
                        DnbKeyCode.Vk4
                    },

                    //   <para>The '5' key on the top of the alphanumeric keyboard.</para>
                    {
                        UnityKeyCode.Alpha5,
                        DnbKeyCode.Vk5
                    },

                    //   <para>The '6' key on the top of the alphanumeric keyboard.</para>
                    {
                        UnityKeyCode.Alpha6,
                        DnbKeyCode.Vk6
                    },

                    //   <para>The '7' key on the top of the alphanumeric keyboard.</para>
                    {
                        UnityKeyCode.Alpha7,
                        DnbKeyCode.Vk7
                    },

                    //   <para>The '8' key on the top of the alphanumeric keyboard.</para>
                    {
                        UnityKeyCode.Alpha8,
                        DnbKeyCode.Vk8
                    },

                    //   <para>The '9' key on the top of the alphanumeric keyboard.</para>
                    {
                        UnityKeyCode.Alpha9,
                        DnbKeyCode.Vk9
                    },


                    //   <para>Colon ':' key.</para>
                    {UnityKeyCode.Colon, DnbKeyCode.Oem1},

                    //   <para>Semicolon ';' key.</para>
                    {UnityKeyCode.Semicolon, DnbKeyCode.Oem1},

                    /*//   <para>Less than '&lt;' key.</para>
                    {UnityEngine.KeyCode.Less,},
                    
                    //   <para>Equals '=' key.</para>
                    {UnityEngine.KeyCode.Equals, },
                    
                    //   <para>Greater than '&gt;' key.</para>
                    {UnityEngine.KeyCode.Greater,},
                    
                    //   <para>Question mark '?' key.</para>
                    {UnityEngine.KeyCode.Question,},
                    
                    //   <para>At key '@'.</para>
                    {UnityEngine.KeyCode.At,},*/

                    //   <para>Left square bracket key '['.</para>
                    {UnityKeyCode.LeftBracket, DnbKeyCode.Oem2},

                    //   <para>Backslash key '\'.</para>
                    {UnityKeyCode.Backslash, DnbKeyCode.Oem5},

                    //   <para>Right square bracket key ']'.</para>
                    {UnityKeyCode.RightBracket, DnbKeyCode.Oem6},
                    /*
                    //   <para>Caret key '^'.</para>
                    {UnityEngine.KeyCode.Caret,},
                    
                    //   <para>Underscore '_' key.</para>
                    {UnityEngine.KeyCode.Underscore,},
                    
                    //   <para>Back quote key '`'.</para>
                    {UnityEngine.KeyCode.BackQuote,},*/

                    //   <para>'a' key.</para>
                    {
                        UnityKeyCode.A,
                        DnbKeyCode.VkA
                    },

                    //   <para>'b' key.</para>
                    {
                        UnityKeyCode.B,
                        DnbKeyCode.VkB
                    },

                    //   <para>'c' key.</para>
                    {
                        UnityKeyCode.C,
                        DnbKeyCode.VkC
                    },

                    //   <para>'d' key.</para>
                    {
                        UnityKeyCode.D,
                        DnbKeyCode.VkD
                    },

                    //   <para>'e' key.</para>
                    {
                        UnityKeyCode.E,
                        DnbKeyCode.VkE
                    },

                    //   <para>'f' key.</para>
                    {
                        UnityKeyCode.F,
                        DnbKeyCode.VkF
                    },

                    //   <para>'g' key.</para>
                    {
                        UnityKeyCode.G,
                        DnbKeyCode.VkG
                    },

                    //   <para>'h' key.</para>
                    {
                        UnityKeyCode.H,
                        DnbKeyCode.VkH
                    },

                    //   <para>'i' key.</para>
                    {
                        UnityKeyCode.I,
                        DnbKeyCode.VkI
                    },

                    //   <para>'j' key.</para>
                    {
                        UnityKeyCode.J,
                        DnbKeyCode.VkJ
                    },

                    //   <para>'k' key.</para>
                    {
                        UnityKeyCode.K,
                        DnbKeyCode.VkK
                    },

                    //   <para>'l' key.</para>
                    {
                        UnityKeyCode.L,
                        DnbKeyCode.VkL
                    },

                    //   <para>'m' key.</para>
                    {
                        UnityKeyCode.M,
                        DnbKeyCode.VkM
                    },

                    //   <para>'n' key.</para>
                    {
                        UnityKeyCode.N,
                        DnbKeyCode.VkN
                    },

                    //   <para>'o' key.</para>
                    {
                        UnityKeyCode.O,
                        DnbKeyCode.VkO
                    },

                    //   <para>'p' key.</para>
                    {
                        UnityKeyCode.P,
                        DnbKeyCode.VkP
                    },


                    //   <para>'q' key.</para>
                    {
                        UnityKeyCode.Q,
                        DnbKeyCode.VkQ
                    },

                    //   <para>'r' key.</para>
                    {
                        UnityKeyCode.R,
                        DnbKeyCode.VkR
                    },

                    //   <para>'s' key.</para>
                    {
                        UnityKeyCode.S,
                        DnbKeyCode.VkS
                    },

                    //   <para>'t' key.</para>
                    {
                        UnityKeyCode.T,
                        DnbKeyCode.VkT
                    },

                    //   <para>'u' key.</para>
                    {
                        UnityKeyCode.U,
                        DnbKeyCode.VkU
                    },

                    //   <para>'v' key.</para>
                    {
                        UnityKeyCode.V,
                        DnbKeyCode.VkV
                    },

                    //   <para>'w' key.</para>
                    {
                        UnityKeyCode.W,
                        DnbKeyCode.VkW
                    },

                    //   <para>'x' key.</para>
                    {
                        UnityKeyCode.X,
                        DnbKeyCode.VkX
                    },

                    //   <para>'y' key.</para>
                    {
                        UnityKeyCode.Y,
                        DnbKeyCode.VkY
                    },

                    //   <para>'z' key.</para>
                    {
                        UnityKeyCode.Z,
                        DnbKeyCode.VkZ
                    },

                    //   <para>Left curly bracket key '{'.</para>
                    {
                        UnityKeyCode.LeftCurlyBracket, DnbKeyCode.Oem2
                    },
                    /*
                    //   <para>Pipe '|' key.</para>
                    {
                        UnityEngine.KeyCode.Pipe,
                    },
                    
                    //   <para>Tilde '~' key.</para>
                    {
                        UnityEngine.KeyCode.Tilde,
                    },*/

                    //   <para>Right curly bracket key '},'.</para>
                    {
                        UnityKeyCode.RightCurlyBracket, DnbKeyCode.Oem6
                    },

                    //   <para>The forward delete key.</para>
                    {
                        UnityKeyCode.Delete,
                        DnbKeyCode.Delete
                    },

                    //   <para>Numeric keypad 0.</para>
                    {
                        UnityKeyCode.Keypad0,
                        DnbKeyCode.Numpad0
                    },

                    //   <para>Numeric keypad 1.</para>
                    {
                        UnityKeyCode.Keypad1,
                        DnbKeyCode.Numpad1
                    },

                    //   <para>Numeric keypad 2.</para>
                    {
                        UnityKeyCode.Keypad2,
                        DnbKeyCode.Numpad2
                    },

                    //   <para>Numeric keypad 3.</para>
                    {
                        UnityKeyCode.Keypad3,
                        DnbKeyCode.Numpad3
                    },

                    //   <para>Numeric keypad 4.</para>
                    {
                        UnityKeyCode.Keypad4,
                        DnbKeyCode.Numpad4
                    },

                    //   <para>Numeric keypad 5.</para>
                    {
                        UnityKeyCode.Keypad5,
                        DnbKeyCode.Numpad5
                    },

                    //   <para>Numeric keypad 6.</para>
                    {
                        UnityKeyCode.Keypad6,
                        DnbKeyCode.Numpad6
                    },

                    //   <para>Numeric keypad 7.</para>
                    {
                        UnityKeyCode.Keypad7,
                        DnbKeyCode.Numpad7
                    },

                    //   <para>Numeric keypad 8.</para>
                    {
                        UnityKeyCode.Keypad8,
                        DnbKeyCode.Numpad8
                    },

                    //   <para>Numeric keypad 9.</para>
                    {
                        UnityKeyCode.Keypad9,
                        DnbKeyCode.Numpad9
                    },

                    //   <para>Numeric keypad '.'.</para>
                    {
                        UnityKeyCode.KeypadPeriod,
                        DnbKeyCode.Decimal
                    },

                    //   <para>Numeric keypad '/'.</para>
                    {
                        UnityKeyCode.KeypadDivide,
                        DnbKeyCode.Divide
                    },

                    //   <para>Numeric keypad '*'.</para>
                    {
                        UnityKeyCode.KeypadMultiply,
                        DnbKeyCode.Multiply
                    },

                    //   <para>Numeric keypad '-'.</para>
                    {
                        UnityKeyCode.KeypadMinus,
                        DnbKeyCode.Subtract
                    },

                    //   <para>Numeric keypad '+'.</para>
                    {
                        UnityKeyCode.KeypadPlus,
                        DnbKeyCode.Add
                    },
                    /*
                    //   <para>Numeric keypad Enter.</para>
                    {
                        UnityEngine.KeyCode.KeypadEnter,
                    },
                    
                    //   <para>Numeric keypad '='.</para>
                    {
                        UnityEngine.KeyCode.KeypadEquals,
                    },*/

                    //   <para>Up arrow key.</para>
                    {
                        UnityKeyCode.UpArrow,
                        DnbKeyCode.Up
                    },

                    //   <para>Down arrow key.</para>
                    {
                        UnityKeyCode.DownArrow,
                        DnbKeyCode.Down
                    },

                    //   <para>Right arrow key.</para>
                    {
                        UnityKeyCode.RightArrow,
                        DnbKeyCode.Right
                    },

                    //   <para>Left arrow key.</para>
                    {
                        UnityKeyCode.LeftArrow,
                        DnbKeyCode.Left
                    },

                    //   <para>Insert key key.</para>
                    {
                        UnityKeyCode.Insert, DnbKeyCode.Insert
                    },

                    //   <para>Home key.</para>
                    {
                        UnityKeyCode.Home, DnbKeyCode.Home
                    },

                    //   <para>End key.</para>
                    {
                        UnityKeyCode.End, DnbKeyCode.End
                    },

                    //   <para>Page up.</para>
                    {
                        UnityKeyCode.PageUp, DnbKeyCode.Prior
                    },

                    //   <para>Page down.</para>
                    {
                        UnityKeyCode.PageDown, DnbKeyCode.Next
                    },

                    //   <para>F1 function key.</para>
                    {
                        UnityKeyCode.F1,
                        DnbKeyCode.F1
                    },

                    //   <para>F2 function key.</para>
                    {
                        UnityKeyCode.F2,
                        DnbKeyCode.F2
                    },

                    //   <para>F3 function key.</para>
                    {
                        UnityKeyCode.F3,
                        DnbKeyCode.F3
                    },

                    //   <para>F4 function key.</para>
                    {
                        UnityKeyCode.F4,
                        DnbKeyCode.F4
                    },

                    //   <para>F5 function key.</para>
                    {
                        UnityKeyCode.F5,
                        DnbKeyCode.F5
                    },

                    //   <para>F6 function key.</para>
                    {
                        UnityKeyCode.F6,
                        DnbKeyCode.F6
                    },

                    //   <para>F7 function key.</para>
                    {
                        UnityKeyCode.F7,
                        DnbKeyCode.F7
                    },

                    //   <para>F8 function key.</para>
                    {
                        UnityKeyCode.F8,
                        DnbKeyCode.F8
                    },

                    //   <para>F9 function key.</para>
                    {
                        UnityKeyCode.F9,
                        DnbKeyCode.F9
                    },

                    //   <para>F10 function key.</para>
                    {
                        UnityKeyCode.F10,
                        DnbKeyCode.F10
                    },

                    //   <para>F11 function key.</para>
                    {
                        UnityKeyCode.F11,
                        DnbKeyCode.F11
                    },

                    //   <para>F12 function key.</para>
                    {
                        UnityKeyCode.F12,
                        DnbKeyCode.F12
                    },

                    //   <para>F13 function key.</para>
                    {
                        UnityKeyCode.F13,
                        DnbKeyCode.F13
                    },

                    //   <para>F14 function key.</para>
                    {
                        UnityKeyCode.F14,
                        DnbKeyCode.F14
                    },

                    //   <para>F15 function key.</para>
                    {
                        UnityKeyCode.F15,
                        DnbKeyCode.F15
                    },

                    //   <para>Numlock key.</para>
                    {
                        UnityKeyCode.Numlock, DnbKeyCode.Numlock
                    },

                    //   <para>Capslock key.</para>
                    {
                        UnityKeyCode.CapsLock, DnbKeyCode.Capital
                    },

                    //   <para>Scroll lock key.</para>
                    {
                        UnityKeyCode.ScrollLock, DnbKeyCode.Scroll
                    },

                    //   <para>Right shift key.</para>
                    {
                        UnityKeyCode.RightShift,
                        DnbKeyCode.RShift
                    },

                    //   <para>Left shift key.</para>
                    {
                        UnityKeyCode.LeftShift,
                        DnbKeyCode.LShift
                    },

                    //   <para>Right Control key.</para>
                    {
                        UnityKeyCode.RightControl,
                        DnbKeyCode.RControl
                    },

                    //   <para>Left Control key.</para>
                    {
                        UnityKeyCode.LeftControl,
                        DnbKeyCode.LControl
                    },

                    //   <para>Right Alt key.</para>
                    {
                        UnityKeyCode.RightAlt,
                        DnbKeyCode.RMenu
                    },

                    //   <para>Left Alt key.</para>
                    {
                        UnityKeyCode.LeftAlt,
                        DnbKeyCode.LMenu
                    },

                    //   <para>Right Command key.</para>
                    {
                        UnityKeyCode.RightCommand,
                        DnbKeyCode.RCmd
                    },

                    //   <para>Left Command key.</para>
                    {
                        UnityKeyCode.LeftCommand,
                        DnbKeyCode.LCmd
                    },

                    //   <para>Left Windows key.</para>
                    {
                        UnityKeyCode.LeftWindows,
                        DnbKeyCode.LWin
                    },

                    //   <para>Right Windows key.</para>
                    {
                        UnityKeyCode.RightWindows,
                        DnbKeyCode.RWin
                    },

                    //   <para>Alt Gr key.</para>
                    {
                        UnityKeyCode.AltGr, DnbKeyCode.RMenu
                    },

                    //   <para>Help key.</para>
                    {
                        UnityKeyCode.Help, DnbKeyCode.Help
                    },

                    //   <para>Print key.</para>
                    {
                        UnityKeyCode.Print, DnbKeyCode.Print
                    },

                    //   <para>Sys Req key.</para>
                    {
                        UnityKeyCode.SysReq, DnbKeyCode.Snapshot
                    },

                    //   <para>Break key.</para>
                    {
                        UnityKeyCode.Break, DnbKeyCode.Pause
                    },

                    //   <para>Menu key.</para>
                    {
                        UnityKeyCode.Menu, DnbKeyCode.Menu
                    },

                    //   <para>The Left (or primary) mouse button.</para>
                    {
                        UnityKeyCode.Mouse0, DnbKeyCode.LButton
                    },

                    //   <para>Right mouse button (or secondary mouse button).</para>
                    {
                        UnityKeyCode.Mouse1, DnbKeyCode.RButton
                    },

                    //   <para>Middle mouse button (or third button).</para>
                    {
                        UnityKeyCode.Mouse2, DnbKeyCode.MButton
                    },

                    //   <para>Additional (fourth) mouse button.</para>
                    {
                        UnityKeyCode.Mouse3, DnbKeyCode.XButton1
                    },

                    //   <para>Additional (fifth) mouse button.</para>
                    {
                        UnityKeyCode.Mouse4, DnbKeyCode.XButton2
                    }
                };

        #endregion

        public KeyboardHelper(IKeyboard keyboard)
        {
            this.keyboard = keyboard;
            GUI.enabled = true;
        }

        public void HandleKeyboardEvents()
        {
            Event e = Event.current;
            if (!(e?.isKey ?? false))
            {
                return;
            }

            Debug.Log($"Event.current. keyCode : {e.keyCode}, type : {e.rawType}, char : {e.character};");
            switch (e.rawType)
            {
                case EventType.KeyDown:
                    OnKeyDown(e.keyCode, e.character, e.shift, e.control, e.alt);
                    break;
                case EventType.KeyUp:
                    OnKeyUp(e.keyCode, e.shift, e.control, e.alt);
                    break;
            }
        }

        private static KeyModifiers GetKeyModifiers(bool shiftDown, bool controlDown,
                                                    bool altDown) => new KeyModifiers
        {
            AltDown = altDown,
            ControlDown = controlDown,
            ShiftDown = shiftDown
        };

        private static bool IsTyped(UnityKeyCode key, char keyChar) => char.IsControl(keyChar)
            || char.IsLetterOrDigit(keyChar)
            || char.IsSymbol(keyChar)
            || char.IsNumber(keyChar)
            || char.IsPunctuation(keyChar)
            || key == UnityKeyCode.Space
            || key == UnityKeyCode.Return
            || key == UnityKeyCode.None;

        private void OnKeyDown(UnityKeyCode keyCode, char ch, bool shiftDown,
                               bool controlDown,
                               bool altDown)
        {
            if (keyCodesMapping.TryGetValue(keyCode, out DnbKeyCode code))
            {
                KeyModifiers modifiers = GetKeyModifiers(shiftDown, controlDown, altDown);
                KeyPressedEventArgs keyDownEventArgs = new KeyPressedEventArgs
                {
                    KeyChar = ch.ToString(),
                    VirtualKey = code,
                    Modifiers = modifiers
                };
                keyboard.KeyPressed.Raise(keyDownEventArgs);

                if (IsTyped(keyCode, ch))
                {
                    KeyTypedEventArgs keyPressEventArgs = new KeyTypedEventArgs
                    {
                        KeyChar = ch.ToString(),
                        VirtualKey = code,
                        Modifiers = modifiers
                    };
                    keyboard.KeyTyped.Raise(keyPressEventArgs);
                }
            }
        }

        private void OnKeyUp(UnityKeyCode keyCode, bool shiftDown, bool controlDown,
                             bool altDown)
        {
            if (keyCodesMapping.TryGetValue(keyCode, out DnbKeyCode code))
            {
                KeyReleasedEventArgs keyUpEventArgs = new KeyReleasedEventArgs
                {
                    VirtualKey = code,
                    Modifiers = GetKeyModifiers(shiftDown, controlDown, altDown)
                };
                keyboard.KeyReleased.Raise(keyUpEventArgs);
            }
        }
    }
}
