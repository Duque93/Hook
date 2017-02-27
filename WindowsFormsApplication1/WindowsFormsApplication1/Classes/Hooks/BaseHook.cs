using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Hook
{
    public abstract class BaseHook
    {
        #region Structs

        //LPARAM --> IntPTr
        //WPARAM -->UIntrPtr
        //ULONG_PTR --> IntPtr
        //DWORD --> uint
        //LONG --> int
        //WORD --> ushort
        //https://msdn.microsoft.com/es-es/library/windows/desktop/ms646270(v=vs.85).aspx
        [StructLayout(LayoutKind.Sequential)]
        internal struct INPUT
        {
            public uint tipo; //0 --> MOUSENINPUT, 1 --> KEYBDINPUT, 2 --> HARDWAREINPUT
            public DataInput data;
        }
        [StructLayout(LayoutKind.Explicit)]
        internal struct DataInput
        {
            [FieldOffset(0)]
            public MOUSEINPUT mouse;
            [FieldOffset(0)]
            public KEYBDINPUT keyboard;
            [FieldOffset(0)]
            public HARDWAREINPUT hardware;
        }

        //https://msdn.microsoft.com/es-es/library/windows/desktop/ms646273(v=vs.85).aspx
        [StructLayout(LayoutKind.Sequential)]
        internal struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }
        //https://msdn.microsoft.com/es-es/library/windows/desktop/ms646271(v=vs.85).aspx
        [StructLayout(LayoutKind.Sequential)]
        internal struct KEYBDINPUT
        {
            public ushort codeVirtualKey;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }
        //https://msdn.microsoft.com/es-es/library/windows/desktop/ms646269(v=vs.85).aspx
        [StructLayout(LayoutKind.Sequential)]
        internal struct HARDWAREINPUT
        {
            public uint uMsg;
            public ushort wParamL;
            public ushort wParamH;
        }

        #endregion

        #region Enums
        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/windows/desktop/ms644990(v=vs.85).aspx
        /// </summary>
        internal enum HookType : int
        {
            WH_MSGFILTER = -1,
            WH_JOURNALRECORD = 0,
            WH_JOURNALPLAYBACK = 1,
            /// <summary>
            /// BaseHook que capturara todas las teclas del teclado pulsadas por el usuario en la ventana DE UN PROCESO
            /// </summary>
            WH_KEYBOARD = 2,
            WH_GETMESSAGE = 3,
            /// <summary>
            /// Installs a hook procedure that monitors messages before the system sends them to the destination window procedure. For more information, see the CallWndProc hook procedure.
            /// </summary>
            WH_CALLWNDPROC = 4,
            WH_CBT = 5,
            WH_SYSMSGFILTER = 6,
            /// <summary>
            /// BaseHook que capturara todos los botones y movimientos del raton ejecutados por el usuario
            /// en la ventana dde un PROCESO
            /// </summary>
            WH_MOUSE = 7,
            WH_HARDWARE = 8,
            WH_DEBUG = 9,
            WH_SHELL = 10,
            WH_FOREGROUNDIDLE = 11,
            /// <summary>
            /// Installs a hook procedure that monitors messages after they have been processed by the destination window procedure. For more information, see the CallWndRetProc hook procedure.
            /// </summary>
            WH_CALLWNDPROCRET = 12,
            /// <summary>
            /// BaseHook que capturara todos las teclas del teclado pulsadas por el usuario. Necesario llamar a 
            /// CallNextHookEx para propagar el evento y permitir a otras apliciones seguir capturando el evento
            /// </summary>
            WH_KEYBOARD_LL = 13,
            /// <summary>
            /// BaseHook que capturara las pulsaciones y movimientos del raton realizadas por el usuario.Necesario llamar a 
            /// CallNextHookEx para propagar el evento y permitir a otras apliciones seguir capturando el evento
            /// </summary>
            WH_MOUSE_LL = 14,
        }
        #endregion Enums

        #region Propiedades

        internal delegate IntPtr hookHandler(int nCode, IntPtr wParam, IntPtr lParam); //Un delegate es un puntero pero que apunta a una función
        internal hookHandler childProcedure = null;
        private IntPtr hookId = IntPtr.Zero; // La dirección de memoria que apunta al hook creado por SetWindowsHookEx

        private HookType typeHook;
        private bool destroyed;
        
        #endregion Propiedades

        #region DLLImports

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)] //Establece la función que sera llamada para cada evento de tipoHook
        private static extern IntPtr SetWindowsHookEx(int tipoHook, hookHandler lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)] //Permite borrar del SO el hook inyectado(el cual hemos creado) para ahorrarnos futuros disgustos
        private static extern bool UnhookWindowsHookEx(IntPtr hookId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)] //Permite propagar el evento
        internal static extern IntPtr CallNextHookEx(IntPtr hookId, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern IntPtr GetModuleHandle(string lpModuleName);

        // https://msdn.microsoft.com/es-es/library/windows/desktop/ms646310(v=vs.85).aspx
        [DllImport("user32.dll", SetLastError = true)] //Permite mandar nuestros propios eventos
        internal static extern uint SendInput(uint numberOfInputs, [MarshalAs(UnmanagedType.LPArray), In] INPUT[] pInputs, int sizeOfInputStructure);

        //MAS INFO --> http://stackoverflow.com/questions/12761169/send-keys-through-sendinput-in-user32-dll
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern uint SendInput(uint nInputs, ref INPUT pInputs, int cbSize);

        #endregion DLLImports

        internal BaseHook(HookType typeHook)
        {
            this.typeHook = typeHook;
        }

        internal void init(IntPtr hwnd, uint processID)
        {
            destroyed = false;
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                if(hwnd == IntPtr.Zero && processID == 0) hookId = SetHook(GetModuleHandle(curModule.ModuleName) , 0);
                else hookId = SetHook(hwnd, processID);

                if(hookId == IntPtr.Zero)
                {
                    int err = Marshal.GetLastWin32Error();
                    if (err != 0)
                        MessageBox.Show("La creación del hook ha fallado. Codigo error = "+err);
                }
            }
                
        }

        public void deleteHook()
        {
            destroyed = UnhookWindowsHookEx(hookId);
        }

        ~BaseHook() //DESTRUCTOR
        {
            if (!destroyed)
            {
                deleteHook();
            }

        }        
        private IntPtr SetHook(IntPtr hwnd, uint processId)
        {
            Trace.WriteLine("Creando hook de tipo -->"+typeHook);
             if (childProcedure == null) return (IntPtr)1;
             return SetWindowsHookEx(Convert.ToInt32(typeHook), childProcedure, hwnd, processId);
        }

        internal IntPtr getHookID()
        {
            return this.hookId;
        }
    }
}
