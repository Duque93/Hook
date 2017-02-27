using Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Utils;
using static Structs.Commons;

namespace Hook
{

    //https://msdn.microsoft.com/en-us/library/windows/desktop/ms644986(v=vs.85).aspx
    class MouseHook : BaseHook
    {
        private enum MouseEventType : int
        {  //Siempre usar hexadecimales pues se mantiene mejor la compatibilidad entre versiones del SO de Windows
            NONE = 0,
            WM_MOUSEMOVE = 0x0200, //512,
            WM_LBUTTONDOWN = 0x0201, //513,
            WM_LBUTTONUP = 0x0202, //514,
            WM_RBUTTONDOWN = 0x0204, //516,
            WM_RBUTTONUP = 0x0205, //517,
            WM_MBUTTONDOWN = 0x0207, //519,
            WM_MBUTTONUP = 0x0208, //520,
            WM_MOUSEWHEEL = 0x020A, //522,
            WM_MOUSEHWHEEL = 0x020E, //526
        }
        //https://msdn.microsoft.com/es-es/library/windows/desktop/ms646260(v=vs.85).aspx Parameters section
        private enum MouseFlags : int
        {
            MOUSEEVENTF_MOVE = 0x0001, //1,
            MOUSEEVENTF_LEFTDOWN = 0x0002, //2,
            MOUSEEVENTF_LEFTUP = 0x0004, //4,
            MOUSEEVENTF_RIGHTDOWN = 0x0008, //8,
            MOUSEEVENTF_RIGHTUP = 0x0010, //16,
            MOUSEEVENTF_MIDDLEDOWN = 0x0020, //32,
            MOUSEEVENTF_MIDDLEUP = 0x0040, //64,
            MOUSEEVENTF_XDOWN = 0x0080, //128,
            MOUSEEVENTF_XUP = 0x0100, //256,
            MOUSEEVENTF_WHEEL = 0x0800, //2048,
            MOUSEEVENTF_HWHEEL = 0x01000, //4096,
            /// <summary>
            /// The dx and dy parameters contain normalized absolute coordinates. If not set, those parameters contain 
            /// relative data: the change in position since the last reported position. This flag can be set, or not set, 
            /// regardless of what kind of mouse or mouse-like device, if any, is connected to the system. For further information
            /// about relative mouse motion, see the following Remarks section.
            /// </summary>
            MOUSEEVENTF_ABSOLUTE = 0x8000, //32768,

        }

        private struct AREA
        {
            POINT topLeft;
            POINT bottomRight;

            public AREA(POINT a, POINT b)
            {
                this.topLeft = a;
                this.bottomRight = b;
            }

            public bool isInArea(POINT a)
            {
                return (topLeft.posX <= a.posX && topLeft.posY <= a.posY && bottomRight.posX >= a.posX && bottomRight.posY >= a.posY);
            }
        }

        private const int TIPO_MOUSEVENT = 0;
        public POINT currentPos
        {
            get;
            private set;
        }
        private IMouseEventsListener manejadorEventos;
        private bool stopPropagation = false;
        private Thread hilo; //Tengo que definirlo como propiedad para la función moveToPointHumanized() sino quiero que l garbage collector me elimine la variable del hil oy mete aposteriori
        private static MouseHook instance = null;

        public static MouseHook getInstance(IMouseEventsListener manejadorEventos)
        {
            if (instance == null) instance = new MouseHook(manejadorEventos);
            return instance;
        }

        private MouseHook(IMouseEventsListener manejadorEventos) : base(HookType.WH_MOUSE_LL)
        {
            this.manejadorEventos = manejadorEventos;
            base.childProcedure = new hookHandler(hookCallback);
            base.init(IntPtr.Zero, 0);
        }
        
         //Cambiar a static para evitar que al GC se le vaya la olla y me lo borre
        private static IntPtr hookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (instance == null) return (IntPtr)1;

            bool result = true;
            if (nCode >= 0 && instance.childProcedure != null)
            {
                if (instance.manejadorEventos != null)
                {
                    MOUSEINPUT hookStruct = new MOUSEINPUT();
                    int param = wParam.ToInt32();
                    MouseEventType tipoEvento = Enum.IsDefined(typeof(MouseEventType), param) ? (MouseEventType)Enum.ToObject(typeof(MouseEventType), param) : MouseEventType.NONE;
                    if (!tipoEvento.Equals(MouseEventType.NONE))
                    {

                        hookStruct = (MOUSEINPUT)Marshal.PtrToStructure(lParam, typeof(MOUSEINPUT)); //Recuperamos el valor guardado en la dirección de memoria especificada
                        Debug.WriteLineIf(instance.stopPropagation, "Se ha interceptado un vento de tipo " + tipoEvento.ToString() + " con data { MouseData = " + hookStruct.mouseData + " , ExtraInfo = " + hookStruct.dwExtraInfo + ", Time = " + hookStruct.time + " , Flags = " + hookStruct.dwFlags + "} ");
                        switch (tipoEvento)
                        {
                            case MouseEventType.WM_MOUSEMOVE:
                                instance.currentPos = new POINT(hookStruct.dx, hookStruct.dy);
                                result = instance.manejadorEventos.onMouseMove(instance, hookStruct);
                                if (instance.stopPropagation == true && hookStruct.dwExtraInfo.ToInt32() != -1) //Se esta ejecutando moveToPointHumanized y no se trata de un evento enviado por el mismo
                                {
                                    result = false;
                                }
                                break;
                            case MouseEventType.WM_LBUTTONDOWN:
                                result = instance.manejadorEventos.onLeftMouseButtonDown(instance, hookStruct);
                                break;
                            case MouseEventType.WM_LBUTTONUP:
                                result = instance.manejadorEventos.onLeftMouseButtonUp(instance, hookStruct);
                                break;
                            case MouseEventType.WM_MBUTTONDOWN:
                                result = instance.manejadorEventos.onMiddleMouseButtonDown(instance, hookStruct);
                                break;
                            case MouseEventType.WM_MBUTTONUP:
                                result = instance.manejadorEventos.onMiddleMouseButtonUp(instance, hookStruct);
                                break;
                            case MouseEventType.WM_RBUTTONDOWN:
                                result = instance.manejadorEventos.onRightMouseButtonDown(instance, hookStruct);
                                break;
                            case MouseEventType.WM_RBUTTONUP:
                                result = instance.manejadorEventos.onRightMouseButtonUp(instance, hookStruct);
                                break;
                            case MouseEventType.WM_MOUSEWHEEL:
                                result = instance.manejadorEventos.onVerticalWheelMouseRotation(instance, hookStruct);
                                break;
                            case MouseEventType.WM_MOUSEHWHEEL:
                                result = instance.manejadorEventos.onHorizontalWheelMouseRotation(instance, hookStruct);
                                break;
                        }
                    }
                }
            }

