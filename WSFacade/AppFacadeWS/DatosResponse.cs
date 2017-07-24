//Duver Manuel Marzola Muentes
namespace ServicioBusqueda
{
    using System.Xml.Serialization;
    using System;
    using System.Collections;

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class DatosBasicosResponse
    {
        public string primerNombre;

        public string segundoNombre;

        public string primerApellido;

        public string segundoApellido;

        public string nombreCompleto;

        public string tipoPersona;

        public string codigoCIIU;

        public string codigoSegmento;

        public string codigoSubSegmento;

        public string nombreGerenteComercial;

        public string codigoRol;

        public string estadoVinculacion;

        public string codigoOficina;

        public string codigoSector;

        public string codigoSubSector;

        public DateTime fechaExpedicion;

        public string codigoCiudadExpedicion;

        public DateTime fechaNacimiento;

        public string codigoCiudadNacimiento;

        public string ocupacion;

        public bool bClienteNoExiste;

        public string estadoCliente;
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class DatosPersonalesResponse
    {
        public string calificacionRiegoInterno;

        public string empresaLabora;

        public string genero;

        public string codigoEstadoCivil;

        public string nacionalidad;

        public string nombreGerenciador;

        public string usuarioRedGerenciador;

        public string actividadEconomica;

        public string codigoCiudad;
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class RecuperarCatalogoBB2Response
    {
        public string condicionForma;

        public string vigencia;

        public string documentType;

        public string documentSubType;

        public int diasVigencia;

        public DateTime fechaVigencia;

        public string obligatorio;

        public string sUrl;
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class claveValor
    {
        public string sClave;

        public string sValor;
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ListaControlResponse
    {
        public string sTipoEstado;

        public string sMensajeEstado;
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class DatosProspectoRequest
    {
        public string sSystemId;
        public string sMessageId;
        public string sUserNameId;
        public string sName;
        public string sNamespace;
        public string sOperation;
        public string urlServicio;
        public DatosGeneralesProspecto oDatosGenerales;
        public DatosOficina[] oficinas;
        public DatosRelaciones[] relaciones;
        public DatosOperExtranjera[] operExtranjera;
        public DatosValExternas[] valExternas;
        public DatosResidenciaFiscal[] residenciaFiscal;
        public DatosDireccion[] datosUbicacion;
        public Identificacion identificacion;
        public string sMensajeEstado;
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class DatosGeneralesProspecto
    {
        public string tipoCliente;

        public string primerNombre;

        public string segundoNombre;

        public string primerApellido;

        public string segundoApellido;

        public string nacionalidad;

        public string paisExpCedula;

        public string ciudadExpCedula;

        public DateTime fechaExpCedula;

        public string paisNacimiento;

        public string ciudadNacimiento;

        public DateTime fechaNacimiento;

        public string sexo;

        public string vincular;

        public string restringido;

        public string infoVeridica;

        public string segmentacion;

        public string segmesarlaft;

        public string pep;

        public string estadoCivil;

        public string ocupacion;

        public string cargo;

        public string profesion;

        public string ciiu;

        public string paisOrigenRec;

        public string ciudadOrigenRec;

        public string nombreEmpresa;

        public string tipoContrato;

        public DateTime fechaIngreso;

        public decimal ingresosMensuales;

        public decimal otrosIngresos;

        public decimal ventasAnuales;

        public DateTime fechaVenAnual;

        public decimal activos;

        public decimal pasivos;

        public decimal egresosMensuales;

        public decimal patrimonio;

        public string personasCargo;

        public string nivelAcademico;

        public string estrato;

        public string declarante;

        public string moneda;

        public string regimenIva;

        public string agenteRetencion;

        public string numEmpleado;

        public string canalContacto;

        public string lugarContacto;

        public string concepComercial;
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class DatosOficina
    {

        public string idOrgVentas;

        public string canalDistri;

        public string sector;

        public string codSucursal;

        public string lugarRadica;
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class DatosRelaciones
    {

        public string operacion;

        public string tipoFuncion;

        public string tipoDocumento;

        public string numeroDocumento;
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class DatosOperExtranjera
    {

        public string operacion;

        public string operacionME;

        public string tipoOperME;

        public string nombreEntidad;

        public string tipoProducto;

        public string numProducto;

        public decimal montoMensual;

        public string moneda;

        public string pais;

        public string departamento;

        public string ciudad;
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class DatosValExternas
    {

        public string tipoValExterna;

        public string codValExterna;

        public string descripcion;
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class DatosResidenciaFiscal
    {

        public string operacion;

        public string codPaisFiscal;

        public string numTributario;
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class DatosDireccion
    {
        public string llaveDireccion;

        public string correspondencia;

        public string tipoDireccion;

        public string direccion;

        public string barrio;

        public string pais;

        public string departamento;

        public string ciudad;

        public string email;

        public string telefono;

        public string extension;

        public string celular;
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class Identificacion
    {
        public string tipoDocumento;

        public string numeroDocumento;
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class DatosProspectoResponse
    {
        public string tipoDocumento;

        public string numeroDocumento;

        public string segmento;

        public string subSegmento;

        public string actEconomica;

        public string sector;

        public string subSector;

        public string codigoRespuesta;

        public string descripcionRespuesta;
    }
}


