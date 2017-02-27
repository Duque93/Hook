using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1.Classes.Utils
{
    public static class Utilidades
    {
        [StructLayout(LayoutKind.Explicit)]
        public struct MACRO
        {
            [FieldOffset(0)]
            public uint Number;

            [FieldOffset(0)]
            public ushort Low;

            [FieldOffset(2)]
            public ushort High;
        } //ej --> MACRO x = new MACRO { Number = 0xDEADBEEF }; 

    }
}
