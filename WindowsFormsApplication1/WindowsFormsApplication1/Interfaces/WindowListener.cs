using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverlayDrawingTest
{
    /// <summary>
    /// Se encarga de mantener la comunicacion entre 2 clases, una de ellas un Form
    /// Ahora mismo solo avisa de cuando se esta cerrando el formulario ya que es lo unico que sirve actualmente
    /// </summary>
    public interface IWindowListener
    {
        void closingWindow();
    }
}
