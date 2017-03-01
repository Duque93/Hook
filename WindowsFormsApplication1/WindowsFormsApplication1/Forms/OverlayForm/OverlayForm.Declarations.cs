#define DEBUG

using Hook;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static Hook.WindowHook;
using Structs;

namespace OverlayDrawingTest
{
    public partial class OverlayForm : Form
    {
        private static bool DEBUG
        {
            get
            {
                #if DEBUG
                    Console.WriteLine("DEBUG MODE!!!!");
                    return true;
                #else
                    Console.WriteLine("Release MODE!!!!");
                    return false;
                #endif
            }
            set
            {
                throw new Exception("You're not autorized to change the value of DEBUG");
            }
        }


        #region Properties

        delegate void SetIntsCallback(int top, int left); //Necesario para la modificacion de parametros atraves de distintos hilos

        IComunicator chivatoPadre = null;
        IntPtr handlerTarget = IntPtr.Zero;
        RECT windowTargetRect;
        RECT ctrlTargetRect {
            get
            {
                return windowTargetRect;
            }
            set
            {
                windowTargetRect = value;
                this.adjust();
            }
        }
        bool working = false;
        //Graphics g; //¿PARA QUE?
        Pen lapiz = null;
        private WindowHook hookWindow = null;

        #endregion Properties

        #region InnerClasses

        private class WindowEventListener : IWindowEventListener
        {
            private static WindowEventListener instance = null;
            private OverlayForm self = null;

            public static IWindowEventListener getInstance(OverlayForm parent)
            {
                if(instance == null)
                {
                    instance = new WindowEventListener(parent);
                }
                return instance;
            }

            private WindowEventListener(OverlayForm parent)
            {
                this.self = parent;
            }
            //IMPLEMENTACION: 
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            public bool onBeforeWindowCreation(object sender, CREATESTRUCT data)
            {
                return true;
            }

            public bool onBeforeWindowDestroy(object sender)
            {
                self.Dispose();
                return true;
            }

            public bool onWindowCreation(object sender, CREATESTRUCT data)
            {
                return true;
            }

            public bool onWindowDestroy(object sender)
            {
                self.Close();
                return true;
            }

            public bool onWindowDPIChange(object sender, Commons.POINT pos, RECT rect)
            {
                self.ctrlTargetRect = rect;
                self.adjust();
                return true;
            }

            public bool onWindowMoved(object sender, Commons.POINT punto)
            {
                return true;
            }

            public bool onWindowMoving(object sender, RECT rectangulo)
            {

                self.ctrlTargetRect = rectangulo;
                self.adjust();
                return true;
            }

            public bool onWindowResized(object sender, Commons.POINT punto)
            {
                return true;
            }

            public bool onWindowResizing(object sender, RECT rectangulo)
            {
                self.ctrlTargetRect = rectangulo;
                self.adjust();
                return true;
            }
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        }

        #endregion InnerClasses
    }
}
