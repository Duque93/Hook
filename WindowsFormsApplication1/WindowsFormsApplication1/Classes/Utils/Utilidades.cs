using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
    }
}
