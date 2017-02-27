using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Interfaces
{
    interface IKeyBoardEventsListener
    {
        bool onKeyDown(object sender,int code,Keys pressedKey);
        bool onKeyUp(object sender, int code, Keys pressedKey);

    }
}
