using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfService
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IService2" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    public interface IService2
    {
        [OperationContract]
        ClaseRespuesta ConvierteEnMayusculas(string texto);
    }

    public class ClaseRespuesta
    {
        public string TextoOriginal { get; set; }
        public string Resultado { get; set; }
        public ClaseRespuesta(string textoOriginal, string resultado)
        {
            TextoOriginal = textoOriginal;
            Resultado = resultado;
        }
    }
}
