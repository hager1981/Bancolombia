using AppFacadeWS11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Clientes
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                clienteMDM();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Message:"+ex.Message);
                Console.WriteLine("InnerException:" + ex.InnerException.Message);
                Console.ReadLine();
            }
        }

        private static void clienteMDM()
        {
            
            Console.WriteLine("bUseSSL:");
            var bUseSSL = Console.ReadLine();
            Console.WriteLine("urlServicio:");
            var urlServicio = Console.ReadLine();
            Console.WriteLine("sSystemId:");
            var sSystemId = Console.ReadLine();
            Console.WriteLine("sMessageId:");
            var sMessageId = Console.ReadLine();
            Console.WriteLine("sUserNameId:");
            var sUserNameId = Console.ReadLine();
            Console.WriteLine("sName:");
            var sName = Console.ReadLine();
            Console.WriteLine("sNamespace:");
            var sNamespace = Console.ReadLine();
            Console.WriteLine("sOperation:");
            var sOperation = Console.ReadLine();
            Console.WriteLine("sTipoDocumento:");
            var sTipoDocumento = Console.ReadLine();
            Console.WriteLine("sNumeroDoc:");
            var sNumeroDoc = Console.ReadLine();

            RecuperarDatosClienteMDM.RecuperarDatosBasicosClienteResponse datosSalida = new RecuperarDatosClienteMDM.RecuperarDatosBasicosClienteResponse();
            ReponseDatosClienteMDM response = new ReponseDatosClienteMDM();

            BasicHttpBinding binding = new BasicHttpBinding();
            if (Convert.ToBoolean(bUseSSL))
            {
                binding.Security = new BasicHttpSecurity();
                binding.Security.Mode = BasicHttpSecurityMode.Transport;
            }

            //Specify the address to be used for the client.
            EndpointAddress address = new EndpointAddress(urlServicio);

            RecuperarDatosClienteMDM.RecuperarDatosClienteClient recuperarDatosBasicos = new RecuperarDatosClienteMDM.RecuperarDatosClienteClient(binding, address);

            RecuperarDatosClienteMDM.RequestHeader requestHeader = new RecuperarDatosClienteMDM.RequestHeader();

            //Se guarda en el Header
            requestHeader.systemId = sSystemId;
            requestHeader.messageId = sMessageId;
            requestHeader.userId = new RecuperarDatosClienteMDM.UsernameToken();
            requestHeader.userId.userName = sUserNameId;
            requestHeader.destination = new RecuperarDatosClienteMDM.Destination();
            requestHeader.destination.name = sName;
            requestHeader.destination.@namespace = sNamespace;
            requestHeader.destination.operation = sOperation;

            //Se guarda en el body
            RecuperarDatosClienteMDM.RecuperarDatosBasicosCliente datosEntrada = new RecuperarDatosClienteMDM.RecuperarDatosBasicosCliente();

            if (sNumeroDoc != null && sNumeroDoc != "" && sNumeroDoc != string.Empty && sTipoDocumento != null && sTipoDocumento != "" && sTipoDocumento != string.Empty)
            {
                datosEntrada.ItemElementName = RecuperarDatosClienteMDM.ItemChoiceType.identificacionCliente;
                datosEntrada.Item = new RecuperarDatosClienteMDM.IdentificacionCliente()
                {
                    tipoDocumento = sTipoDocumento,
                    numeroDocumento = sNumeroDoc,
                };
            }
            recuperarDatosBasicos.recuperarDatosBasicosCliente(requestHeader, datosEntrada, out datosSalida);

            if (datosSalida != null)
            {

                //Cargo datos para persona Juridica
                if (datosSalida.cliente.Item.GetType().Name == "PersonaJuridica")
                {
                    RecuperarDatosClienteMDM.PersonaJuridica personaJuridica = (RecuperarDatosClienteMDM.PersonaJuridica)datosSalida.cliente.Item;
                    response.razonSocial = personaJuridica.razonSocial;

                }
                //Cargo datos para persona Natural
                if (datosSalida.cliente.Item.GetType().Name == "PersonaNatural")
                {
                    RecuperarDatosClienteMDM.PersonaNatural personaNatural = (RecuperarDatosClienteMDM.PersonaNatural)datosSalida.cliente.Item;
                    response.nombreCompleto = personaNatural.nombreLargoCliente;

                }

                //Cargo el resto de los datos
                response.codigoSector = datosSalida.codigoSector;
                response.codigoSubSector = datosSalida.codigoSubSector;
                response.codigoCIIU = datosSalida.codigoCIIU;
                response.codigoRegion = datosSalida.codigoRegion;
                response.codigoSegmento = datosSalida.codigoSegmento;
                response.codigoSubSegmento = datosSalida.codigoSubSegmento;
                response.codigoRol = datosSalida.codigoRol;
                response.fechaEstadoVinculacion = datosSalida.fechaEstadoVinculacion;
                response.fechaUltimaActualizacion = datosSalida.fechaUltimaActualizacion;

                Console.WriteLine("codigoSubSector" + response.codigoSubSector);
            }
            System.Console.Read();
        }
    }
}
