using Hook;
using Interfaces;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace OverlayDrawingTest
{
    public partial class Formulario : Form
    {

        public Formulario()
        {
            InitializeComponent();
        }

        #region privateMethods

        private void initOverlay()
        {
            comunicador = WindowListener.getInstance(this);
            overlay = new OverlayForm(comunicador, handlerFounded);
        }
        private void init()
        {
            ptrCallbackLoadingForm = loadingForm;
            ptrCallbackAppendText = appendText;

            IKeyBoardEventsListener escuchadorTeclado = KeyBoardEventsListener.getInstance(this);
            hookTeclado = KeyBoardHook.getInstance(escuchadorTeclado);

            IMouseEventsListener escuchadorRaton = MouseEventListener.getInstance(this);
            hookRaton =  MouseHook.getInstance(escuchadorRaton);
        }
        private IntPtr findWindow(string windowName, int secondsToWait)
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
                    
                }
                if(hWnd != IntPtr.Zero) this.appendText(richTextBox_Debug, "Se ha encontrado un proceso con nombre -->" + windowName);
                else this.appendText(richTextBox_Debug, "NO se ha encontrado un proceso con nombre -->" + windowName);
                loadingForm(true);
            }).Start();
            
            return hWnd;
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
                if (overlay.IsDisposed) initOverlay();
                overlay.Show();
            }
            else
            {
                overlay.Hide();
            }
        }
      
        private void buttonBuscar_Click(object sender, EventArgs e)
        {
            handlerFounded = findWindow(this.textBoxWindowNameTarget.Text, 5);
            if (handlerFounded != IntPtr.Zero)
            {
                this.checkBoxOverlay.Visible = true;
                MessageBox.Show("Se ha encontrado el proceso con titulo ' "+ this.textBoxWindowNameTarget.Text +" ' " );
                initOverlay();
            }
            else this.checkBoxOverlay.Visible = false;
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
