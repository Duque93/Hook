using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Utils;
using WindowsFormsApplication1.Classes.Utils;
using static WindowsFormsApplication1.Classes.Utils.Utilidades.TCP_TABLE_CLASS;
using static WindowsFormsApplication1.Classes.Utils.Utilidades.UDP_TABLE_CLASS;

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
            if (instance == null)
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
                this.Text = "Detalles de " + proc.MainModule.ModuleName + "(" + proc.Id + ") - " + proc.MachineName;
                this.labelBasePriorityData.Text += "" + proc.BasePriority;
                this.labelHandlersData.Text += "0x" + proc.Handle.ToInt64().ToString("X") + "  Handlers abiertos : " + proc.HandleCount;
                //http://pastebin.com/f3eda7c8
                //stdout = GetStdHandle(STD_OUTPUT_HANDLE);
                //stderr = GetStdHandle(STD_ERROR_HANDLE);

                ////////////////////////////////////////////////////////////////////////////////////////////////////
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
                foreach (ProcessModule procModule in proc.Modules)
                {
                    ListViewItem item = Utilidades.createItem("ItemModule" + procModule.ModuleName, procModule, procModule.ModuleName, procModule.FileName, "0x" + procModule.EntryPointAddress.ToInt64().ToString("X"), "0x" + procModule.BaseAddress.ToInt64().ToString("X"), procModule.ModuleMemorySize.ToString());
                    this.listViewModules.Items.Add(item);
                }
                ////////////////////////////////////////////////////////////////////////////////////////////////////

                ////////////////////////////////////////////////////////////////////////////////////////////////////
                this.listViewNetwork.HeaderStyle = ColumnHeaderStyle.Nonclickable;
                this.listViewNetwork.View = View.Details;
                this.listViewNetwork.MouseWheel += new MouseEventHandler(
                    (object sender, MouseEventArgs e) =>
                    {
                        this.OnMouseWheel(e);
                    }
                    );
                this.listViewNetwork.Columns.Add(Utilidades.createCabecera("headerTipo", "Tipos"));
                this.listViewNetwork.Columns.Add(Utilidades.createCabecera("headerLocalAdress", "Local Address"));
                this.listViewNetwork.Columns.Add(Utilidades.createCabecera("headerLocalPort", "Puerto local"));
                this.listViewNetwork.Columns.Add(Utilidades.createCabecera("headerRemoteAddress", "Remote Address"));
                this.listViewNetwork.Columns.Add(Utilidades.createCabecera("headerRemotePort", "Puerto remoto"));
                this.listViewNetwork.Columns.Add(Utilidades.createCabecera("headerState", "Estado"));

                List<TCPROW_OWNER_PID> listaTCP = Utilidades.GetConnections<Utilidades.TCP_TABLE_CLASS, TCPROW_OWNER_PID>(Utilidades.IPv4);
                int i = 0;
                foreach (TCPROW_OWNER_PID fila in listaTCP)
                {
                    if(fila.owningPid == proc.Id)
                    {
                        ListViewItem item = Utilidades.createItem("ItemNetworkTCP" + i, fila, "TCP", fila.LocalAddress.ToString(), fila.LocalPort.ToString(), fila.RemoteAddress.ToString(), fila.RemotePort.ToString(), fila.State);
                        this.listViewNetwork.Items.Add(item);
                        i++;
                    }
                   
                }
                List<UDPROW_OWNER_PID> listaUDP = Utilidades.GetConnections<Utilidades.UDP_TABLE_CLASS, Utilidades.UDP_TABLE_CLASS.UDPROW_OWNER_PID>(Utilidades.IPv4);
                i = 0;
                foreach (UDPROW_OWNER_PID fila in listaUDP)
                {
                    if (fila.owningPid == proc.Id)
                    {
                        ListViewItem item = Utilidades.createItem("ItemNetworkUDP" + i, fila, "UDP", fila.LocalAddress.ToString(), fila.LocalPort.ToString(), "-", "-", "-");
                        this.listViewNetwork.Items.Add(item);
                        i++;
                    }

                }
            }
        }

        private void resizingForm()
        {
            Size maxSize = Utilidades.calculateMaxSize(this.listViewModules);
            this.tabModulos.Size = maxSize;
            this.listViewModules.Size = maxSize;
            //this.listViewNetwork.Size = maxSize;
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

 
    }
}
