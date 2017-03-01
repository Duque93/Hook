using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils;

namespace WindowsFormsApplication1.Forms
{
    public partial class ProcessListForm : Form
    {
        public ProcessListForm()
        {
            InitializeComponent();


            this.init();
            
            this.Show();
        }

        private void init()
        {
            this.listProcessView.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            this.listProcessView.View = View.Details;
            
            this.listProcessView.Columns.Add(this.createCabecera("headerPiD", "PiD"));
            this.listProcessView.Columns.Add(this.createCabecera("headerProcessName", "Proceso"));
            this.listProcessView.Columns.Add(this.createCabecera("headerPath", "Ruta"));
            this.listProcessView.Columns.Add(this.createCabecera("headerBaseAddress", "Entry Point Address"));
            this.listProcessView.Columns.Add(this.createCabecera("headerBaseAddress", "Base Address"));

            int errors = 0;
            foreach (Process proc in Process.GetProcesses())
            {
                try
                {
                    using (ProcessModule procModule = proc.MainModule)
                    {
                        ListViewItem item = this.createItem("ItemProc" + proc.Id, proc, proc.Id.ToString(), procModule.ModuleName, procModule.FileName, "0x" + procModule.EntryPointAddress.ToInt64().ToString("X"), "0x" + procModule.BaseAddress.ToInt64().ToString("X"));
                    }
                }
                catch (Win32Exception e)
                {
                    errors += 1;
                    this.listProcessView.Items.Add(this.createErrorItem("ItemProcErrorNum" + errors, e, e.ErrorCode.ToString(), "Modulo Virtual", "System.dll", "0x0"));
                    Trace.WriteLine("Codigo error = " + e.NativeErrorCode + " Descripción = " + e.Message);
                    Trace.WriteLine(e.ToString());
                }
            }
        }

        private ColumnHeader createCabecera(String name, String texto, HorizontalAlignment alineamientoTexto = HorizontalAlignment.Center)
        {
            ColumnHeader columnHeader = new ColumnHeader();
            columnHeader.Name = name;
            columnHeader.Text = texto;
            columnHeader.TextAlign = alineamientoTexto;
            columnHeader.Width = -2; //Un valor exacto -2 es interpretado como redimensionar para mostrar todo el contenido
 
            columnHeader.Tag = columnHeader;

           

            return columnHeader;
        }

        private ListViewItem createItem(String name, object tag , params string[] texto)
        {
            ListViewItem item = new ListViewItem(texto);
            item.Name = name;
            item.Tag = tag;
            item.BackColor = Color.White;

            return item; 
        }
        private ListViewItem createErrorItem(String name, object tag, params string[] texto)
        {
            ListViewItem item = this.createItem(name,tag,texto);
            item.ForeColor = Color.White;
            item.BackColor = Color.Red;


            return item;
        }

        private Size calculateMaxSize(ListView lista)
        {
            int MaxWidth = 0, height = 0;
            int tamaño = lista.Items.Count;
            for(int i = 0; i < tamaño; i++)
            {
                Rectangle rect = lista.GetItemRect(i);
                MaxWidth = MaxWidth < rect.Width ? rect.Width : MaxWidth ;
                height += rect.Height;
            }
            height += 35; //No hemos añadido lo que seria la altura de la cabecera
            return new Size(MaxWidth, height);

        }

        private void resizingForm()
        {
            this.listProcessView.Size = this.calculateMaxSize(this.listProcessView);
            this.MaximumSize = new Size(ControllerSystemInfo.getSystemInfo(ControllerSystemInfo.SystemMetric.SM_CXFULLSCREEN), ControllerSystemInfo.getSystemInfo(ControllerSystemInfo.SystemMetric.SM_CYFULLSCREEN));
        }

        private void ProcessListForm_Load(object sender, EventArgs e)
        {
            //this.listProcessView.Size = this.Size;
            this.resizingForm();            
        }

        private void ProcessListForm_SizeChanged(object sender, EventArgs e)
        {
            //this.listProcessView.Size = this.Size;
        }
    }
}
