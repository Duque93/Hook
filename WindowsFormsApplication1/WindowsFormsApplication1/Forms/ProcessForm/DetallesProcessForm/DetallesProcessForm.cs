using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils;
using WindowsFormsApplication1.Classes.Utils;

namespace WindowsFormsApplication1.Forms.ProcessForm.DetallesProcessForm
{
    public partial class DetallesProcessForm : Form
    {
        private Process proc = null;
        private delegate void ptrInit();
        private delegate void ptrAppendText(RichTextBox areaTextBox, string text);
        private static DetallesProcessForm instance = null;

        private IntPtr stdout;
        private IntPtr stderr;

        private bool working = false;

        public static DetallesProcessForm getInstance(Process proc)
        {
            if(instance == null )
            {
                instance = new DetallesProcessForm(proc);
            }
            else 
            {
                if (!instance.IsDisposed)  //Siempre que se llame a getInstance, si ya hay uno abierto, lo cerraremos y crearemos uno nuevo
                {
                    instance.Close();
                }
                instance = new DetallesProcessForm(proc);
            }
            return instance;
        }

        private DetallesProcessForm(Process proc)
        {
            this.proc = proc;

            InitializeComponent();

            init();
        }

        private void init()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new ptrInit(init));
            }
            else
            {
                working = true;
                this.Text = "Detalles de " + proc.MainModule.ModuleName + "(" + proc.Id + ") - "+proc.MachineName;
                this.labelBasePriorityData.Text += "" + proc.BasePriority;
                this.labelHandlersData.Text += "0x" + proc.Handle.ToInt64().ToString("X") + "  Handlers abiertos : " + proc.HandleCount;

                //http://pastebin.com/f3eda7c8
                //stdout = GetStdHandle(STD_OUTPUT_HANDLE);
                //stderr = GetStdHandle(STD_ERROR_HANDLE);
                

                this.listViewModules.HeaderStyle = ColumnHeaderStyle.Nonclickable;
                this.listViewModules.View = View.Details;
                this.listViewModules.MouseWheel += new MouseEventHandler(
                    (object sender, MouseEventArgs e) =>
                    {
                        this.OnMouseWheel(e);
                    }
                    );
                
                this.listViewModules.Columns.Add(Utilidades.createCabecera("headerModulesName", "Modulo"));
                this.listViewModules.Columns.Add(Utilidades.createCabecera("headerPath", "Ruta"));
                this.listViewModules.Columns.Add(Utilidades.createCabecera("headerBaseAddress", "Entry Point Address"));
                this.listViewModules.Columns.Add(Utilidades.createCabecera("headerBaseAddress", "Base Address"));
                this.listViewModules.Columns.Add(Utilidades.createCabecera("headerMemorySize", "Tamaño memoria"));

                foreach ( ProcessModule procModule in proc.Modules)
                {
                    ListViewItem item = Utilidades.createItem("IteModule" + procModule.ModuleName, procModule, procModule.ModuleName, procModule.FileName, "0x" + procModule.EntryPointAddress.ToInt64().ToString("X"), "0x" + procModule.BaseAddress.ToInt64().ToString("X"), procModule.ModuleMemorySize.ToString());
                    this.listViewModules.Items.Add(item);
                }

            }
        }

        private void resizingForm()
        {
            Size maxSize = Utilidades.calculateMaxSize(this.listViewModules);
            this.tabModulos.Size = maxSize;
            this.listViewModules.Size = maxSize;
            this.MaximumSize = new Size(ControllerSystemInfo.getSystemInfo(ControllerSystemInfo.SystemMetric.SM_CXFULLSCREEN), ControllerSystemInfo.getSystemInfo(ControllerSystemInfo.SystemMetric.SM_CYFULLSCREEN));
        }

        private void AppendText(RichTextBox areaTextBox, string text)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new ptrAppendText(AppendText), areaTextBox, text);
            }
            else
            {
                areaTextBox.AppendText(text);
            }
        }

        private void DetallesProcessForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            working = false;
        }

        private const int STD_OUTPUT_HANDLE = -11;
        private const int STD_ERROR_HANDLE = -12;
        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetStdHandle(int nStdHandle, IntPtr hHandle);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetStdHandle(int nStdHandle);
    }
}
