using Hook;
using Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using WindowsFormsApplication1.Forms;

namespace OverlayDrawingTest
{
    public partial class Formulario : Form
    {

        public Formulario()
        {
            //chivatoHijos = new List<IComunicator>();
            InitializeComponent();
        }

        #region privateMethods

        private void initOverlay()
        {

            //IComunicator chivato = Comunicator.getInstance(this, true);
            //chivatoHijos.Add(chivato);
            onChildClose += new System.EventHandler(
                (object sender, EventArgs e) => { this.checkBoxOverlay.Checked = false; });
            overlayProcess = new OverlayForm(onChildClose, handlerFounded);
            overlayProcess.init();
        }
        private void init()
        {
            ptrCallbackLoadingForm = loadingForm;
            ptrCallbackAppendText = appendText;

            IKeyBoardEventsListener escuchadorTeclado = KeyBoardEventsListener.getInstance(this);
            hookTeclado = KeyBoardHook.getInstance(escuchadorTeclado);

            IMouseEventsListener escuchadorRaton = MouseEventListener.getInstance(this);
            hookRaton =  MouseHook.getInstance(escuchadorRaton);

            onChildClose += new System.EventHandler(
                (object sender, EventArgs e) => { this.Close(); });
            listaProcesosForm = new ProcessListForm(onChildClose);
            //listaProcesosForm.Show();
            /*
            foreach ( Process proc in Process.GetProcesses() ){
                try
                {
                    using (ProcessModule curModule = proc.MainModule)
                    {
                        Trace.WriteLine(curModule.FileName);
                    }
                }
                catch(Win32Exception e)
                {
                    Trace.WriteLine("Ha ocurrido un error intentando leer un proceso. Error("+e.NativeErrorCode+") = "+e.Message);
                }
                
            }*/
            //this.overlayDebugging = new OverlayForm(null, IntPtr.Zero);
        }
        private void findWindow(string windowName, int secondsToWait)
        {
            richTextBox_Debug.ResetText();
            richTextBox_Debug.AppendText("------------------------------------------------\n");
            IntPtr hWnd = FindWindow(null, windowName);
            this.Cursor = Cursors.WaitCursor;
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                int seconds = 1;
                loadingForm(false);
                while (seconds != secondsToWait && hWnd == IntPtr.Zero)
                {
                    System.Threading.Thread.Sleep(1000);
                    seconds += 1;
                    this.appendText(richTextBox_Debug, "Searching process with window title --> " + windowName);
                    hWnd = FindWindow(null, windowName);
                    if (hWnd != IntPtr.Zero) break;
                }
                if (hWnd != IntPtr.Zero)
                {
                    this.appendText(richTextBox_Debug, "Se ha encontrado un proceso con nombre -->" + windowName);
                    this.handlerFounded = hWnd;
                }
                else this.appendText(richTextBox_Debug, "NO se ha encontrado un proceso con nombre -->" + windowName);
                loadingForm(true);

                buttonBuscar_Click(Thread.CurrentThread, null);
            }).Start();
            
        }

        private void loadingForm(bool end)
        {
            if (this.InvokeRequired) this.Invoke(ptrCallbackLoadingForm, end );
            else
            {
                if (!end) //comienza a cargar
                {
                    this.Cursor = Cursors.WaitCursor;
                    this.Enabled = false;
                }
                else //ha terminado
                {
                    this.Cursor = Cursors.Default;
                    this.Enabled = true;
                }
            }
        }

        private void appendText(RichTextBox textarea,string contenido)
        {
            if (this.InvokeRequired) this.Invoke(ptrCallbackAppendText, textarea, contenido);
            else
            {
                textarea.AppendText(contenido+"\n");
                textarea.ScrollToCaret();
            }
        }
        #endregion privateMethods


        #region ControlEvents

        private void checkBoxOverlay_CheckedChanged(object sender, EventArgs e)
        {
            this.buttonBuscar.Enabled = false;
            if (this.checkBoxOverlay.Checked)
            {
                if (overlayProcess.IsDisposed) initOverlay();
                overlayProcess.Show();
            }
            else
            {
                overlayProcess.Hide();
            }
        }
      
        private void buttonBuscar_Click(object sender, EventArgs e)
        {
            if( ! (sender is Thread) ) findWindow(this.textBoxWindowNameTarget.Text, 5);
            else //Significa que estamos llamandolo desde el hilo buscador
            {
                if (this.InvokeRequired) this.Invoke(new System.EventHandler(this.buttonBuscar_Click), Thread.CurrentThread, null);
                else
                {
                    if (handlerFounded != IntPtr.Zero)
                    {
                        this.checkBoxOverlay.Visible = true;
                        MessageBox.Show("Se ha encontrado el proceso con titulo ' " + this.textBoxWindowNameTarget.Text + " ' ");
                        initOverlay();
                    }
                    else this.checkBoxOverlay.Visible = false;
                }
                
            }
           
        }


        private void textBoxWindowNameTarget_TextChanged(object sender, EventArgs e)
        {
            this.checkBoxOverlay.Visible = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.init();
        }

        private void Formulario_FormClosing(object sender, FormClosingEventArgs e)
        {
            if( hookTeclado != null ) hookTeclado.deleteHook();
            if( hookRaton != null ) hookRaton.deleteHook();
        }
        #endregion ControlEvents
    }
}