            if (result) return CallNextHookEx(instance.getHookID(), nCode, wParam, lParam); //Permite seguir propagando el evento a otras aplicaciones
            else return (IntPtr)1; //Bloquea la propagación del evento a otras aplicaciones
        }
        //http://stackoverflow.com/a/4555214
        public bool moveToPoint(decimal x, decimal y)
        {
            List<INPUT> mouseInputList = new List<INPUT>();

            INPUT mouseEvent = new INPUT() { };
            mouseEvent.tipo = TIPO_MOUSEVENT;
            //Si pongo de la manera directa sin MouseFlags.MOUSEEVENTF_ABSOLUTE en el parametro de flag, resulta que simplemente suma a la posición actual del raton.
            //La manera correcta es la siguiente
            mouseEvent.data.mouse.dx = Convert.ToInt32( x*( 65536.0m / ControllerSystemInfo.getSystemInfo(ControllerSystemInfo.SystemMetric.SM_CXSCREEN)) ); //Se recomienda usar 65536.0f.
            mouseEvent.data.mouse.dy = Convert.ToInt32( y*( 65536.0m / ControllerSystemInfo.getSystemInfo(ControllerSystemInfo.SystemMetric.SM_CYSCREEN)) );
            mouseEvent.data.mouse.mouseData = 0;
            mouseEvent.data.mouse.time = 0;
            mouseEvent.data.mouse.dwExtraInfo = new IntPtr(-1); //Coloco esto como flag para saber cuando es automatizado //mouseEvent.data.mouse.dwExtraInfo = ControllerSystemInfo.GetMessageExtraInfo();
            mouseEvent.data.mouse.dwFlags =  (uint)MouseFlags.MOUSEEVENTF_ABSOLUTE | (uint)MouseFlags.MOUSEEVENTF_MOVE ;
            
            if (  SendInput( 1, ref mouseEvent, Marshal.SizeOf(typeof(INPUT))) == 0 )
            {
                MessageBox.Show("Ha fallado el remplazo del evento en MouseHook. Codigo error -->"+ Marshal.GetLastWin32Error());
            }

            return false;
        }
        public bool moveToPoint(POINT target)
        {
            return moveToPoint(target.posX, target.posY);
        }
        //
        public void moveToPointHumanized( POINT targetPos, int margen)
        {
            if (!stopPropagation) //Para evitar que se puedan crear 2 o mas hilos interactuand ocon el raton
            {
                hilo = new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    int milisegundosPasados = 0;


                
                    POINT margeninferior = targetPos - new POINT(2, 2);
                    POINT margenSuperior = targetPos + new POINT(2, 2);
                    AREA margenArea = new AREA(margeninferior, margenSuperior);
                
                    
                    //Ecuación de la recta --> y = mx + b;
                    decimal m = (decimal)(targetPos.posY - currentPos.posY) / (decimal)(targetPos.posX - currentPos.posX); //Hallamos m
                    decimal b = -m * currentPos.posX + currentPos.posY; //Hallamos b

                    decimal x = currentPos.posX;
                    decimal y = m * x + b;

                    //Cuanto tiene que valer x para que y sea igual a targetPos.posY;
                    // y = m * x + b --> (y-b)/m
                    int limit = (int)((targetPos.posY - b) / m);
                    Trace.WriteLine("Entonces y == 100 cuando x == "+limit+". Y x == 100 cuando y == "+ (m * 100 + b));
                    //Sabemos el limite, debemos hallar el incremento que x debe tener para que tanto x como y llegan al valor especificado
                    int incrX = x > limit ? -1 : 1;
                    instance.stopPropagation = true;
                    while ( !margenArea.isInArea(currentPos) )
                    {
                        currentPos = new POINT(Math.Abs(x), Math.Abs(y));
                        this.moveToPoint(currentPos);
                        milisegundosPasados += 1;
                        System.Threading.Thread.Sleep(1);

                        x += incrX;
                        y =(int)(m * x + b);
                    }
                    instance.stopPropagation = false;
                    
                });
                hilo.Start();
            }

            
        }
        /////////////FIN CLASE
    }

    
}
