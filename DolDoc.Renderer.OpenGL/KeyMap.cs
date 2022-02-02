using DolDoc.Editor.Core;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;

namespace DolDoc.Renderer.OpenGL
{
    internal static class KeyMap
    {
        internal static Dictionary<Keys, Key> Map = new Dictionary<Keys, Key>
        {
            { Keys.Up, Key.ARROW_UP },
            { Keys.Left, Key.ARROW_LEFT },
            { Keys.Down, Key.ARROW_DOWN },
            { Keys.Right, Key.ARROW_RIGHT },
            { Keys.Backspace, Key.BACKSPACE },
            { Keys.Delete, Key.DEL },
            { Keys.Home, Key.HOME },
            { Keys.PageUp, Key.PAGE_UP },
            { Keys.PageDown, Key.PAGE_DOWN },
            { Keys.Space, Key.SPACE },
            { Keys.Period, Key.DOT },
            { Keys.A, Key.A_LOWER },
            { Keys.B, Key.B_LOWER },
            { Keys.C, Key.C_LOWER },
            { Keys.D, Key.D_LOWER },
            { Keys.E, Key.E_LOWER },
            { Keys.F, Key.F_LOWER },
            { Keys.G, Key.G_LOWER },
            { Keys.H, Key.H_LOWER },
            { Keys.I, Key.I_LOWER },
            { Keys.J, Key.J_LOWER },
            { Keys.K, Key.K_LOWER },
            { Keys.L, Key.L_LOWER },
            { Keys.M, Key.M_LOWER },
            { Keys.N, Key.N_LOWER },
            { Keys.O, Key.O_LOWER },
            { Keys.P, Key.P_LOWER },
            { Keys.Q, Key.Q_LOWER },
            { Keys.S, Key.S_LOWER },
            { Keys.R, Key.R_LOWER },
            { Keys.T, Key.T_LOWER },
            { Keys.U, Key.U_LOWER },
            { Keys.V, Key.V_LOWER },
            { Keys.W, Key.W_LOWER },
            { Keys.X, Key.X_LOWER },
            { Keys.Y, Key.Y_LOWER },
            { Keys.Z, Key.Z_LOWER },
            { Keys.Slash, Key.SLASH_FOWARD },
            { Keys.Backslash, Key.SLASH_BACKWARD },
            { Keys.D1, Key.NUMBER_1 },
            { Keys.D2, Key.NUMBER_2 },
            { Keys.D3, Key.NUMBER_3 },
            { Keys.D4, Key.NUMBER_4 },
            { Keys.D5, Key.NUMBER_5 },
            { Keys.D6, Key.NUMBER_6 },
            { Keys.D7, Key.NUMBER_7 },
            { Keys.D8, Key.NUMBER_8 },
            { Keys.D9, Key.NUMBER_9 },
            { Keys.D0, Key.NUMBER_0 },
            { Keys.Apostrophe, Key.QUOTATION_MARK_SINGLE },
            { Keys.Equal, Key.EQUAL_TO },
            { Keys.Comma, Key.COMMA },
            { Keys.Enter, Key.ENTER },
            { Keys.Escape, Key.ESC },
            { Keys.F12, Key.F12 }
        };

        internal static Dictionary<Keys, Key> ShiftMap = new Dictionary<Keys, Key>
        {
            { Keys.A, Key.A_UPPER },
            { Keys.B, Key.B_UPPER },
            { Keys.C, Key.C_UPPER },
            { Keys.D, Key.D_UPPER },
            { Keys.E, Key.E_UPPER },
            { Keys.F, Key.F_UPPER },
            { Keys.G, Key.G_UPPER },
            { Keys.H, Key.H_UPPER },
            { Keys.I, Key.I_UPPER },
            { Keys.J, Key.J_UPPER },
            { Keys.K, Key.K_UPPER },
            { Keys.L, Key.L_UPPER },
            { Keys.M, Key.M_UPPER },
            { Keys.N, Key.N_UPPER },
            { Keys.O, Key.O_UPPER },
            { Keys.P, Key.P_UPPER },
            { Keys.Q, Key.Q_UPPER },
            { Keys.R, Key.R_UPPER },
            { Keys.S, Key.S_UPPER },
            { Keys.T, Key.T_UPPER },
            { Keys.U, Key.U_UPPER },
            { Keys.V, Key.V_UPPER },
            { Keys.W, Key.W_UPPER },
            { Keys.X, Key.X_UPPER },
            { Keys.Y, Key.Y_UPPER },
            { Keys.Z, Key.Z_UPPER },
            { Keys.D1, Key.EXCLAMAION_MARK },
            { Keys.D2, Key.SING_AT },
            { Keys.D3, Key.HASHTAG },
            { Keys.D4, Key.SING_DOLLAR },
            { Keys.D5, Key.PRECENT },
            { Keys.D6, Key.CARET },
            { Keys.D7, Key.AMPERSANT },
            { Keys.D8, Key.ASTERISK },
            { Keys.D9, Key.PARENTHESES_OPEN },
            { Keys.D0, Key.PARENTHESES_CLOSE },
            { Keys.Apostrophe, Key.QUOTATION_MARK_DOUBLE },
            { Keys.Equal, Key.PLUS},
            { Keys.Comma, Key.LESS_THAN },
            { Keys.Escape, Key.SHIFT_ESC }
        };

        internal static Key? GetKey(KeyboardKeyEventArgs e)
        {
            Key key;
            if (e.Shift)
            {
                if (!ShiftMap.TryGetValue(e.Key, out key))
                    return null;
            }
            else
            {
                if (!Map.TryGetValue(e.Key, out key))
                    return null;
            }

            return key;
        }
    }
}
