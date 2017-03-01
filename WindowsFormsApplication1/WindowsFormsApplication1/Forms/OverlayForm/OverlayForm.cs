

using Hook;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using static Hook.WindowHook;

namespace OverlayDrawingTest
{
    public partial class OverlayForm : Form
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listener">Se ocupara de informar al padre de los cambios de estado en el overlay</param>
        public OverlayForm(IComunicator chivatoPadre, IntPtr handlerTarget)
        {
            InitializeComponent();
            this.chivatoPadre = chivatoPadre;
            this.handlerTarget = handlerTarget;
            
        }

        #region Methods
        public void init()
        {
            if (working) throw new Exception("La clase ya ha sido inicializada");
            working = true;
            lapiz = new Pen(Color.Red);

            IWindowEventListener escuchadorTeclado = WindowEventListener.getInstance(this);
            hookWindow = new WindowHook(this.handlerTarget, escuchadorTeclado); //Bug, simplemente no tengo permiso para instalar un hook en un proceso especifico.
            

            /*
            new Thread //Este hilo en segundo plano se encarga de ir consultando el tamaño de la ventana objetivo y en caso de existir modificación, notificarnoslo
                (
                    () =>
                    {
                        RECT rect;
                        Thread.CurrentThread.IsBackground = true;
                        Thread.CurrentThread.Name = "movedWindowListener";
                        while (this.working)
                        {
                            GetWindowRect(this.handlerTarget, out rect);
                            if (this.ctrlTargetRect != rect)
                            {
                                this.ctrlTargetRect = rect;
                                this.adjust();
                            }
                            Thread.Sleep(500);
                        }
                    }
                ).Start();
              */
        }
        

        /// <summary>
        /// Se ajusta a la posición y tamaño de la ventana objetivo
        /// </summary>
        private void adjust()
        {
            this.Size = new Size(ctrlTargetRect.right - ctrlTargetRect.left, ctrlTargetRect.bottom - ctrlTargetRect.top); //Establecemos el overlay del mismo tamaño qe la ventana objetivo
            this.setWindowPos( ctrlTargetRect.top , ctrlTargetRect.left);
        }
        /// <summary>
        /// Desplaza la posición del overlay en coordenadas x,y de la pantalla
        /// </summary>
        /// <param name="top">Posicion eje Y</param>
        /// <param name="left">Posicion eje X</param>
        private void setWindowPos(int top, int left)
        {
            if (this.InvokeRequired) //Esto libera el recurso para que otro proceso lo modifique
            {
                //Una tonteria pero de esta manera es posible controlar cuando un proceso externo accede a modificar propiedades de un objeto manejado
                //por otro proceso ya que ahora se le avisa de que otro le va a modificar el objeto. Antes no tenia manera de darse cuenta
                //Es semejante al synchronized de JAVA.
                SetIntsCallback d = setWindowPos; //Es necesario crear un objeto de tipo delegate para lo siguiente:
                this.Invoke(d,  top, left ); //Llama de manera procedural a una función dentro de la clase.1º arg-->callback,2ºarg-->argumentos
            }
            else
            {
                this.Top = top;
                this.Left = left;
            }
        }

        private void DrawMenu()
        {

        }
        
        #endregion Methods

        #region EventsForm

        private void Overlay_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.Black; //Seteamos el color de fondo a negro.
            this.TransparencyKey = Color.Black; //Indicamos que todo lo que se encuentre en negro dentro del formulario sea transparente.
            
            this.TopMost = true; //Hacemos que el Overlay este siempre por delante de todo

            //this.Opacity = 1;
            //Desactivado mientras desarrollamos :
            //if( !OverlayForm.DEBUG )
            this.FormBorderStyle = FormBorderStyle.None; // Quitamos todos los bordes, la cabecera de la ventana incluida

            this.ctrlTargetRect = hookWindow.GetWindowRect(this.handlerTarget);


        }

        private void Overlay_FormClosing(object sender, FormClosingEventArgs e)
        {
            working = false;
            chivatoPadre.closingWindow();
        }

        private void OverlayForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle( lapiz , 100,100,200, 200);
            Font fuente = new Font(FontFamily.GenericSansSerif, 12f);
            Brush pincel = new SolidBrush(Color.Yellow);
            e.Graphics.DrawString("Hello World!!!", fuente, pincel , 0, 0);

            //e.Graphics.Clear(Color.Black); Para limpiar todo
        }

        #endregion EventsForm
        
    }
}
