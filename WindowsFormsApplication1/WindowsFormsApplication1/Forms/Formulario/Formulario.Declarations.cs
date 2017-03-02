using Hook;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication1.Forms;
using static Structs.Commons;

namespace OverlayDrawingTest
{
    public partial class Formulario : Form
    {
        #region DLLImports

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string className, string windowName);

        #endregion DLLImports

        #region Parameters

        private delegate void LoadingFormCallback(bool end);
        LoadingFormCallback ptrCallbackLoadingForm = null;

        private delegate void AppendTextCallback(RichTextBox textarea, string content);
        AppendTextCallback ptrCallbackAppendText = null;

        private OverlayForm overlayProcess = null;
        private OverlayForm overlayDebugging = null;
        private ProcessListForm listaProcesosForm = null;

        //private List<IComunicator> chivatoHijos = null;
        public event EventHandler onChildClose;
        private IntPtr handlerFounded = IntPtr.Zero;
        private KeyBoardHook hookTeclado = null;
        private MouseHook hookRaton = null;

        #endregion Parameters

        #region Hooks

            #region KeyBoardEventsListener
        private class KeyBoardEventsListener : IKeyBoardEventsListener
        {
            private Formulario self;
            private static IKeyBoardEventsListener instance;

            public static IKeyBoardEventsListener getInstance(Formulario formulario)
            {
                if (instance == null)
                {
                    instance = new KeyBoardEventsListener(formulario);
                }
                return instance;
            }

            private KeyBoardEventsListener(Formulario formulario)
            {
                self = formulario;
            }

            //////////////////////////////////////////////////////////////
            public bool onKeyDown(object sender, int code, Keys pressedKey)
            {
                if (pressedKey.Equals(Keys.D))
                {
                    //self.hookRaton.moveToPoint(100, 100); 
                    self.hookRaton.moveToPointHumanized(new POINT(100, 100));
                    return true;
                    //return ((KeyBoardHook)sender).replaceKey(Keys.B);
                }
                else return true; //set to false to avoid event propagation
            }

            public bool onKeyUp(object sender, int code, Keys pressedKey)
            {
                return true; //set to false to avoid event propagation
            }
        }
        #endregion KeyBoardEventsListener

            #region MouseEventsListener

        private class MouseEventListener : IMouseEventsListener
        {
            private Formulario self;
            private static IMouseEventsListener instance;
            public static IMouseEventsListener getInstance(Formulario formulario)
            {
                if (instance == null)
                {
                    instance = new MouseEventListener(formulario);
                }
                return instance;
            }

            private MouseEventListener(Formulario formulario)
            {
                self = formulario;
            }
            //////////////////////////////////////////////////////////////////////
            // Implementando metodos 
            public bool onHorizontalWheelMouseRotation(object sender, BaseHook.MOUSEINPUT data)
            {
                return true;
            }

            public bool onLeftMouseButtonDown(object sender, BaseHook.MOUSEINPUT data)
            {
                return true;
            }

            public bool onLeftMouseButtonUp(object sender, BaseHook.MOUSEINPUT data)
            {
                return true;
            }

            public bool onMiddleMouseButtonDown(object sender, BaseHook.MOUSEINPUT data)
            {
                return true;
            }

            public bool onMiddleMouseButtonUp(object sender, BaseHook.MOUSEINPUT data)
            {
                return true;
            }

            public bool onMouseMove(object sender, BaseHook.MOUSEINPUT data)
            {
                self.richTextBox1.ResetText();
                self.richTextBox1.Text += "El ratón se encuentra en la posición{ x = "+data.dx+" , y = "+data.dy+" }";
                return true;
            }

            public bool onRightMouseButtonDown(object sender, BaseHook.MOUSEINPUT data)
            {
                return true;
            }

            public bool onRightMouseButtonUp(object sender, BaseHook.MOUSEINPUT data)
            {
                return true;
            }

            public bool onVerticalWheelMouseRotation(object sender, BaseHook.MOUSEINPUT data)
            {
                return true;
            }
            //FIN Implementacion
            //////////////////////////////////////////////////////////////////////
        }

        #endregion MouseEventsListener

        #endregion Hooks
    }
}
