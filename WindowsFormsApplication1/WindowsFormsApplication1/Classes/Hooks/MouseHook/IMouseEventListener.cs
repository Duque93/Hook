using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Hook.BaseHook;

namespace Interfaces
{ 

    interface IMouseEventsListener
    {
        bool onMouseMove(object sender, MOUSEINPUT data);

        bool onLeftMouseButtonDown(object sender, MOUSEINPUT data);
        bool onLeftMouseButtonUp(object sender, MOUSEINPUT data);

        bool onRightMouseButtonDown(object sender, MOUSEINPUT data);
        bool onRightMouseButtonUp(object sender, MOUSEINPUT data);

        bool onMiddleMouseButtonDown(object sender, MOUSEINPUT data);
        bool onMiddleMouseButtonUp(object sender, MOUSEINPUT data);

        bool onVerticalWheelMouseRotation(object sender, MOUSEINPUT data);
        bool onHorizontalWheelMouseRotation(object sender, MOUSEINPUT data);
    }
}
