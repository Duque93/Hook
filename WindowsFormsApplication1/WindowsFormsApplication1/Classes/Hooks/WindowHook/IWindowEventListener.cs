using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Hook.WindowHook;
using static Hook.WindowHook.MINMAXINFO;
using static Structs.Commons;

namespace Hook
{
    interface IWindowEventListener
    {
        
        bool onWindowDPIChange(object sender, POINT pos, RECT rect); //WM_DPICHANGED
        bool onWindowMoving(object sender, RECT rectangulo); //WM_MOVING
        bool onWindowMoved(object sender, POINT punto); //WM_MOVE
        bool onWindowResizing(object sender, RECT rectangulo); //WM_SIZING
        bool onWindowResized(object sender, POINT punto); //WM_SIZE
        bool onBeforeWindowCreation(object sender, CREATESTRUCT data); //WM_NCCREATE
        bool onWindowCreation(object sender, CREATESTRUCT data); //WM_CREATE
        /// <summary>
        /// Enviado cuando se lanza un mensaje para confirmar que se va a cerrar la ventana. No indica que la ventana se vaya a cerrar
        /// </summary>
        /// <returns></returns>
        bool onBeforeWindowDestroy(object sender); //WM_DESTROY
        bool onWindowDestroy(object sender); //WM_NCDESTROY

    }
}
