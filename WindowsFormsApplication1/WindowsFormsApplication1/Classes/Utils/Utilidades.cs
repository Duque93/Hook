using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        #region FormUtils
        public static ColumnHeader createCabecera(String name, String texto, HorizontalAlignment alineamientoTexto = HorizontalAlignment.Center)
        {
            ColumnHeader columnHeader = new ColumnHeader();
            columnHeader.Name = name;
            columnHeader.Text = texto;
            columnHeader.TextAlign = alineamientoTexto;
            columnHeader.Width = -2; //Un valor exacto -2 es interpretado como redimensionar para mostrar todo el contenido

            columnHeader.Tag = columnHeader;

            return columnHeader;
        }

        public static ListViewItem createItem(String name, object tag, params string[] texto)
        {
            ListViewItem item = new ListViewItem(texto);
            item.Name = name;
            item.Tag = tag;
            item.BackColor = Color.White;

            return item;
        }
        public static ListViewItem createErrorItem(String name, object tag, params string[] texto)
        {
            ListViewItem item = Utilidades.createItem(name, tag, texto);
            item.ForeColor = Color.White;
            item.BackColor = Color.Red;

            return item;
        }

        public static Size calculateMaxSize(ListView lista)
        {
            int MaxWidth = 0, height = 0;
            int tamaño = lista.Items.Count;
            for (int i = 0; i < tamaño; i++)
            {
                Rectangle rect = lista.GetItemRect(i);
                MaxWidth = MaxWidth < rect.Width ? rect.Width : MaxWidth;
                height += rect.Height;
            }
            height += 35; //No hemos añadido lo que seria la altura de la cabecera
            return new Size(MaxWidth, height);

        }

        #endregion FormUtils

        #region Network

        /// <summary>
        /// http://www.pinvoke.net/default.aspx/iphlpapi.getextendedtcptable
        /// </summary>
        /// <param name="ipVersion"></param>
        /// <param name="pId"></param>
        /// <returns></returns>
        public static List<IPR> GetConnections<IPT, IPR>(int ipVersion)//IPR = Row Type, IPT = Table Type
        {
            IPR[] tableRows;
            int buffSize = 0;

            var dwNumEntriesField = typeof(IPT).GetField("dwNumEntries");

            // how much memory do we need?
            uint ret = typeof(IPT).Equals(typeof(TCP_TABLE_CLASS)) ? GetExtendedTcpTable(IntPtr.Zero, ref buffSize, true, ipVersion, TCP_TABLE_CLASS.TCP_TABLE_CLASS_TYPES.TCP_TABLE_OWNER_PID_ALL)  : GetExtendedUdpTable(IntPtr.Zero, ref buffSize, true, ipVersion, UDP_TABLE_CLASS.UDP_TABLE_CLASS_TYPES.UDP_TABLE_OWNER_PID);
            IntPtr tcpTablePtr = Marshal.AllocHGlobal(buffSize);

            try
            {
                ret = typeof(IPT).Equals(typeof(TCP_TABLE_CLASS)) ? GetExtendedTcpTable(tcpTablePtr, ref buffSize, true, ipVersion, TCP_TABLE_CLASS.TCP_TABLE_CLASS_TYPES.TCP_TABLE_OWNER_PID_ALL) : GetExtendedUdpTable(tcpTablePtr, ref buffSize, true, ipVersion, UDP_TABLE_CLASS.UDP_TABLE_CLASS_TYPES.UDP_TABLE_OWNER_PID);
                if (ret != 0)
                    return new List<IPR>();

                // get the number of entries in the table
                IPT table = (IPT)Marshal.PtrToStructure(tcpTablePtr, typeof(IPT));
                int rowStructSize = Marshal.SizeOf(typeof(IPR));
                uint numEntries = (uint)dwNumEntriesField.GetValue(table);

                // buffer we will be returning
                tableRows = new IPR[numEntries];

                IntPtr rowPtr = (IntPtr)((long)tcpTablePtr + 4);
                //var owningPidField = typeof(IPR).GetField("owningPid");
                for (int i = 0; i < numEntries; i++)
                {
                    IPR tcpRow = (IPR)Marshal.PtrToStructure(rowPtr, typeof(IPR));
                    /*if(pId == (uint)owningPidField.GetValue(tcpRow))
                    {*/
                    tableRows[i] = tcpRow;
                    //}
                    rowPtr = (IntPtr)((long)rowPtr + rowStructSize);   // next entrys
                }
            }
            finally
            {
                // Free the Memory
                Marshal.FreeHGlobal(tcpTablePtr);
            }
            return tableRows != null ? tableRows.ToList() : new List<IPR>();
        }

        public static int IPv4 = 2;    // IP_v4 = System.Net.Sockets.AddressFamily.InterNetwork
        public static int IPv6 = 23;  // IP_v6 = System.Net.Sockets.AddressFamily.InterNetworkV6

        [DllImport("iphlpapi.dll", SetLastError = true)] //https://msdn.microsoft.com/es-es/library/windows/desktop/aa365928(v=vs.85).aspx
        static extern uint GetExtendedTcpTable(IntPtr pUdpTable, ref int dwOutBufLen, bool sort, int ipVersion, TCP_TABLE_CLASS.TCP_TABLE_CLASS_TYPES tblClass, uint reserved = 0);
        [DllImport("iphlpapi.dll", SetLastError = true)] //https://msdn.microsoft.com/en-us/library/windows/desktop/aa365930(v=vs.85).aspx
        static extern uint GetExtendedUdpTable(IntPtr pTcpTable, ref int dwOutBufLen, bool sort, int ipVersion, UDP_TABLE_CLASS.UDP_TABLE_CLASS_TYPES tblClass, uint reserved = 0);

        /// <summary>
        /// 
        /// </summary>
        public struct UDP_TABLE_CLASS //De tipo --> _MIB_UDPTABLE_OWNER_PID --> mas info https://msdn.microsoft.com/en-us/library/windows/desktop/aa365930(v=vs.85).aspx
        {
            public enum UDP_TABLE_CLASS_TYPES
            {
                UDP_TABLE_BASIC,
                UDP_TABLE_OWNER_PID,
                UDP_TABLE_OWNER_MODULE
            }

            public uint dwNumEntries;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 1)]
            public UDPROW_OWNER_PID[] table;

            /// <summary>
            /// 
            /// </summary>
            [StructLayout(LayoutKind.Sequential)]
            public struct UDPROW_OWNER_PID
            {
                public uint localAddr;
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
                public byte[] localPort;
                public uint owningPid;

                public IPAddress LocalAddress
                {
                    get { return new IPAddress(localAddr); }
                }

                public ushort LocalPort
                {
                    get
                    {
                        return BitConverter.ToUInt16(new byte[2] { localPort[1], localPort[0] }, 0);
                    }
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct TCP_TABLE_CLASS //De tipo --> MIB_TCPTABLE_OWNER_PID --> mas info https://msdn.microsoft.com/en-us/library/windows/desktop/aa365928(v=vs.85).aspx
        {
            public enum TCP_TABLE_CLASS_TYPES
            {
                TCP_TABLE_BASIC_LISTENER,
                TCP_TABLE_BASIC_CONNECTIONS,
                TCP_TABLE_BASIC_ALL,
                TCP_TABLE_OWNER_PID_LISTENER,
                TCP_TABLE_OWNER_PID_CONNECTIONS,
                TCP_TABLE_OWNER_PID_ALL,
                TCP_TABLE_OWNER_MODULE_LISTENER,
                TCP_TABLE_OWNER_MODULE_CONNECTIONS,
                TCP_TABLE_OWNER_MODULE_ALL
            }

            public uint dwNumEntries;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 1)]
            public TCPROW_OWNER_PID[] table;

            [StructLayout(LayoutKind.Sequential)]
            public struct TCPROW_OWNER_PID
            {
                public uint state;
                public uint localAddr;
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
                public byte[] localPort;
                public uint remoteAddr;
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
                public byte[] remotePort;
                public uint owningPid;

                public uint ProcessId
                {
                    get { return owningPid; }
                }

                public IPAddress LocalAddress
                {
                    get { return new IPAddress(localAddr); }
                }

                public ushort LocalPort
                {
                    get
                    {
                        return BitConverter.ToUInt16(new byte[2] { localPort[1], localPort[0] }, 0);
                    }
                }

                public IPAddress RemoteAddress
                {
                    get { return new IPAddress(remoteAddr); }
                }

                public ushort RemotePort
                {
                    get
                    {
                        return BitConverter.ToUInt16(new byte[2] { remotePort[1], remotePort[0] }, 0);
                    }
                }
                private enum STATE : uint //mas info --> https://msdn.microsoft.com/en-us/library/windows/desktop/aa366913(v=vs.85).aspx
                {
                    UNDEFINED = 0,
                    CLOSED = 1,
                    LISTEN = 2,
                    SYN_SENT = 3,
                    SYN_RECEIVED = 4,
                    ESTABLISHED = 5,
                    FIN_WAIT_1 = 6,
                    FIN_WAIT_2 = 7,
                    CLOSE_WAIT = 8,
                    CLOSING = 9,
                    LAST_ACK = 10,
                    TIME_WAIT = 11,
                    TCB = 12,
                }
                public String State
                {
                    get
                    {
                        STATE estado = Enum.IsDefined(typeof(STATE), state) ? (STATE)Enum.ToObject(typeof(STATE), state) : STATE.UNDEFINED;
                        return estado.ToString();
                    }
                }
            }
        }


        #endregion Network
    }
}
