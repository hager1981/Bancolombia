using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace AppFacadeWS11
{
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ReponseDatosClienteMDM
    {
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public class Cliente
        {
            //public object item;

            public string tipoPersona;
        }

        public Cliente cliente;

        public string codigoSegmento;

        public string codigoSubSegmento;

        public string razonSocial;

        public string nombreCompleto;

        public string codigoCIIU;

        public string codigoRol;

        public System.DateTime fechaEstadoVinculacion;

        public System.DateTime fechaUltimaActualizacion;

        public string codigoRegion;

        public string codigoSector;

        public string codigoSubSector;

        public string faultCode { get; set; }

        public string faultDescription { get; set; }

        /*
        public string identificadorClienteBP;

        public string llaveMDM;

        public string llaveCIF;                

        public string codigoSubCIIU;        

        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public class GerenteMonedaExtranjera
        {

            private string nombreLargo;

            private string codigo;

            private string numeroDocumentoIdentidad;
        }

        public Resultado[] Resultados;
        public Fuente[] Fuentes;
        

        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public class IdentificacionCliente
        {

            public string tipoDocumento;

            public string numeroDocumento;
        }

        public GerenteMonedaExtranjera gerenteMonedaExtranjera;

        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class GerenteComercial
        {

            private string nombreLargo;

            private string codigo;
        }

        public GerenteComercial gerenteComercial;

        public IdentificacionCliente identificacionCliente;

        public string estadoCliente;

        public string estadoVinculacion;        

        public bool fechaEstadoVinculacionSpecified;

        public System.DateTime fechaCambioEstado;

        public bool fechaCambioEstadoSpecified;        

        public string codigoOficina;
        

        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class InformacionExpedicionIdentificacion
        {

            private System.DateTime fechaExpedicion;

            private bool fechaExpedicionSpecified;

            private string codigoCiudadExpedicion;

            private string codigoPaisExpedicion;
        }

        public InformacionExpedicionIdentificacion informacionExpedicionIdentificacion;

        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class InformacionNacimientoPersona
        {

            private System.DateTime fecha;

            private bool fechaSpecified;

            private string codigoCiudad;

            private string codigoPais;
        }

        public InformacionNacimientoPersona informacionNacimientoCliente;

        public string ocupacion;

        //public bool esIntermediarioMercadoCambiario;

        //public bool esIntermediarioMercadoCambiarioSpecified;

        //public bool esVigiladoPorSuperfinanciera;

        //public bool esVigiladoPorSuperfinancieraSpecified;

        

        //public bool fechaUltimaActualizacionSpecified;

        //public bool esClienteDeTesoreria;

        //public bool esClienteDeTesoreriaSpecified;

        public string usuarioRedTrader;

        public string codigoGrupoPersona;

        
    }

        public partial class Resultado
    {
        public string ResultCode;
        public string Msg;
    }
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class Fuente
    {
        public string CodError;
        public string Origen;
        public string MsgError;
    }
    */
    }

    public partial class ReponseDatosClienteHUB
    {
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public class Cliente
        {
            //public object item;

            public string tipoPersona;
        }

        public Cliente cliente;

        public string codigoSegmento;

        public string codigoSubSegmento;

        public string razonSocial;

        public string nombreCompleto;

        public string codigoCIIU;

        public string codigoRol;

        public System.DateTime fechaEstadoVinculacion;

        public System.DateTime fechaUltimaActualizacion;

        public string codigoRegion;

        public string estadoVinculacion;

        public string codigoSector;

        public string codigoSubSector;

        public string nombreLargo;

        public string codigo;

        public string faultCode { get; set; }

        public string faultDescription { get; set; }

        /*
        public string identificadorClienteBP;

        public string llaveMDM;

        public string llaveCIF;                

        public string codigoSubCIIU;        

        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public class GerenteMonedaExtranjera
        {

            private string nombreLargo;

            private string codigo;

            private string numeroDocumentoIdentidad;
        }

        public Resultado[] Resultados;
        public Fuente[] Fuentes;
        

        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public class IdentificacionCliente
        {

            public string tipoDocumento;

            public string numeroDocumento;
        }

        public GerenteMonedaExtranjera gerenteMonedaExtranjera;

        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class GerenteComercial
        {

            private string nombreLargo;

            private string codigo;
        }

        public GerenteComercial gerenteComercial;

        public IdentificacionCliente identificacionCliente;

        public string estadoCliente;

        public string estadoVinculacion;        

        public bool fechaEstadoVinculacionSpecified;

        public System.DateTime fechaCambioEstado;

        public bool fechaCambioEstadoSpecified;        

        public string codigoOficina;
        

        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class InformacionExpedicionIdentificacion
        {

            private System.DateTime fechaExpedicion;

            private bool fechaExpedicionSpecified;

            private string codigoCiudadExpedicion;

            private string codigoPaisExpedicion;
        }

        public InformacionExpedicionIdentificacion informacionExpedicionIdentificacion;

        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class InformacionNacimientoPersona
        {

            private System.DateTime fecha;

            private bool fechaSpecified;

            private string codigoCiudad;

            private string codigoPais;
        }

        public InformacionNacimientoPersona informacionNacimientoCliente;

        public string ocupacion;

        //public bool esIntermediarioMercadoCambiario;

        //public bool esIntermediarioMercadoCambiarioSpecified;

        //public bool esVigiladoPorSuperfinanciera;

        //public bool esVigiladoPorSuperfinancieraSpecified;

        

        //public bool fechaUltimaActualizacionSpecified;

        //public bool esClienteDeTesoreria;

        //public bool esClienteDeTesoreriaSpecified;

        public string usuarioRedTrader;

        public string codigoGrupoPersona;

        
    }

        public partial class Resultado
    {
        public string ResultCode;
        public string Msg;
    }
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class Fuente
    {
        public string CodError;
        public string Origen;
        public string MsgError;
    }
    */
    }

}