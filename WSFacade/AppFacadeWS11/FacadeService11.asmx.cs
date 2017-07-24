using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.ServiceModel;
using System.Xml.Serialization;
using System.Xml;
using System.Web.Services.Protocols;
using System.ServiceModel.Channels;
using System.Text;
using System.Collections;

namespace AppFacadeWS11
{
    /// <summary>
    /// Summary description for FacadeService11
    /// </summary>
    [WebService(Namespace = "http://localhost/FacadeService11")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class FacadeService11 : System.Web.Services.WebService
    {       

        [WebMethod]
        public ReponseDatosClienteMDM recuperarDatosBasicosMDM(bool bUseSSL, string urlServicio, string sSystemId, string sMessageId, string sUserNameId, string sName, string sNamespace, string sOperation, string sTipoDocumento, string sNumeroDoc)
        {
            try
            {
                RecuperarDatosClienteMDM.RecuperarDatosBasicosClienteResponse datosSalida = new RecuperarDatosClienteMDM.RecuperarDatosBasicosClienteResponse();
                ReponseDatosClienteMDM response = new ReponseDatosClienteMDM();

                BasicHttpBinding binding = new BasicHttpBinding();
                if (bUseSSL)
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


                }

                return response;
            }
                       
            catch (FaultException<RecuperarDatosClienteMDM.ClienteNoExisteBusinessException> excNegocio)
            {
                ReponseDatosClienteMDM recuperarDatosBasicosClienteResponse = new ReponseDatosClienteMDM();
                recuperarDatosBasicosClienteResponse.faultCode = excNegocio.Detail.genericException.code;
                recuperarDatosBasicosClienteResponse.faultDescription = excNegocio.Detail.genericException.description;
                return recuperarDatosBasicosClienteResponse;
            }
            catch (FaultException<RecuperarDatosClienteMDM.SystemException> exsys)
            {
                ReponseDatosClienteMDM recuperarDatosBasicosClienteResponse = new ReponseDatosClienteMDM();
                recuperarDatosBasicosClienteResponse.faultCode = exsys.Detail.genericException.code;
                recuperarDatosBasicosClienteResponse.faultDescription = exsys.Detail.genericException.description + ". Facade";
                return recuperarDatosBasicosClienteResponse;
            }
            catch (Exception ex)
            {
                ReponseDatosClienteMDM recuperarDatosBasicosClienteResponse = new ReponseDatosClienteMDM();
                recuperarDatosBasicosClienteResponse.faultCode = "999";
                recuperarDatosBasicosClienteResponse.faultDescription = ex.Message + ". Facade";
                return recuperarDatosBasicosClienteResponse;
            }
            
        }

        [WebMethod]
        public ReponseDatosClienteHUB recuperarDatosBasicosHUB(bool bUseSSL, string urlServicio, string sSystemId, string sMessageId, string sUserNameId, string sName, string sNamespace, string sOperation, string sTipoDocumento, string sNumeroDoc)
        {
            try
            {
                RecuperarDatosClienteHUB.RecuperarDatosBasicosClienteResponse datosSalida = new RecuperarDatosClienteHUB.RecuperarDatosBasicosClienteResponse();
                ReponseDatosClienteHUB response = new ReponseDatosClienteHUB();

                BasicHttpBinding binding = new BasicHttpBinding();
                if (bUseSSL)
                {
                    binding.Security = new BasicHttpSecurity();
                    binding.Security.Mode = BasicHttpSecurityMode.Transport;
                }

                //Specify the address to be used for the client.
                EndpointAddress address = new EndpointAddress(urlServicio);

                RecuperarDatosClienteHUB.RecuperarDatosClienteClient recuperarDatosBasicos = new RecuperarDatosClienteHUB.RecuperarDatosClienteClient(binding, address);

                RecuperarDatosClienteHUB.RequestHeader requestHeader = new RecuperarDatosClienteHUB.RequestHeader();

                //Se guarda en el Header
                requestHeader.systemId = sSystemId;
                requestHeader.messageId = sMessageId;
                requestHeader.userId = new RecuperarDatosClienteHUB.UsernameToken();
                requestHeader.userId.userName = sUserNameId;
                requestHeader.destination = new RecuperarDatosClienteHUB.Destination();
                requestHeader.destination.name = sName;
                requestHeader.destination.@namespace = sNamespace;
                requestHeader.destination.operation = sOperation;

                //Se guarda en el body
                RecuperarDatosClienteHUB.RecuperarDatosBasicosCliente datosEntrada = new RecuperarDatosClienteHUB.RecuperarDatosBasicosCliente();

                if (sNumeroDoc != null && sNumeroDoc != "" && sNumeroDoc != string.Empty && sTipoDocumento != null && sTipoDocumento != "" && sTipoDocumento != string.Empty)
                {
                    datosEntrada.identificacionCliente = new RecuperarDatosClienteHUB.IdentificacionCliente();
                    datosEntrada.identificacionCliente.tipoDocumento = sTipoDocumento;
                    datosEntrada.identificacionCliente.numeroDocumento = sNumeroDoc;
                }

                recuperarDatosBasicos.recuperarDatosBasicosCliente(requestHeader, datosEntrada, out datosSalida);


                if (datosSalida != null)
                {
                    
                    //Cargo datos para persona Juridica
                    if (datosSalida.cliente.Item.GetType().Name == "PersonaJuridica")
                    {
                        RecuperarDatosClienteHUB.PersonaJuridica personaJuridica = (RecuperarDatosClienteHUB.PersonaJuridica)datosSalida.cliente.Item;
                        response.razonSocial = personaJuridica.razonSocial;

                    }
                    //Cargo datos para persona Natural
                    if (datosSalida.cliente.Item.GetType().Name == "PersonaNatural")
                    {
                        RecuperarDatosClienteHUB.PersonaNatural personaNatural = (RecuperarDatosClienteHUB.PersonaNatural)datosSalida.cliente.Item;
                        response.nombreCompleto = personaNatural.nombreLargoCliente;

                    }


                    //Cargo el resto de los datos
                    response.codigoSector = datosSalida.codigoSector;
                    response.codigoSubSector = datosSalida.codigoSubSector;
                    response.codigoCIIU = datosSalida.codigoCIIU;
                    response.codigoRegion = datosSalida.codigoRegion;
                    response.codigoSegmento = datosSalida.codigoSegmento;
                    response.codigoSubSegmento = datosSalida.codigoSubSegmento;
                    //Cargo datos para Gerente Comercial
                    if (datosSalida.gerenteComercial != null)
                    {
                        response.nombreLargo = datosSalida.gerenteComercial.nombreLargo;
                        response.codigo = datosSalida.gerenteComercial.codigo;
                    }
                    response.codigoRol = datosSalida.codigoRol;
                    response.fechaEstadoVinculacion = datosSalida.fechaEstadoVinculacion;
                    response.fechaUltimaActualizacion = datosSalida.fechaUltimaActualizacion;
                    response.estadoVinculacion = datosSalida.estadoVinculacion;


                }

                return response;
            }

            catch (FaultException<RecuperarDatosClienteHUB.ClienteNoExisteBusinessException> excNegocio)
            {
                ReponseDatosClienteHUB recuperarDatosBasicosClienteResponse = new ReponseDatosClienteHUB();
                recuperarDatosBasicosClienteResponse.faultCode = excNegocio.Detail.genericException.code;
                recuperarDatosBasicosClienteResponse.faultDescription = excNegocio.Detail.genericException.description;
                return recuperarDatosBasicosClienteResponse;
            }
            catch (FaultException<RecuperarDatosClienteHUB.SystemException> exsys)
            {
                ReponseDatosClienteHUB recuperarDatosBasicosClienteResponse = new ReponseDatosClienteHUB();
                recuperarDatosBasicosClienteResponse.faultCode = exsys.Detail.genericException.code;
                recuperarDatosBasicosClienteResponse.faultDescription = exsys.Detail.genericException.description + ". Facade";
                return recuperarDatosBasicosClienteResponse;
            }
            catch (Exception ex)
            {
                ReponseDatosClienteHUB recuperarDatosBasicosClienteResponse = new ReponseDatosClienteHUB();
                recuperarDatosBasicosClienteResponse.faultCode = "999";
                recuperarDatosBasicosClienteResponse.faultDescription = ex.Message + ". Facade";
                return recuperarDatosBasicosClienteResponse;
            }

        }

    }
}
