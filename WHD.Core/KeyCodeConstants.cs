using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace WHD.Core
{
    //https://msdn.microsoft.com/en-us/library/ee199372.aspx?f=255&MSPPError=-2147217396
    public enum KeyCodeSymbol
    {
        [Description(";:")] SemiColon = 186,
        [Description("=+")] Equal = 187,
        [Description(",<")] Comma = 188,
        [Description("-_")] Dash = 189,
        [Description(".>")] Period = 190,
        [Description("/?")] ForwardSlash = 191,
        [Description("`~")] GraveAccent = 192,
        [Description("[{")] OpenBracket = 219,
        [Description("\\|")] BackSlash = 220,
        [Description("]}")] CloseBraket = 221,
        [Description("'\"")] SingleQuote = 222,

    }
    public enum KeyCodeAZ09 : byte
    {
        [Description("A key")] vbKeyA = 65,
        [Description("B key")] vbKeyB = 66,
        [Description("C key")] vbKeyC = 67,
        [Description("D key")] vbKeyD = 68,
        [Description("E key")] vbKeyE = 69,
        [Description("F key")] vbKeyF = 70,
        [Description("G key")] vbKeyG = 71,
        [Description("H key")] vbKeyH = 72,
        [Description("I key")] vbKeyI = 73,
        [Description("J key")] vbKeyJ = 74,
        [Description("K key")] vbKeyK = 75,
        [Description("L key")] vbKeyL = 76,
        [Description("M key")] vbKeyM = 77,
        [Description("N key")] vbKeyN = 78,
        [Description("O key")] vbKeyO = 79,
        [Description("P key")] vbKeyP = 80,
        [Description("Q key")] vbKeyQ = 81,
        [Description("R key")] vbKeyR = 82,
        [Description("S key")] vbKeyS = 83,
        [Description("T key")] vbKeyT = 84,
        [Description("U key")] vbKeyU = 85,
        [Description("V key")] vbKeyV = 86,
        [Description("W key")] vbKeyW = 87,
        [Description("X key")] vbKeyX = 88,
        [Description("Y key")] vbKeyY = 89,
        [Description("Z key")] vbKeyZ = 90,
        [Description("0 key")] vbKey0 = 48,
        [Description("1 key")] vbKey1 = 49,
        [Description("2 key")] vbKey2 = 50,
        [Description("3 key")] vbKey3 = 51,
        [Description("4 key")] vbKey4 = 52,
        [Description("5 key")] vbKey5 = 53,
        [Description("6 key")] vbKey6 = 54,
        [Description("7 key")] vbKey7 = 55,
        [Description("8 key")] vbKey8 = 56,
        [Description("9 key")] vbKey9 = 57,
    }

    public enum KeyCodeMouse : byte
    {
        [Description("Left mouse button")] vbKeyLButton = 1,
        [Description("Right mouse button")] vbKeyRButton = 2,
        [Description("CANCEL key")] vbKeyCancel = 3,
        [Description("Middle mouse button")] vbKeyMButton = 4,
    }
    public enum KeyCodeNumPad : byte
    {
        [Description("NUM LOCK key")] vbKeyNumlock = 144,
        [Description("Numpad 0 key")] vbKeyNumpad0 = 96,
        [Description("Numpad 1 key")] vbKeyNumpad1 = 97,
        [Description("Numpad 2 key")] vbKeyNumpad2 = 98,
        [Description("Numpad 3 key")] vbKeyNumpad3 = 99,
        [Description("Numpad 4 key")] vbKeyNumpad4 = 100,
        [Description("Numpad 5 key")] vbKeyNumpad5 = 101,
        [Description("Numpad 6 key")] vbKeyNumpad6 = 102,
        [Description("Numpad 7 key")] vbKeyNumpad7 = 103,
        [Description("Numpad 8 key")] vbKeyNumpad8 = 104,
        [Description("Numpad 9 key")] vbKeyNumpad9 = 105,
        [Description("Numpad MULTIPLICATION SIGN ( * ) key")] vbKeyMultiply = 106,
        [Description("Numpad PLUS SIGN ( + ) key")] vbKeyAdd = 107,
        [Description("Numpad ENTER (keypad) key")] vbKeySeparator = 108,
        [Description("Numpad MINUS SIGN ( - ) key")] vbKeySubtract = 109,
        [Description("Numpad DECIMAL POINT(.) key")] vbKeyDecimal = 110,
        [Description("Numpad DIVISION SIGN ( / ) key")] vbKeyDivide = 111,
    }

    public enum KeyCodeControl : byte
    {
        [Description("BACKSPACE key")] vbKeyBack = 8,
        [Description("TAB key")] vbKeyTab = 9,
        [Description("CLEAR key")] vbKeyClear = 12,
        [Description("ENTER key")] vbKeyReturn = 13,
        [Description("SHIFT key")] vbKeyShift = 16,
        [Description("CTRL key")] vbKeyControl = 17,
        [Description("MENU key")] vbKeyMenu = 18,
        [Description("PAUSE key")] vbKeyPause = 19,
        [Description("CAPS LOCK key")] vbKeyCapital = 20,
        [Description("ESC key")] vbKeyEscape = 27,
        [Description("SPACEBAR key")] vbKeySpace = 32,
        [Description("PAGE UP key")] vbKeyPageUp = 33,
        [Description("PAGE DOWN key")] vbKeyPageDown = 34,
        [Description("END key")] vbKeyEnd = 35,
        [Description("HOME key")] vbKeyHome = 36,
        [Description("LEFT ARROW key")] vbKeyLeft = 37,
        [Description("UP ARROW key")] vbKeyUp = 38,
        [Description("RIGHT ARROW key")] vbKeyRight = 39,
        [Description("DOWN ARROW key")] vbKeyDown = 40,
        [Description("SELECT key")] vbKeySelect = 41,
        [Description("PRINT SCREEN key")] vbKeyPrint = 42,
        [Description("EXECUTE key")] vbKeyExecute = 43,
        [Description("SNAPSHOT key")] vbKeySnapshot = 44,
        [Description("INS key")] vbKeyInsert = 45,
        [Description("DEL key")] vbKeyDelete = 46,
        [Description("HELP key")] vbKeyHelp = 47,
    }
    public enum KeyCodeFunction : byte
    {
        [Description("F1 key")] vbKeyF1 = 112,
        [Description("F2 key")] vbKeyF2 = 113,
        [Description("F3 key")] vbKeyF3 = 114,
        [Description("F4 key")] vbKeyF4 = 115,
        [Description("F5 key")] vbKeyF5 = 116,
        [Description("F6 key")] vbKeyF6 = 117,
        [Description("F7 key")] vbKeyF7 = 118,
        [Description("F8 key")] vbKeyF8 = 119,
        [Description("F9 key")] vbKeyF9 = 120,
        [Description("F10 key")] vbKeyF10 = 121,
        [Description("F11 key")] vbKeyF11 = 122,
        [Description("F12 key")] vbKeyF12 = 123,
        [Description("F13 key")] vbKeyF13 = 124,
        [Description("F14 key")] vbKeyF14 = 125,
        [Description("F15 key")] vbKeyF15 = 126,
        [Description("F16 key")] vbKeyF16 = 127,
    }
}
