
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Utils;
using static Structs.Commons;
using static WindowsFormsApplication1.Classes.Utils.Utilidades;

namespace Hook
{

    class WindowHook : BaseHook
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi)]
        static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)]string lpFileName);


        //https://msdn.microsoft.com/es-es/library/windows/desktop/ff468922(v=vs.85).aspx
        private enum WindowEventType
        {
            /// <summary>
            /// Enviado cuando un hijo esta siendo creado o eliminado
            /// wParam = TRUE si esta siendo creado , FALSE si esta siendo eliminado
            /// lParam = si wParam == TRUE id del thread del hijo || si wParam == FALSE id del thread del padre
            /// Debe devolver cero el hookCallback para este evento
            /// </summary>
            WM_ACTIVATEAPP = 0x001C,
            /// <summary>
            /// Enviado cuando la ventana target esta ignorando cualquier evento de teclado o raton (ej. --> enabled = false)
            /// Devolver 0;
            /// </summary>
            WM_CANCELMODE = 0x001F,
            /// <summary>
            /// Enviado cuando una ventana hija recibe un click y recoge el foco
            /// Devolver 0;
            /// </summary>
            WM_CHILDACTIVATE = 0x0022,
            /// <summary>
            /// Enviado cuando se procede a cerrar la ventana
            /// No usar como indicador de que la ventana se va a cerrar, pues un mensaje de confirmación negativo puede anular el proceso
            /// Devolver 0
            /// </summary>
            WM_CLOSE= 0x0010,
            /// <summary>
            /// Solo enviado para sistemas de 16 bits.
            /// Enviado cuando más del 12.5% del sistema es usado en un intervalo de 30-60 segundos
            /// wParam = % de CPU siendo utilizada (ej --> 0x8000(32768) = 50% CPU)
            /// Devolver 0.
            /// </summary>
            WM_COMPACTING = 0x0041,
            /// <summary>
            /// Enviado cuando la aplicación manda a crear una ventana llamando a las funciones CreateWindowEx o CreateWindow 
            /// lParam = *CREATESTRUCT
            /// Devolver -1 para evitar que se cree la ventana, 0 para funcionamiento normal
            /// </summary>
            WM_CREATE = 0x0001,
            /// <summary>
            /// Enviado cuando la ventana visual desaparece de la pantalla y se va a proceder a terminar el proceso que la controlaba
            /// Devolver 0.
            /// </summary>
            WM_DESTROY = 0x0002,
            /// <summary>
            /// Enviado cuando la escala DPI se detecta que cambia en la ventana, bien porque la ventana se ha movido a otro monitor o el
            /// sistema que controla el monitor ha cambiado la configuracion
            /// wParam = {HIWORD = Y-axis ,  LOWORD = X-axis} . Usar MACRO x = new MACRO { Number = wParam }; Recuperar con x.Low y x.High
            /// lParam = *RECT
            /// Debe devolver 0.
            /// </summary>
            WM_DPICHANGED = 0x02E0,
            /// <summary>
            /// Enviado cuando se cambia la propiedad de enable de la ventana.
            /// wParam = TRUE if enabled || FALSE if disabled
            /// Debe devolver 0.
            /// </summary>
            WM_ENABLE = 0x000A,
            /// <summary>
            /// Enviado cuando una ventana empieza a ser movida o redimensionada
            /// Devolver 0.
            /// </summary>
            WM_ENTERSIZEMOVE = 0x0231,
            /// <summary>
            /// Enviado cuando una ventana termina de ser modificada su tamaño
            /// Devolver 0.
            /// </summary>
            WM_EXITSIZEMOVE = 0x0232,
            /// <summary>
            /// Pues algo de un handler del icono que puede ser de varios tipos
            /// wParam = ICON_BIG = 1 || ICON_SMALL = 0 | ICON_SMALL2(default icon provided by system) = 2
            /// lParam = DPI del icono 
            /// Devolver un handler de otro icono con Icon.FromHandle( new Bitmap(@"c:\FakePhoto.jpg").GetHicon() ) para cambiar, devolver el valor retornado por DefWindowProc para proseguir funcionamiento normal
            /// Mas info --> https://msdn.microsoft.com/es-es/library/windows/desktop/ms632625(v=vs.85).aspx
            /// </summary>
            WM_GETICON =  0x007F,
            /// <summary>
            /// Enviado cuando el tamaño de la ventana va a cambiar.
            /// lParam = *MINMAXINFO
            /// Devolver 0 para permitir seguir procesando el mensaje
            /// </summary>
            WM_GETMINMAXINFO = 0x0024,
            /// <summary>
            /// Enviado a la ventana con el foco cuando el lenguaje del input recibido es cambiado
            /// wParam = the character set of the new locale.
            /// lParam The input locale identifier. For more information, see Languages, Locales, and Keyboard Layouts.( https://msdn.microsoft.com/es-es/library/windows/desktop/ms646267(v=vs.85).aspx#_win32_Languages_Locales_and_Keyboard_Layouts )
            /// Devolver !0 para permitir que el mensaje se siga procesando.
            /// </summary>
            WM_INPUTLANGCHANGE = 0x0051,
            /// <summary>
            /// Enviado cuando el lenguaje del input es cambiado bien con un hotkey o desde la barra de tareas.
            /// wParam = INPUTLANGCHANGE_SYSCHARSET && ( INPUTLANGCHANGE_FORWARD || INPUTLANGCHANGE_BACKWARD)
            /// lParam = The input locale identifier.
            /// Devolver 0 para negar el cambio, devolver valor devuelto por funcion DefWindowProc para hacer efectivo el cambio
            /// </summary>
            WM_INPUTLANGCHANGEREQUEST = 0x0050,
            /// <summary>
            /// Enviado cuando una ventana ha terminado de ser movida
            /// lParam = MACRO.LOW(lParam) == x-coordinates && MACRO.HIGH(lParam) == y-coordinates
            /// </summary>
            WM_MOVE = 0x0003,
            /// <summary>
            /// Enviado cuando una ventana esta siendo movida
            /// lParam = *RECT
            /// Devolver TRUE pra continuar el proceso.
            /// </summary>
            WM_MOVING = 0x0216,
            /// <summary>
            /// Enviado cuando una se pierde el foco o se recupera
            /// wParam = TRUE si se debe dibujar un title bar y un icono activos || FALSE si debe dibujar un title bar y un icono inactivos
            /// lParam . Solo usado si no estan activos visual styles( https://msdn.microsoft.com/es-es/library/windows/desktop/bb773187(v=vs.85).aspx ) . If this parameter is set to -1, DefWindowProc does not repaint the nonclient area to reflect the state change.
            /// Devolver FALSE si wParam == false para evitar el proceso de redibujar titlebar e icono inactivos, TRUE para continuar el proceso normal. Si wParam == true, todo valor devuelto es ignorado
            /// </summary>
            WM_NCACTIVATE = 0x0086,
            /// <summary>
            /// Enviado cuando se redimensiona la ventana y la posición y tamaño de los elementos dentro deben ser redimensionados o movidos
            /// wParam = TRUE si la aplicación debe espeficar que parte del area contiene información valida || FALSE si no debe especificarlo
            /// lParam = *NCCALCSIZE_PARAMS  Si wParam == TRUE  || lParam = *RECT si wParam = false 
            /// Devolver 0 si wParam = false. Devolver  0 o uno de los valores especificados aqui: https://msdn.microsoft.com/es-es/library/windows/desktop/ms632634(v=vs.85).aspx
            /// </summary>
            WM_NCCALCSIZE = 0x0083,
            /// <summary>
            /// Enviado antes de WM_CREATE para inicializar los estilos basicos de una ventana (menu bar, title bar, taskbar, borders ,...)
            /// lParam = * CREATESTRUCT .
            /// Devolver FALSE para evitar la creacion de los elementos basicos de una ventana, devolver TRUE para continuar con el funcionamiento normal
            /// </summary>
            WM_NCCREATE = 0x0081,
            /// <summary>
            /// Enviado despues de WM_DESTROY por lo cual todos los hijos en este caso si han sido eliminados y para eliminar los elementos basicos de una ventana Windows
            /// Devolver 0.
            /// </summary>
            WM_NCDESTROY = 0x0082,
            /// <summary>
            /// Pues eso. Mas info https://msdn.microsoft.com/es-es/library/windows/desktop/ms632637(v=vs.85).aspx
            /// </summary>
            WM_NULL = 0x0000,
            /// <summary>
            /// Enviado cuando se piensa hacer drag sobre un icono de la barra de tareas.
            /// Devolver un handler de un Icono con LoadCursor or LoadIcon 
            /// </summary>
            WM_QUERYDRAGICON = 0x0037,
            /// <summary>
            /// Enviado cuando un usuario restaura una ventana minimizada para volver a su tamaño y localizacion previa
            /// Devolver true si se quiere que se abra normalmente, false para evitarlo.
            /// </summary>
            WM_QUERYOPEN = 0x0013,
            /// <summary>
            /// NO TRATAR COMO EVENTO NUNCA ES ENVIADO.
            /// Es un flag enviado por nosotros mismos para mandar a terminar una aplicacion con la función PostQuitMessage
            /// NO SE DEVUELVE NADA YA QUE NO SE CONTROLA NUNCA
            /// </summary>
            WM_QUIT = 0x0012,
            /// <summary>
            /// Enviado cuando una ventana va a ser escondida o mostrada
            /// wPAram = TRUE si vaa ser mostrado, FALSE si va a ser escondida
            /// lParam = 0 si la ventana se ha mostrado por consecuencia de una llamada a ShowWindow(). Consultar otros valores : https://msdn.microsoft.com/es-es/library/windows/desktop/ms632645(v=vs.85).aspx
            /// Devolver 0
            /// </summary>
            WM_SHOWWINDOW = 0x0018,
            /// <summary>
            /// Enviado cuando el tamaño de la ventana ha cambiado.
            /// wParam = {4(SIZE_MAXHIDE) = Mensaje enviado a todos los popup cuando otra ventana es mazimizada, 2(SIZE_MAXIMIZED) = La propia ventana ha sido mazimizada, 3(SIZE_MAXSHOW) = Mensaje enviado a todos los pop-ups cuando otra ventana ha sido restaurada a su tamaño original, 1(SIZE_MINIMIZED) = LA propia ventana ha sido minimizada, 0(SIZE_RESTORED) = La ventana ha sido redemensionada}
            /// lPAram = MACRO.Low(new width of the client area) || MACRO.High(new height of the client area)
            /// Devolver 0
            /// </summary>
            WM_SIZE = 0x0005,
            /// <summary>
            /// Enviado cuando el usuario esta redimensionado la ventana
            /// wPAram hace referencia desde que parte de la ventana se esta redimensionando.
            /// wPAram = {WMSZ_BOTTOM(6) = Bottom edge, WMSZ_BOTTOMLEFT(7) = Bottom-left corner, WMSZ_BOTTOMRIGHT(8) = Bottom-right corner, WMSZ_LEFT(1) = Left edge, WMSZ_RIGHT(2) = Right Edge, WMSZ_TOP(3) = Top edge, WMSZ_TOPLEFT(4) = Top-left corner, WMSZ_TOPRIGHT(5) = TOP-right corner }
            /// lParam = *RECT
            /// Devolver TRUE para permitir el correcto redimensionado.
            /// </summary>
            WM_SIZING = 0x0214,
            /// <summary>
            /// Enviado cuando se ha realizado un cambio en los estilos de la ventana por una llamada a SetWindowLong ha sido realizada
            /// wPAram indica que tipo de estilos han sido modificados
            /// wParam = { GWL_EXSTYLE(-20) = Extended window styiles changed ,  GWL_STYLE(-16) = The window styles have changed}
            /// lParam = *STYLESTRUCT
            /// Debe devolver 0
            /// </summary>
            WM_STYLECHANGED = 0x007D,
            /// <summary>
            /// Enviado cuando la llamada al metodo SetWindowLong esta apunto de realizar cambios en uno o mas estilos de la ventana
            /// wPAram indica que tipo de estilos han sido modificados
            /// wParam = { GWL_EXSTYLE(-20) = Extended window styiles changed ,  GWL_STYLE(-16) = The window styles have changed}
            /// lParam = *STYLESTRUCT
            /// Debe devolver 0
            /// </summary>
            WM_STYLECHANGING = 0x007C,
            /// <summary>
            /// Enviado cuando el tema de una aplicacion cambia (por ejemplo cambiar a tema windows basico provocaria que este mensaje se enviase)
            /// Debe devolver 0
            /// </summary>
            WM_THEMECHANGED = 0x031A,
            /// <summary>
            /// Mensaje enviado cuando se desloguea o loguea un usuario en el ordenador
            /// Debe devolver 0.
            /// </summary>
            WM_USERCHANGED = 0x0054,
            /// <summary>
            /// Mensaje enviado cuando ha ocurrido un cambio en el tamaño, posicion y z-index de la ventana a consecuencia de una llamada al metodo SetWindowPos 
            /// lParam = *WINDOWPOS
            /// Debe devolver 0
            /// </summary>
            WM_WINDOWPOSCHANGED = 0x0047,
            /// <summary>
            /// Mensaje enviado cuando va a ocurrir un cambio en el tamaño, posicion y z-index de la ventana a consecuencia de una llamada al metodo SetWindowPos 
            /// lParam = *WINDOWPOS
            /// Debe devolver 0
            /// </summary>
            WM_WINDOWPOSCHANGING = 0x0046,
        }
        
        //Mirar estilos --> https://msdn.microsoft.com/es-es/library/windows/desktop/ms632600(v=vs.85).aspx


        #region STRUCTS
        internal struct WINDOWINPUT
        {
            public IntPtr lParam;
            public UIntPtr wParam;
            public UInt32 message;
            public IntPtr hwnd;
        }
        /// <summary>
        /// Recibido en WM_GETMINMAXINFO
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct MINMAXINFO
        {
           
            POINT ptReserved; //No usar
            POINT ptMaxSize;// x = max width || y = max height
            POINT ptMaxPosition;// x = position of the left maximized side || y = position of the top maximized side
            POINT ptMinTrackSize;//x = SM_CXMINTRACK  || y = SM_CYMINTRACK
            POINT ptMaxTrackSize;// x = SM_CXMAXTRACK || y = SM_CYMAXTRACK

            MINMAXINFO(POINT ptMaxSize, POINT ptMaxPosition)
            {
                this.ptReserved = new POINT();
                this.ptMaxSize = ptMaxSize;
                this.ptMaxPosition = ptMaxPosition;
                this.ptMinTrackSize = new POINT(ControllerSystemInfo.getSystemInfo(ControllerSystemInfo.SystemMetric.SM_CXMINTRACK), ControllerSystemInfo.getSystemInfo(ControllerSystemInfo.SystemMetric.SM_CYMINTRACK));
                this.ptMaxTrackSize = new POINT(ControllerSystemInfo.getSystemInfo(ControllerSystemInfo.SystemMetric.SM_CXMAXTRACK), ControllerSystemInfo.getSystemInfo(ControllerSystemInfo.SystemMetric.SM_CYMAXTRACK));
            }
            
        }

        /// <summary>
        /// Recibido en mensaje WM_DPICHANGED
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left, top, right, bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }

            public RECT(System.Drawing.Rectangle r) : this(r.Left, r.Top, r.Right, r.Bottom) { }

            public int X
            {
                get { return left; }
                set { right -= (left - value); left = value; }
            }

            public int Y
            {
                get { return top; }
                set { bottom -= (top - value); top = value; }
            }

            public int Height
            {
                get { return bottom - top; }
                set { bottom = value + top; }
            }

            public int Width
            {
                get { return right - left; }
                set { right = value + left; }
            }

            public System.Drawing.Point Location
            {
                get { return new System.Drawing.Point(left, top); }
                set { X = value.X; Y = value.Y; }
            }

            public System.Drawing.Size Size
            {
                get { return new System.Drawing.Size(Width, Height); }
                set { Width = value.Width; Height = value.Height; }
            }

            public static implicit operator System.Drawing.Rectangle(RECT r)
            {
                return new System.Drawing.Rectangle(r.left, r.top, r.Width, r.Height);
            }

            public static implicit operator RECT(System.Drawing.Rectangle r)
            {
                return new RECT(r);
            }

            public static bool operator ==(RECT r1, RECT r2)
            {
                return r1.Equals(r2);
            }

            public static bool operator !=(RECT r1, RECT r2)
            {
                return !r1.Equals(r2);
            }

            public bool Equals(RECT r)
            {
                return r.left == left && r.top == top && r.right == right && r.bottom == bottom;
            }

            public override bool Equals(object obj)
            {
                if (obj is RECT)
                    return Equals((RECT)obj);
                else if (obj is System.Drawing.Rectangle)
                    return Equals(new RECT((System.Drawing.Rectangle)obj));
                return false;
            }

            public override int GetHashCode()
            {
                return ((System.Drawing.Rectangle)this).GetHashCode();
            }

            public override string ToString()
            {
                return string.Format(System.Globalization.CultureInfo.CurrentCulture, "{{Left={0},Top={1},Right={2},Bottom={3}}}", left, top, right, bottom);
            }
        }
        /// <summary>
        /// Recibido en mensaje WM_CREATE
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
        public struct CREATESTRUCT
        {
            public IntPtr lpCreateParams;
            public IntPtr hInstance;
            public IntPtr hMenu;
            public IntPtr hwndParent;
            public int cy;
            public int cx;
            public int y;
            public int x;
            public int style;
            public IntPtr lpszName;
            public IntPtr lpszClass;
            public int dwExStyle;
        }
        /// <summary>
        /// Recibido en WM_NCCALCSIZE
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        struct NCCALCSIZE_PARAMS
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            RECT[] rgrc;
            WINDOWPOS lppos;

            [StructLayout(LayoutKind.Sequential)]
            public struct WINDOWPOS
            {
                public IntPtr hwnd;
                public IntPtr hwndInsertAfter;
                public int x;
                public int y;
                public int cx;
                public int cy;
                public uint flags;
            }

        }

        /// <summary>
        /// Recibido en WM_STYLECHANGED
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        struct STYLESTRUCT
        {
            uint styleOld;
            uint styleNew;
        }
        #endregion STRUCTS
        
        private IWindowEventListener manejadorEventos = null;
        private IntPtr hwndTarget = IntPtr.Zero;
        private uint pID = 0;


        public WindowHook(IntPtr hwndTarget, IWindowEventListener manejadorEventos) : base(HookType.WH_CALLWNDPROC)
        {
            this.manejadorEventos = manejadorEventos;
            base.childProcedure = new hookHandler(hookCallback);

            Trace.WriteLine("_--------------------------------_");
            try
            {
                //this.hwndTarget = LoadLibrary("user32.dll");
                this.hwndTarget = hwndTarget;
                GetWindowThreadProcessId(this.hwndTarget, out this.pID); //PAsamos por referencia para que pID salga con el valor del proceso
                using ( Process targetProc = Process.GetProcessById( (int)this.pID ))
                using ( ProcessModule curModule = targetProc.MainModule )
                {
                    Trace.WriteLine("HANDLER NAME = " + curModule.ModuleName+" , HANDLER FULL FILE PATH = "+ curModule.FileName);
                    IntPtr Auxhandler = LoadLibrary(curModule.FileName); //Parece ser que sino se carga antes, GetModuleHandler fallara con codigo erro 126
                    IntPtr handler = GetModuleHandle(curModule.ModuleName);
                    if(handler == IntPtr.Zero)
                    {
                        //handler = Auxhandler;
                        int err = Marshal.GetLastWin32Error();
                        if (err != 0)
                            MessageBox.Show("No se ha podido encontrar el handler . Codigo error = " + err);

                    }
                    Trace.WriteLine("FOUNDED HANDLER ADRESS = " + handler.ToString());
                    base.init(handler, 0);
                   // base.init(IntPtr.Zero, 0);
                }
            }
            catch(Win32Exception e)
            {
                Trace.WriteLine("Codigo error = " + e.NativeErrorCode + " Descripción = " + e.Message);
                Trace.WriteLine( e.ToString() );
            }

        }

        private IntPtr hookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            bool result = true;
            if (nCode >= 0 && base.childProcedure != null)
            {
                if (this.manejadorEventos != null)
                {
                    WINDOWINPUT hookStruct = new WINDOWINPUT();
                    int param = wParam.ToInt32();
                    WindowEventType tipoEvento = Enum.IsDefined(typeof(WindowEventType), param) ? (WindowEventType)Enum.ToObject(typeof(WindowEventType), param) : WindowEventType.WM_NULL;
                    if (!tipoEvento.Equals(WindowEventType.WM_NULL ) )
                    {
                        hookStruct = (WINDOWINPUT)Marshal.PtrToStructure(lParam, typeof(WINDOWINPUT));
                        Debug.WriteLine("Se ha interceptado un evento de tipo "+ tipoEvento.ToString() + " con data { lParam = " + hookStruct.lParam + " , wParam = " + hookStruct.wParam + ", message = " + hookStruct.message + " , hwnd = " + hookStruct.hwnd + "} ");
                        CREATESTRUCT data;
                        RECT rectangulo;
                        MACRO x;
                        POINT punto;
                        switch (tipoEvento)
                        {
                            case WindowEventType.WM_DESTROY:
                                result = this.manejadorEventos.onBeforeWindowDestroy(this);
                            break;
                            case WindowEventType.WM_NCDESTROY:
                                result = this.manejadorEventos.onWindowDestroy(this);
                            break;
                            case WindowEventType.WM_NCCREATE:
                                data = (CREATESTRUCT)Marshal.PtrToStructure(hookStruct.lParam, typeof(CREATESTRUCT)); 
                                result = this.manejadorEventos.onBeforeWindowCreation(this, data);
                                break;
                            case WindowEventType.WM_CREATE:
                                data = (CREATESTRUCT)Marshal.PtrToStructure(hookStruct.lParam, typeof(CREATESTRUCT));
                                result = this.manejadorEventos.onWindowCreation(this, data);
                                break;
                            case WindowEventType.WM_DPICHANGED:
                                x = new MACRO { Number = hookStruct.wParam.ToUInt32() };
                                punto = new POINT(x.Low, x.High);
                                rectangulo = (RECT)Marshal.PtrToStructure(hookStruct.lParam, typeof(RECT));
                                result = this.manejadorEventos.onWindowDPIChange(this, punto, rectangulo);
                            break;
                            case WindowEventType.WM_MOVING:
                                rectangulo = (RECT)Marshal.PtrToStructure(hookStruct.lParam, typeof(RECT));
                                result = this.manejadorEventos.onWindowMoving(this,rectangulo);
                            break;
                            case WindowEventType.WM_MOVE:
                                x = new MACRO { Number = unchecked((uint)hookStruct.lParam.ToInt32()) };
                                punto = new POINT(x.Low, x.High);
                                result = this.manejadorEventos.onWindowMoved(this, punto);
                            break;
                            case WindowEventType.WM_SIZING:
                                rectangulo = (RECT)Marshal.PtrToStructure(hookStruct.lParam, typeof(RECT));
                                result = this.manejadorEventos.onWindowResizing(this,rectangulo);
                                break;
                            case WindowEventType.WM_SIZE:
                                x = new MACRO { Number = unchecked((uint)hookStruct.lParam.ToInt32()) };
                                punto = new POINT(x.Low, x.High);
                                result = this.manejadorEventos.onWindowResized(this, punto);
                                break;
                        }
                    }
                }
            }
            if (result) return CallNextHookEx(base.getHookID(), nCode, wParam, lParam);
            else return (IntPtr)1;
        }

        public RECT GetWindowRect(IntPtr hwnd)
        {
            RECT rectangulo = new RECT();
            GetWindowRect(hwnd, out rectangulo);
            return rectangulo;
        }
    }

    
}
