using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegados
{
    public delegate void EjecutarDlg(string UnTexto);

    public class Deposito
    {
        public EjecutarDlg DelegadoEjecutar;

        public event EjecutarDlg EventoEjecutar;
        public void EjecutarLosMetodosEnLosDelegados(string UnTexto)
        {
            if(this.DelegadoEjecutar != null)
            {
            this.DelegadoEjecutar(UnTexto);
            }
        }
        public void DispararElEvento(string UnTexto)
        {
            if (this.EventoEjecutar != null)
            {
                this.EventoEjecutar(UnTexto);
            }
        }
    }
}
