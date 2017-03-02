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
using WindowsFormsApplication1.Classes.Utils;
using WindowsFormsApplication1.Forms.ProcessForm.DetallesProcessForm;

namespace WindowsFormsApplication1.Forms
{
    public partial class ProcessListForm : Form
    {

        EventHandler onClose = null;

        public ProcessListForm(EventHandler onClose)
        {
            this.onClose = onClose;
            InitializeComponent();


            this.init();
            
            this.Show();
        }

        private void init()
        {
            this.listProcessView.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            this.listProcessView.View = View.Details;
            this.listProcessView.MouseWheel += new MouseEventHandler( 
                (object sender, MouseEventArgs e) =>
                {
                    this.OnMouseWheel(e);
                } 
                ) ;
            
            this.listProcessView.Columns.Add(Utilidades.createCabecera("headerPiD", "PiD"));
            this.listProcessView.Columns.Add(Utilidades.createCabecera("headerProcessName", "Proceso"));
            this.listProcessView.Columns.Add(Utilidades.createCabecera("headerPath", "Ruta"));
            this.listProcessView.Columns.Add(Utilidades.createCabecera("headerBaseAddress", "Entry Point Address"));
            this.listProcessView.Columns.Add(Utilidades.createCabecera("headerBaseAddress", "Base Address"));

            int errors = 0;
            foreach (Process proc in Process.GetProcesses())
            {
                try
                {
                    using (ProcessModule procModule = proc.MainModule)
                    {
                        ListViewItem item = Utilidades.createItem("ItemProc" + proc.Id, proc, proc.Id.ToString(), procModule.ModuleName, procModule.FileName, "0x" + procModule.EntryPointAddress.ToInt64().ToString("X"), "0x" + procModule.BaseAddress.ToInt64().ToString("X") );
                        this.listProcessView.Items.Add(item);
                        proc.Exited += new EventHandler(
                            (object sender, EventArgs e) =>
                            {
                                item.Remove();
                            }
                            );
                       
                    }
                }
                catch (Win32Exception e)
                {
                    errors += 1;
                    this.listProcessView.Items.Add(Utilidades.createErrorItem("ItemProcErrorNum" + errors, e, e.ErrorCode.ToString(), "Modulo Virtual", "System.dll", "0x0", "0x0"));
                    Trace.WriteLine("Codigo error = " + e.NativeErrorCode + " Descripción = " + e.Message);
                    Trace.WriteLine(e.ToString());
                }
            }

            this.listProcessView.ItemActivate += new System.EventHandler(
                (object sender, EventArgs e) =>
                {
                    ListViewItem item = ((ListView)sender).SelectedItems[0];
                    Trace.WriteLine("Se ha hecho click al item con Name = " + item.Name);
                    if (!item.Name.Contains("ItemProcErrorNum"))
                    {
                        Process proc = (Process)item.Tag;
                        ProcessModule procModule = proc.MainModule;

                        //Actualizamos el item seleccionado
                        ListViewItem updatedItem = Utilidades.createItem("ItemProc" + proc.Id, proc, proc.Id.ToString(), procModule.ModuleName, procModule.FileName, "0x" + procModule.EntryPointAddress.ToInt64().ToString("X"), "0x" + procModule.BaseAddress.ToInt64().ToString("X"));
                        item.Text = updatedItem.Text;
                        DetallesProcessForm.getInstance(proc).Show();
                    }
                   
                }
                    );
        }

        private void resizingForm()
        {
            this.listProcessView.Size = Utilidades.calculateMaxSize(this.listProcessView);
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

        private void ProcessListForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.onClose(sender, e);
        }
    }
}
