using Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Hook
{
    //https://msdn.microsoft.com/es-es/library/windows/desktop/ms644985(v=vs.85).aspx
    class KeyBoardHook : BaseHook
    {

        private enum KeyBoardEventType : int
        { //USar valores hexadecimales pues es mas improbable que estos valores se tengan que cambiar en cada cambio de version del SO Windows
            NONE = 0,
            WM_KEYDOWN = 0x0100, //256,
            WM_KEYUP =  0x0101, //257,
            WM_SYSKEYDOWN = 0x0104, //260, //Este evento solo ocurre cuando las teclas ctrl, fx(F1,F2,..), shift or alt son presionadas.
            WM_SYSKEYUP = 0x0105, //261,
        }

        private const int TIPO_KEYBOARDEVENT = 1;
        private IKeyBoardEventsListener manejadorEventos;
        private static KeyBoardHook instance;

        public static KeyBoardHook getInstance(IKeyBoardEventsListener manejadorEventos)
        {
            if (instance == null) instance = new KeyBoardHook(manejadorEventos);
            return instance;
        }

        private KeyBoardHook(IKeyBoardEventsListener manejadorEventos) : base(HookType.WH_KEYBOARD_LL)
        {
            this.manejadorEventos = manejadorEventos;
            base.childProcedure = new hookHandler(hookCallback);
            base.init(IntPtr.Zero, 0);
        }

        private static IntPtr hookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if(instance == null) return (IntPtr)1;

            bool result = true;
            if (nCode >= 0 && instance.childProcedure != null)
            {
                int param = wParam.ToInt32();
                KeyBoardEventType tipoEvento = Enum.IsDefined(typeof(KeyBoardEventType), param) ? (KeyBoardEventType)Enum.ToObject(typeof(KeyBoardEventType), param) : KeyBoardEventType.NONE;
                if (instance.manejadorEventos != null && !tipoEvento.Equals(KeyBoardEventType.NONE))
                {
                    int vkCode = Marshal.ReadInt32(lParam);
                    switch (tipoEvento)
                    {
                        case KeyBoardEventType.WM_KEYDOWN:
                        case KeyBoardEventType.WM_SYSKEYDOWN:
                            result = instance.manejadorEventos.onKeyDown(instance, vkCode, (Keys)vkCode);
                            break;
                        case KeyBoardEventType.WM_KEYUP:
                        case KeyBoardEventType.WM_SYSKEYUP:
                            result = instance.manejadorEventos.onKeyUp(instance, vkCode, (Keys)vkCode);
                            break;
                    }
                }
            }
            if (result) return CallNextHookEx(instance.getHookID(), nCode, wParam, lParam); //Permite seguir propagando el evento a otras aplicaciones
            else return (IntPtr)1; //Bloquea la propagación del evento a otras aplicaciones
        }
        //FIN METODO hookCallback

        public bool replaceKey(Keys newKey)
        {
            List<INPUT> keyList = new List<INPUT>();
        
            INPUT keyDown = new INPUT();
            keyDown.tipo = TIPO_KEYBOARDEVENT; // Keyboard
            keyDown.data.keyboard.codeVirtualKey = (ushort)newKey;
            keyDown.data.keyboard.dwFlags = (int)KeyBoardEventType.WM_KEYDOWN;
            keyDown.data.keyboard.wScan = (ushort)newKey;
            keyList.Add(keyDown);
            
            /*
            INPUT keyUp = new INPUT();
            keyUp.tipo = 1; //Keyboard
            keyUp.data.keyboard.codeVirtualKey = (ushort)newKey;
            keyUp.data.keyboard.dwFlags = (int)KeyBoardEventType.WM_KEYUP | (int)KeyBoardEventType.WM_SYSKEYUP;
            keyUp.data.keyboard.wScan = (ushort)newKey;
            keyList.Add(keyUp);
            */

            if ( SendInput((uint)keyList.Count, keyList.ToArray(), Marshal.SizeOf(typeof(INPUT))) == 0)
            {
                MessageBox.Show("Ha fallado el remplazo del evento en KeyBoardHook");
            }

            return false;
        }
        
    }
}
