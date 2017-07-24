namespace AppFacadeWS
{

    using System;
    using System.ServiceModel;
    using System.Web.Services;
    using System.Web.Services.Protocols;
    using AppFacadeWS.ConsultarListasControl;
    using AppFacadeWS.GestionProspecto;
    using AppFacadeWS.pruebaHUB;
    using AppFacadeWS.RecuperarCatalogoBB2;
    using ServicioBusqueda;
    using System.ServiceModel.Channels;
    using System.Xml;

    [WebService(Namespace = "http://localhost/FacadeService")]
    [SoapDocumentService(RoutingStyle = SoapServiceRoutingStyle.RequestElement)]
    public class FacadeService
    {
        RecuperarDatosClienteClient recuperarDatosCliente;
        RecuperarCatalogo_BB2Client recuperarCatalogo_BB2;
        ConsultarEstadoClienteListasControl1Client consultarListasControl;
        GestionarProspectoClient gestionarProspecto;

        [WebMethod]
        public DatosBasicosResponse BusquedaDatosBasicosHUB(string sSystemId, string sMessageId, string sUserNameId, string sName, string sNamespace, string sOperation, string sTipoDoc, string sNumeroDoc, string urlServicio)
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            binding.Security = new BasicHttpSecurity();
            binding.Security.Mode = BasicHttpSecurityMode.Transport;

            //Specify the address to be used for the client.
            EndpointAddress address = new EndpointAddress(urlServicio);

            recuperarDatosCliente = new RecuperarDatosClienteClient(binding, address);
            //recuperarDatosCliente = new RecuperarDatosClienteClient();
            //recuperarDatosCliente.Endpoint.Address = new System.ServiceModel.EndpointAddress(urlServicio);      

            AppFacadeWS.pruebaHUB.RequestHeader requestHeader = new AppFacadeWS.pruebaHUB.RequestHeader();
            //Guardo en el Header
            requestHeader.systemId = sSystemId;
            requestHeader.messageId = sMessageId;
            requestHeader.userId = new AppFacadeWS.pruebaHUB.UsernameToken();
            requestHeader.userId.userName = sUserNameId;
            requestHeader.destination = new AppFacadeWS.pruebaHUB.Destination();
            requestHeader.destination.name = sName;
            requestHeader.destination.@namespace = sNamespace;
            requestHeader.destination.operation = sOperation;

            RecuperarDatosBasicosCliente datosEntrada = new RecuperarDatosBasicosCliente();
            datosEntrada.identificacionCliente = new AppFacadeWS.pruebaHUB.IdentificacionCliente();
            datosEntrada.identificacionCliente.numeroDocumento = sNumeroDoc;
            datosEntrada.identificacionCliente.tipoDocumento = sTipoDoc;

            RecuperarDatosBasicosClienteResponse datosSalida = new RecuperarDatosBasicosClienteResponse();

            try
            {
                AppFacadeWS.pruebaHUB.ResponseHeader responseHeader = new pruebaHUB.ResponseHeader();
                responseHeader = recuperarDatosCliente.recuperarDatosBasicosCliente(requestHeader, datosEntrada, out datosSalida);
                DatosBasicosResponse datosResponse = new DatosBasicosResponse();
                //Cargo datos para persona Natural
                if (datosSalida.cliente.Item.GetType().Name == "PersonaNatural")
                {
                    PersonaNatural personaNatural = (PersonaNatural)datosSalida.cliente.Item;
                    datosResponse.primerNombre = personaNatural.nombreCliente.primerNombre;
                    datosResponse.segundoNombre = personaNatural.nombreCliente.segundoNombre;
                    datosResponse.primerApellido = personaNatural.nombreCliente.primerApellido;
                    datosResponse.segundoApellido = personaNatural.nombreCliente.segundoApellido;
                    datosResponse.nombreCompleto = personaNatural.nombreLargoCliente;

                }
                //Cargo datos para persona Jurida, esto es por si en algun momento del tiempo se implementa
                else if (datosSalida.cliente.Item.GetType().Name == "PersonaJuridica")
                {
                    PersonaJuridica personaJuridica = (PersonaJuridica)datosSalida.cliente.Item;
                    datosResponse.nombreCompleto = personaJuridica.razonSocial;
                }
                datosResponse.tipoPersona = datosSalida.cliente.tipoPersona;
                //Cargo el resto de los datos.

                datosResponse.codigoCIIU = datosSalida.codigoCIIU;
                datosResponse.codigoSegmento = datosSalida.codigoSegmento;
                datosResponse.codigoSubSegmento = datosSalida.codigoSubSegmento;
                if (datosSalida.gerenteComercial != null)
                {
                    datosResponse.nombreGerenteComercial = datosSalida.gerenteComercial.nombreLargo;
                }
                datosResponse.codigoRol = datosSalida.codigoRol;
                datosResponse.estadoVinculacion = datosSalida.estadoVinculacion;
                datosResponse.codigoOficina = datosSalida.codigoOficina;
                datosResponse.codigoSector = datosSalida.codigoSector;
                datosResponse.codigoSubSector = datosSalida.codigoSubSector;
                datosResponse.estadoCliente = datosSalida.estadoCliente;
                if (datosSalida.informacionExpedicionIdentificacion != null)
                {
                    if (datosSalida.informacionExpedicionIdentificacion.fechaExpedicion != null && datosSalida.informacionExpedicionIdentificacion.fechaExpedicion != DateTime.MinValue)
                    {
                        datosResponse.fechaExpedicion = datosSalida.informacionExpedicionIdentificacion.fechaExpedicion;
                    }
                    //ciudad expedicion documento
                    if (datosSalida.informacionExpedicionIdentificacion.codigoCiudadExpedicion != null && datosSalida.informacionExpedicionIdentificacion.codigoCiudadExpedicion.Length > 0)
                    {
                        string sCiudadExpedicion = datosSalida.informacionExpedicionIdentificacion.codigoCiudadExpedicion;
                        if (sCiudadExpedicion.Length == 8 && sCiudadExpedicion.StartsWith("0"))
                        {
                            sCiudadExpedicion = sCiudadExpedicion.Substring(1);
                        }
                        datosResponse.codigoCiudadExpedicion = sCiudadExpedicion;
                    }
                }
                if (datosSalida.informacionNacimientoCliente != null)
                {
                    datosResponse.fechaNacimiento = datosSalida.informacionNacimientoCliente.fecha;
                    //Validacion campos Ciudad Nacimiento
                    if (datosSalida.informacionNacimientoCliente.codigoCiudad != null && datosSalida.informacionNacimientoCliente.codigoCiudad.Length > 0)
                    {
                        string sCiudadNacimiento = datosSalida.informacionNacimientoCliente.codigoCiudad;
                        if (sCiudadNacimiento.Length == 8 && sCiudadNacimiento.StartsWith("0"))
                        {
                            sCiudadNacimiento = sCiudadNacimiento.Substring(1);
                        }
                        datosResponse.codigoCiudadNacimiento = sCiudadNacimiento;
                    }
                }
                datosResponse.ocupacion = datosSalida.ocupacion;
                datosResponse.bClienteNoExiste = false;
                return datosResponse;
            }
            catch (Exception exc)
            {
                DatosBasicosResponse datosResponseError = new DatosBasicosResponse();
                datosResponseError.bClienteNoExiste = true;
                return datosResponseError;
            }
        }

        [WebMethod]
        public DatosPersonalesResponse BusquedaDatosPersonalesHUB(string sSystemId, string sMessageId, string sUserNameId, string sName, string sNamespace, string sOperation, string sTipoDoc, string sNumeroDoc, string urlServicio)
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            binding.Security = new BasicHttpSecurity();
            binding.Security.Mode = BasicHttpSecurityMode.Transport;
            //Specify the address to be used for the client.
            EndpointAddress address = new EndpointAddress(urlServicio);

            recuperarDatosCliente = new RecuperarDatosClienteClient(binding, address);
            //recuperarDatosCliente = new RecuperarDatosClienteClient();
            //recuperarDatosCliente.Endpoint.Address = new System.ServiceModel.EndpointAddress(urlServicio);

            AppFacadeWS.pruebaHUB.RequestHeader requestHeader = new AppFacadeWS.pruebaHUB.RequestHeader();
            //Guardo en el Header
            requestHeader.systemId = sSystemId;
            requestHeader.messageId = sMessageId;
            requestHeader.userId = new AppFacadeWS.pruebaHUB.UsernameToken();
            requestHeader.userId.userName = sUserNameId;
            requestHeader.destination = new AppFacadeWS.pruebaHUB.Destination();
            requestHeader.destination.name = sName;
            requestHeader.destination.@namespace = sNamespace;
            requestHeader.destination.operation = sOperation;

            RecuperarDatosPersonalesCliente datosEntrada = new RecuperarDatosPersonalesCliente();

            datosEntrada.identificacionCliente = new AppFacadeWS.pruebaHUB.IdentificacionCliente();
            datosEntrada.identificacionCliente.numeroDocumento = sNumeroDoc;
            datosEntrada.identificacionCliente.tipoDocumento = sTipoDoc;

            RecuperarDatosPersonalesClienteResponse datosSalida = new RecuperarDatosPersonalesClienteResponse();

            try
            {

                AppFacadeWS.pruebaHUB.ResponseHeader responseHeader = new pruebaHUB.ResponseHeader();
                responseHeader = recuperarDatosCliente.recuperarDatosPersonalesCliente(requestHeader, datosEntrada, out datosSalida);
                DatosPersonalesResponse datosResponse = new DatosPersonalesResponse();
                //Cargo la respuesta
                datosResponse.calificacionRiegoInterno = datosSalida.calificacionRiesgoInterno;
                datosResponse.empresaLabora = datosSalida.empresaDondeLabora;
                datosResponse.genero = datosSalida.genero;
                datosResponse.codigoEstadoCivil = datosSalida.codigoEstadoCivil;
                //Valido nacionalidad nula
                if (datosSalida.codigoPaisNacionalidad != null && datosSalida.codigoPaisNacionalidad.ItemElementName == ItemChoiceType1.codigoISO3166Alfa2)
                {
                    datosResponse.nacionalidad = datosSalida.codigoPaisNacionalidad.Item;
                }

                if (datosSalida.informacionGerenciador != null)
                {
                    datosResponse.nombreGerenciador = datosSalida.informacionGerenciador.nombre;
                    datosResponse.usuarioRedGerenciador = datosSalida.informacionGerenciador.usuarioRed;
                }

                datosResponse.actividadEconomica = datosSalida.actividadEconomica;

                if (datosSalida.informacionOrigenRecursos != null)
                {
                    //Validacion campos Ciudad Origen recurzos
                    if (datosSalida.informacionOrigenRecursos.codigoCiudad != null && datosSalida.informacionOrigenRecursos.codigoCiudad.Length > 0)
                    {
                        string sCiudadOrigenRecu = datosSalida.informacionOrigenRecursos.codigoCiudad;
                        if (sCiudadOrigenRecu.Length == 8 && sCiudadOrigenRecu.StartsWith("0"))
                        {
                            sCiudadOrigenRecu = sCiudadOrigenRecu.Substring(1);
                        }
                        datosResponse.codigoCiudad = sCiudadOrigenRecu;
                    }
                }

                return datosResponse;
            }
            catch (Exception exc)
            {
                throw;
            }
        }


        [WebMethod]
        public RecuperarCatalogoBB2Response[] RecuperarCatalogoBB2_Cliente(string sSystemId, string sMessageId, string sUserNameId, string sName, string sNamespace, string sOperation, string sTipoDoc, string sNumeroDoc, string sOrigin, claveValor[] arrayValores, string sBranchCode, string sProductCode, string urlServicio)
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            binding.Security = new BasicHttpSecurity();
            binding.Security.Mode = BasicHttpSecurityMode.Transport;
            //Specify the address to be used for the client.
            EndpointAddress address = new EndpointAddress(urlServicio);

            recuperarCatalogo_BB2 = new RecuperarCatalogo_BB2Client(binding, address);
            //recuperarCatalogo_BB2 = new RecuperarCatalogo_BB2Client();
            //recuperarCatalogo_BB2.Endpoint.Address = new System.ServiceModel.EndpointAddress(urlServicio); 

            AppFacadeWS.RecuperarCatalogoBB2.RequestHeader requestHeader = new AppFacadeWS.RecuperarCatalogoBB2.RequestHeader();
            //Guardo en el Header
            requestHeader.systemId = sSystemId;
            requestHeader.messageId = sMessageId;
            requestHeader.userId = new AppFacadeWS.RecuperarCatalogoBB2.UsernameToken();
            requestHeader.userId.userName = sUserNameId;
            requestHeader.destination = new AppFacadeWS.RecuperarCatalogoBB2.Destination();
            requestHeader.destination.name = sName;
            requestHeader.destination.@namespace = sNamespace;
            requestHeader.destination.operation = sOperation;


            //Datos Entrada
            RecuperarCatalogoPorCliente datosEntrada = new RecuperarCatalogoPorCliente();
            datosEntrada.arg0 = new clientRequest();
            datosEntrada.arg0.documentIdType = sTipoDoc;
            datosEntrada.arg0.idNumber = sNumeroDoc;
            datosEntrada.arg0.origin = sOrigin;
            //Datos del Producto
            product datosProducto = new product();
            datosProducto.branchCode = sBranchCode;
            datosProducto.productCode = sProductCode;
            //Cargo el array de valores           
            datosProducto.attributes = new attributes[arrayValores.Length];
            for (int i = 0; i < arrayValores.Length; i++)
            {
                datosProducto.attributes[i] = new attributes { name = arrayValores[i].sClave, value = arrayValores[i].sValor };
            }
            //Cargo procesos a la entidad principal
            datosEntrada.arg0.Item = datosProducto;


            RecuperarCatalogoPorClienteResponse datosSalida = new RecuperarCatalogoPorClienteResponse();

            try
            {
                AppFacadeWS.RecuperarCatalogoBB2.ResponseHeader responseHeader = new RecuperarCatalogoBB2.ResponseHeader();
                responseHeader = recuperarCatalogo_BB2.recuperarCatalogoPorCliente(requestHeader, datosEntrada, out datosSalida);
                RecuperarCatalogoBB2Response[] datosResponse = new RecuperarCatalogoBB2Response[datosSalida.@return.Length];
                //Cargo la respuesta
                for (int i = 0; i < datosSalida.@return.Length; i++)
                {
                    datosResponse[i] = new RecuperarCatalogoBB2Response
                    {
                        condicionForma = datosSalida.@return[i].conditionForm
                        ,
                        vigencia = datosSalida.@return[i].documentStatus
                        ,
                        documentType = datosSalida.@return[i].documentType
                        ,
                        documentSubType = datosSalida.@return[i].documentSubType
                        ,
                        diasVigencia = datosSalida.@return[i].expiryDays
                        ,
                        fechaVigencia = datosSalida.@return[i].expiryDate
                        ,
                        obligatorio = datosSalida.@return[i].obligatory
                        ,
                        sUrl = datosSalida.@return[i].url
                    };
                }

                return datosResponse;
            }
            catch (Exception exc)
            {
                throw;
            }
        }

        [WebMethod]
        public ListaControlResponse ConsultarListasDeControl(string sSystemId, string sMessageId, string sUserNameId, string sName, string sNamespace, string sOperation, string sTipoDoc, string sNumeroDoc, string sNombreCompleto, string urlServicio)
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            binding.Security = new BasicHttpSecurity();
            binding.Security.Mode = BasicHttpSecurityMode.Transport;

            //Specify the address to be used for the client.
            EndpointAddress address = new EndpointAddress(urlServicio);

            consultarListasControl = new ConsultarEstadoClienteListasControl1Client(binding, address);
            //consultarListasControl = new ConsultarEstadoClienteListasControl1Client();            
            //consultarListasControl.Endpoint.Address = new System.ServiceModel.EndpointAddress(urlServicio);

            ConsultarListasControl.RequestHeader requestHeader = new ConsultarListasControl.RequestHeader();
            //Guardo en el Header
            requestHeader.systemId = sSystemId;
            requestHeader.messageId = sMessageId;
            requestHeader.userId = new ConsultarListasControl.UsernameToken();
            requestHeader.userId.userName = sUserNameId;
            requestHeader.destination = new ConsultarListasControl.Destination();
            requestHeader.destination.name = sName;
            requestHeader.destination.@namespace = sNamespace;
            requestHeader.destination.operation = sOperation;

            ConsultarEstadoClienteListasControl datosEntrada = new ConsultarEstadoClienteListasControl();
            datosEntrada.identidadCliente = new ConsultarListasControl.IdentificacionCliente();
            datosEntrada.identidadCliente.nombreCompleto = sNombreCompleto;
            datosEntrada.identidadCliente.numeroDocumento = sNumeroDoc;
            datosEntrada.identidadCliente.tipoDocumento = sTipoDoc;


            ConsultarEstadoClienteListasControlResponse datosSalida = new ConsultarEstadoClienteListasControlResponse();

            try
            {
                AppFacadeWS.ConsultarListasControl.ResponseHeader responseHeader = new AppFacadeWS.ConsultarListasControl.ResponseHeader();
                responseHeader = consultarListasControl.consultarEstadoClienteListasControl(requestHeader, datosEntrada, out datosSalida);
                ListaControlResponse listaControlResponse = new ListaControlResponse();
                if (datosSalida != null && datosSalida.estadoClienteListasControl != null)
                {
                    listaControlResponse.sTipoEstado = datosSalida.estadoClienteListasControl.tipoEstado;
                    listaControlResponse.sMensajeEstado = datosSalida.estadoClienteListasControl.mensajeEstado;
                }
                return listaControlResponse;
            }
            catch (Exception exc)
            {
                throw;
            }
        }

        [WebMethod]
        public RecuperarCatalogoBB2Response[] RecuperarCatalogoBB2_Proceso(string sSystemId, string sMessageId, string sUserNameId, string sName, string sNamespace, string sOperation, string sTipoDoc, string sNumeroDoc, string sOrigin, claveValor[] arrayValores, string sBranchCode, string sProcessCode, string sSubProcessCode, string urlServicio)
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            binding.Security = new BasicHttpSecurity();
            binding.Security.Mode = BasicHttpSecurityMode.Transport;
            //Specify the address to be used for the client.
            EndpointAddress address = new EndpointAddress(urlServicio);

            recuperarCatalogo_BB2 = new RecuperarCatalogo_BB2Client(binding, address);

            AppFacadeWS.RecuperarCatalogoBB2.RequestHeader requestHeader = new AppFacadeWS.RecuperarCatalogoBB2.RequestHeader();
            //Guardo en el Header
            requestHeader.systemId = sSystemId;
            requestHeader.messageId = sMessageId;
            requestHeader.userId = new AppFacadeWS.RecuperarCatalogoBB2.UsernameToken();
            requestHeader.userId.userName = sUserNameId;
            requestHeader.destination = new AppFacadeWS.RecuperarCatalogoBB2.Destination();
            requestHeader.destination.name = sName;
            requestHeader.destination.@namespace = sNamespace;
            requestHeader.destination.operation = sOperation;


            //Datos Entrada
            RecuperarCatalogoPorCliente datosEntrada = new RecuperarCatalogoPorCliente();
            datosEntrada.arg0 = new clientRequest();
            datosEntrada.arg0.documentIdType = sTipoDoc;
            datosEntrada.arg0.idNumber = sNumeroDoc;
            datosEntrada.arg0.origin = sOrigin;
            //Datos del proceso
            process datosProceso = new process();
            datosProceso.branchCode = sBranchCode;
            datosProceso.processCode = sProcessCode;
            datosProceso.subProcessCode = sSubProcessCode;

            //Cargo el array de valores           
            datosProceso.attributes = new attributes[arrayValores.Length];
            for (int i = 0; i < arrayValores.Length; i++)
            {
                datosProceso.attributes[i] = new attributes { name = arrayValores[i].sClave, value = arrayValores[i].sValor };
            }
            //Cargo procesos a la entidad principal
            datosEntrada.arg0.Item = datosProceso;


            RecuperarCatalogoPorClienteResponse datosSalida = new RecuperarCatalogoPorClienteResponse();

            try
            {
                AppFacadeWS.RecuperarCatalogoBB2.ResponseHeader responseHeader = new RecuperarCatalogoBB2.ResponseHeader();
                responseHeader = recuperarCatalogo_BB2.recuperarCatalogoPorCliente(requestHeader, datosEntrada, out datosSalida);
                RecuperarCatalogoBB2Response[] datosResponse = new RecuperarCatalogoBB2Response[datosSalida.@return.Length];
                //Cargo la respuesta
                for (int i = 0; i < datosSalida.@return.Length; i++)
                {
                    datosResponse[i] = new RecuperarCatalogoBB2Response
                    {
                        condicionForma = datosSalida.@return[i].conditionForm
                        ,
                        vigencia = datosSalida.@return[i].documentStatus
                        ,
                        documentType = datosSalida.@return[i].documentType
                        ,
                        documentSubType = datosSalida.@return[i].documentSubType
                        ,
                        diasVigencia = datosSalida.@return[i].expiryDays
                        ,
                        fechaVigencia = datosSalida.@return[i].expiryDate
                        ,
                        obligatorio = datosSalida.@return[i].obligatory
                        ,
                        sUrl = datosSalida.@return[i].url
                    };
                }

                return datosResponse;
            }
            catch (Exception exc)
            {
                throw;
            }
        }

        [WebMethod]
        public DatosProspectoResponse CrearPersonaNatural(DatosProspectoRequest datosProspectoRequest)
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            binding.Security = new BasicHttpSecurity();
            binding.Security.Mode = BasicHttpSecurityMode.Transport;
            //Specify the address to be used for the client.
            EndpointAddress address = new EndpointAddress(datosProspectoRequest.urlServicio);

            gestionarProspecto = new GestionarProspectoClient(binding, address);

            AppFacadeWS.GestionProspecto.RequestHeader requestHeader = new AppFacadeWS.GestionProspecto.RequestHeader();
            //Guardo en el Header
            requestHeader.systemId = datosProspectoRequest.sSystemId;
            requestHeader.messageId = datosProspectoRequest.sMessageId;
            requestHeader.userId = new AppFacadeWS.GestionProspecto.UsernameToken();
            requestHeader.userId.userName = datosProspectoRequest.sUserNameId;
            requestHeader.destination = new AppFacadeWS.GestionProspecto.Destination();
            requestHeader.destination.name = datosProspectoRequest.sName;
            requestHeader.destination.@namespace = datosProspectoRequest.sNamespace;
            requestHeader.destination.operation = datosProspectoRequest.sOperation;

            #region Datos generales
            var datosEntrada = new CrearPersonaNatural();
            if (datosProspectoRequest.oDatosGenerales != null)
            {
                datosEntrada.datosGenerales = new DatosGeneralesCrearPersonaNatural
                {
                    activos = datosProspectoRequest.oDatosGenerales.activos,
                    agenteRetencion = datosProspectoRequest.oDatosGenerales.agenteRetencion,
                    canalContacto = datosProspectoRequest.oDatosGenerales.canalContacto,
                    cargo = datosProspectoRequest.oDatosGenerales.cargo,
                    ciiu = datosProspectoRequest.oDatosGenerales.ciiu,
                    ciudadExpCedula = datosProspectoRequest.oDatosGenerales.ciudadExpCedula,
                    ciudadNacimiento = datosProspectoRequest.oDatosGenerales.ciudadNacimiento,
                    ciudadOrigenRec = datosProspectoRequest.oDatosGenerales.ciudadOrigenRec,
                    concepComercial = datosProspectoRequest.oDatosGenerales.concepComercial,
                    declarante = datosProspectoRequest.oDatosGenerales.declarante,
                    egresosMensuales = datosProspectoRequest.oDatosGenerales.egresosMensuales,
                    estadoCivil = datosProspectoRequest.oDatosGenerales.estadoCivil,
                    estrato = datosProspectoRequest.oDatosGenerales.estrato,
                    fechaExpCedula = datosProspectoRequest.oDatosGenerales.fechaExpCedula,
                    fechaIngreso = datosProspectoRequest.oDatosGenerales.fechaIngreso,
                    fechaNacimiento = datosProspectoRequest.oDatosGenerales.fechaNacimiento,
                    infoVeridica = datosProspectoRequest.oDatosGenerales.infoVeridica,
                    ingresosMensuales = datosProspectoRequest.oDatosGenerales.ingresosMensuales,
                    lugarContacto = datosProspectoRequest.oDatosGenerales.lugarContacto,
                    moneda = datosProspectoRequest.oDatosGenerales.moneda,
                    nacionalidad = datosProspectoRequest.oDatosGenerales.nacionalidad,
                    nivelAcademico = datosProspectoRequest.oDatosGenerales.nivelAcademico,
                    nombreEmpresa = datosProspectoRequest.oDatosGenerales.nombreEmpresa,
                    numEmpleado = datosProspectoRequest.oDatosGenerales.numEmpleado,
                    ocupacion = datosProspectoRequest.oDatosGenerales.ocupacion,
                    otrosIngresos = datosProspectoRequest.oDatosGenerales.otrosIngresos,
                    paisExpCedula = datosProspectoRequest.oDatosGenerales.paisExpCedula,
                    paisNacimiento = datosProspectoRequest.oDatosGenerales.paisNacimiento,
                    paisOrigenRec = datosProspectoRequest.oDatosGenerales.paisOrigenRec,
                    pasivos = datosProspectoRequest.oDatosGenerales.pasivos,
                    patrimonio = datosProspectoRequest.oDatosGenerales.patrimonio,
                    pep = datosProspectoRequest.oDatosGenerales.pep,
                    personasCargo = datosProspectoRequest.oDatosGenerales.personasCargo,
                    primerApellido = datosProspectoRequest.oDatosGenerales.primerApellido,
                    primerNombre = datosProspectoRequest.oDatosGenerales.primerNombre,
                    profesion = datosProspectoRequest.oDatosGenerales.profesion,
                    regimenIva = datosProspectoRequest.oDatosGenerales.regimenIva,
                    restringido = datosProspectoRequest.oDatosGenerales.restringido,
                    segmentacion = datosProspectoRequest.oDatosGenerales.segmentacion,
                    segmesarlaft = datosProspectoRequest.oDatosGenerales.segmesarlaft,
                    segundoApellido = datosProspectoRequest.oDatosGenerales.segundoApellido,
                    segundoNombre = datosProspectoRequest.oDatosGenerales.segundoNombre,
                    sexo = datosProspectoRequest.oDatosGenerales.sexo,
                    tipoCliente = datosProspectoRequest.oDatosGenerales.tipoCliente,
                    tipoContrato = datosProspectoRequest.oDatosGenerales.tipoContrato,
                    ventasAnuales = datosProspectoRequest.oDatosGenerales.ventasAnuales,
                    vincular = datosProspectoRequest.oDatosGenerales.vincular,
                    activosSpecified = true,
                    egresosMensualesSpecified = true,
                    fechaExpCedulaSpecified = true,
                    fechaIngresoSpecified = true,
                    fechaNacimientoSpecified = true,
                    // fechaVenAnualSpecified = true,
                    ingresosMensualesSpecified = true,
                    otrosIngresosSpecified = true,
                    pasivosSpecified = true,
                    patrimonioSpecified = true,
                    ventasAnualesSpecified = true,
                    // fechaVenAnual = datosProspectoRequest.oDatosGenerales.fechaVenAnual,
                };
            }
            #endregion

            #region Datos ubicacion
            if (datosProspectoRequest.datosUbicacion != null)
            {
                int ubicacionLenght = datosProspectoRequest.datosUbicacion.Length;
                if (ubicacionLenght > 0)
                {
                    datosEntrada.datosUbicacion = new GestionProspecto.DatosDireccion[ubicacionLenght];
                    for (int i = 0; i < ubicacionLenght; i++)
                    {
                        datosEntrada.datosUbicacion[i] = new GestionProspecto.DatosDireccion();
                        datosEntrada.datosUbicacion[i].barrio = datosProspectoRequest.datosUbicacion[i].barrio;
                        datosEntrada.datosUbicacion[i].celular = datosProspectoRequest.datosUbicacion[i].celular;
                        datosEntrada.datosUbicacion[i].ciudad = datosProspectoRequest.datosUbicacion[i].ciudad;
                        datosEntrada.datosUbicacion[i].correspondencia = datosProspectoRequest.datosUbicacion[i].correspondencia;
                        datosEntrada.datosUbicacion[i].departamento = datosProspectoRequest.datosUbicacion[i].departamento;
                        datosEntrada.datosUbicacion[i].direccion = datosProspectoRequest.datosUbicacion[i].direccion;
                        datosEntrada.datosUbicacion[i].email = datosProspectoRequest.datosUbicacion[i].email;
                        datosEntrada.datosUbicacion[i].llaveDireccion = datosProspectoRequest.datosUbicacion[i].llaveDireccion;
                        datosEntrada.datosUbicacion[i].pais = datosProspectoRequest.datosUbicacion[i].pais;
                        datosEntrada.datosUbicacion[i].telefono = datosProspectoRequest.datosUbicacion[i].telefono;
                        datosEntrada.datosUbicacion[i].tipoDireccion = datosProspectoRequest.datosUbicacion[i].tipoDireccion;
                        datosEntrada.datosUbicacion[i].extension = datosProspectoRequest.datosUbicacion[i].extension;
                    }
                }
            }
            #endregion

            #region Datos Oficina
            if (datosProspectoRequest.oficinas != null)
            {
                int oficinasLenght = datosProspectoRequest.oficinas.Length;
                if (oficinasLenght > 0)
                {
                    datosEntrada.oficinas = new GestionProspecto.DatosOficina[oficinasLenght];
                    for (int i = 0; i < oficinasLenght; i++)
                    {
                        datosEntrada.oficinas[i] = new GestionProspecto.DatosOficina();
                        datosEntrada.oficinas[i].canalDistri = datosProspectoRequest.oficinas[i].canalDistri;
                        datosEntrada.oficinas[i].codSucursal = datosProspectoRequest.oficinas[i].codSucursal;
                        datosEntrada.oficinas[i].idOrgVentas = datosProspectoRequest.oficinas[i].idOrgVentas;
                        datosEntrada.oficinas[i].lugarRadica = datosProspectoRequest.oficinas[i].lugarRadica;
                        datosEntrada.oficinas[i].sector = datosProspectoRequest.oficinas[i].sector;
                    }
                }
            }
            #endregion

            #region Operacion Extrangera
            if (datosProspectoRequest.operExtranjera != null)
            {
                int operExtranjeraLenght = datosProspectoRequest.operExtranjera.Length;
                if (operExtranjeraLenght > 0)
                {
                    datosEntrada.operExtranjera = new GestionProspecto.DatosOperExtranjera[operExtranjeraLenght];
                    for (int i = 0; i < operExtranjeraLenght; i++)
                    {
                        datosEntrada.operExtranjera[i] = new GestionProspecto.DatosOperExtranjera();
                        datosEntrada.operExtranjera[i].ciudad = datosProspectoRequest.operExtranjera[i].ciudad;
                        datosEntrada.operExtranjera[i].departamento = datosProspectoRequest.operExtranjera[i].departamento;
                        datosEntrada.operExtranjera[i].moneda = datosProspectoRequest.operExtranjera[i].moneda;
                        datosEntrada.operExtranjera[i].montoMensual = datosProspectoRequest.operExtranjera[i].montoMensual;
                        datosEntrada.operExtranjera[i].nombreEntidad = datosProspectoRequest.operExtranjera[i].nombreEntidad;
                        datosEntrada.operExtranjera[i].numProducto = datosProspectoRequest.operExtranjera[i].numProducto;
                        datosEntrada.operExtranjera[i].operacion = datosProspectoRequest.operExtranjera[i].operacion;
                        datosEntrada.operExtranjera[i].operacionME = datosProspectoRequest.operExtranjera[i].operacionME;
                        datosEntrada.operExtranjera[i].pais = datosProspectoRequest.operExtranjera[i].pais;
                        datosEntrada.operExtranjera[i].tipoOperME = datosProspectoRequest.operExtranjera[i].tipoOperME;
                        datosEntrada.operExtranjera[i].tipoProducto = datosProspectoRequest.operExtranjera[i].tipoProducto;
                        datosEntrada.operExtranjera[i].montoMensualSpecified = true;
                    }
                }
            }
            else
            {
                datosEntrada.operExtranjera = new GestionProspecto.DatosOperExtranjera[0];
            }
            #endregion

            #region Relaciones
            if (datosProspectoRequest.relaciones != null)
            {
                int relacionesLenght = datosProspectoRequest.relaciones.Length;
                if (relacionesLenght > 0)
                {
                    datosEntrada.relaciones = new GestionProspecto.DatosRelaciones[relacionesLenght];
                    for (int i = 0; i < relacionesLenght; i++)
                    {
                        datosEntrada.relaciones[i] = new GestionProspecto.DatosRelaciones();
                        datosEntrada.relaciones[i].numeroDocumento = datosProspectoRequest.relaciones[i].numeroDocumento;
                        datosEntrada.relaciones[i].operacion = datosProspectoRequest.relaciones[i].operacion;
                        datosEntrada.relaciones[i].tipoDocumento = datosProspectoRequest.relaciones[i].tipoDocumento;
                        datosEntrada.relaciones[i].tipoFuncion = datosProspectoRequest.relaciones[i].tipoFuncion;
                    }
                }
            }
            else
            {
                datosEntrada.relaciones = new GestionProspecto.DatosRelaciones[0];
            }
            #endregion

            #region Residencia fiscaL
            if (datosProspectoRequest.residenciaFiscal != null)
            {
                int residenciaFiscalLenght = datosProspectoRequest.residenciaFiscal.Length;
                if (residenciaFiscalLenght > 0)
                {
                    datosEntrada.residenciaFiscal = new GestionProspecto.DatosResidenciaFiscal[residenciaFiscalLenght];
                    for (int i = 0; i < residenciaFiscalLenght; i++)
                    {
                        datosEntrada.residenciaFiscal[i] = new GestionProspecto.DatosResidenciaFiscal();
                        datosEntrada.residenciaFiscal[i].codPaisFiscal = datosProspectoRequest.residenciaFiscal[i].codPaisFiscal;
                        datosEntrada.residenciaFiscal[i].numTributario = datosProspectoRequest.residenciaFiscal[i].numTributario;
                        datosEntrada.residenciaFiscal[i].operacion = datosProspectoRequest.residenciaFiscal[i].operacion;
                    }
                }
            }
            else
            {
                datosEntrada.residenciaFiscal = new GestionProspecto.DatosResidenciaFiscal[0];
            }
            #endregion

            #region  Val Externas
            if (datosProspectoRequest.valExternas != null)
            {
                int valExternasLenght = datosProspectoRequest.valExternas.Length;
                if (valExternasLenght > 0)
                {
                    datosEntrada.valExternas = new GestionProspecto.DatosValExternas[valExternasLenght];
                    for (int i = 0; i < valExternasLenght; i++)
                    {
                        datosEntrada.valExternas[i] = new GestionProspecto.DatosValExternas();
                        datosEntrada.valExternas[i].codValExterna = datosProspectoRequest.valExternas[i].codValExterna;
                        datosEntrada.valExternas[i].descripcion = datosProspectoRequest.valExternas[i].descripcion;
                        datosEntrada.valExternas[i].tipoValExterna = datosProspectoRequest.valExternas[i].tipoValExterna;
                    }
                }
            }
            else
            {
                datosEntrada.valExternas = new GestionProspecto.DatosValExternas[0];
            }
            #endregion

            datosEntrada.identificacion = new GestionProspecto.Identificacion
            {
                numeroDocumento =
                datosProspectoRequest.identificacion.numeroDocumento,
                tipoDocumento = datosProspectoRequest.identificacion.tipoDocumento,
            };

            CrearPersonaNaturalResponse datosSalida = new CrearPersonaNaturalResponse();

            try
            {
                var responseHeader = gestionarProspecto.crearPersonaNatural(requestHeader, datosEntrada, out datosSalida);
                //recuperarDatosCliente.recuperarDatosBasicosCliente(requestHeader, datosEntrada, out datosSalida);
                DatosProspectoResponse datosResponse = new DatosProspectoResponse();
                //Cargo datos para persona Natural
                if (datosSalida != null)
                {
                    datosResponse.actEconomica = datosSalida.actEconomica;
                    datosResponse.codigoRespuesta = datosSalida.codigoRespuesta;
                    datosResponse.descripcionRespuesta = datosSalida.descripcionRespuesta;
                    datosResponse.numeroDocumento = datosSalida.numeroDocumento;
                    datosResponse.sector = datosSalida.sector;
                    datosResponse.segmento = datosSalida.segmento;
                    datosResponse.subSector = datosSalida.subSector;
                    datosResponse.subSegmento = datosSalida.subSegmento;
                    datosResponse.tipoDocumento = datosSalida.tipoDocumento;
                }
                return datosResponse;
            }
            catch (FaultException<CrmBusinessException> excNegocio)
            {
                DatosProspectoResponse datosResponseError = new DatosProspectoResponse();
                datosResponseError.codigoRespuesta = excNegocio.Detail.genericException.code;
                datosResponseError.descripcionRespuesta = excNegocio.Detail.genericException.description;
                return datosResponseError;
            }
            catch (FaultException<AppFacadeWS.GestionProspecto.SystemException> exsys)
            {
                DatosProspectoResponse datosResponseError = new DatosProspectoResponse();
                datosResponseError.codigoRespuesta = exsys.Detail.genericException.code;
                datosResponseError.descripcionRespuesta = exsys.Detail.genericException.description + ". Facade";
                return datosResponseError;
            }
            catch (Exception ex)
            {
                DatosProspectoResponse datosResponseError = new DatosProspectoResponse();
                datosResponseError.codigoRespuesta = "999";
                datosResponseError.descripcionRespuesta = ex.Message + ". Facade";
                return datosResponseError;
            }
        }

        [WebMethod]
        public DatosProspectoResponse ModificarPersonaNatural(DatosProspectoRequest datosProspectoRequest)
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            binding.Security = new BasicHttpSecurity();
            binding.Security.Mode = BasicHttpSecurityMode.Transport;
            EndpointAddress address = new EndpointAddress(datosProspectoRequest.urlServicio);
            gestionarProspecto = new GestionarProspectoClient(binding, address);

            AppFacadeWS.GestionProspecto.RequestHeader requestHeader = new AppFacadeWS.GestionProspecto.RequestHeader();
            requestHeader.systemId = datosProspectoRequest.sSystemId;
            requestHeader.messageId = datosProspectoRequest.sMessageId;
            requestHeader.userId = new AppFacadeWS.GestionProspecto.UsernameToken();
            requestHeader.userId.userName = datosProspectoRequest.sUserNameId;
            requestHeader.destination = new AppFacadeWS.GestionProspecto.Destination();
            requestHeader.destination.name = datosProspectoRequest.sName;
            requestHeader.destination.@namespace = datosProspectoRequest.sNamespace;
            requestHeader.destination.operation = datosProspectoRequest.sOperation;

            #region Datos generales
            var datosEntrada = new ModificarPersonaNatural();
            if (datosProspectoRequest.oDatosGenerales != null)
            {
                datosEntrada.datosGenerales = new DatosGeneralesModificarPersonaNatural
                {
                    activos = datosProspectoRequest.oDatosGenerales.activos == 0 ? -1 : datosProspectoRequest.oDatosGenerales.activos,
                    agenteRetencion = datosProspectoRequest.oDatosGenerales.agenteRetencion,
                    canalContacto = datosProspectoRequest.oDatosGenerales.canalContacto,
                    cargo = datosProspectoRequest.oDatosGenerales.cargo,
                    ciiu = datosProspectoRequest.oDatosGenerales.ciiu,
                    ciudadExpCedula = datosProspectoRequest.oDatosGenerales.ciudadExpCedula,
                    ciudadNacimiento = datosProspectoRequest.oDatosGenerales.ciudadNacimiento,
                    ciudadOrigenRec = datosProspectoRequest.oDatosGenerales.ciudadOrigenRec,
                    concepComercial = datosProspectoRequest.oDatosGenerales.concepComercial,
                    declarante = datosProspectoRequest.oDatosGenerales.declarante,
                    egresosMensuales = datosProspectoRequest.oDatosGenerales.egresosMensuales == 0 ? -1 : datosProspectoRequest.oDatosGenerales.egresosMensuales,
                    estadoCivil = datosProspectoRequest.oDatosGenerales.estadoCivil,
                    estrato = datosProspectoRequest.oDatosGenerales.estrato,
                    fechaExpCedula = datosProspectoRequest.oDatosGenerales.fechaExpCedula,
                    fechaIngreso = datosProspectoRequest.oDatosGenerales.fechaIngreso,
                    fechaNacimiento = datosProspectoRequest.oDatosGenerales.fechaNacimiento,
                    infoVeridica = datosProspectoRequest.oDatosGenerales.infoVeridica,
                    ingresosMensuales = datosProspectoRequest.oDatosGenerales.ingresosMensuales == 0 ? -1 : datosProspectoRequest.oDatosGenerales.ingresosMensuales,
                    lugarContacto = datosProspectoRequest.oDatosGenerales.lugarContacto,
                    moneda = datosProspectoRequest.oDatosGenerales.moneda,
                    nacionalidad = datosProspectoRequest.oDatosGenerales.nacionalidad,
                    nivelAcademico = datosProspectoRequest.oDatosGenerales.nivelAcademico,
                    nombreEmpresa = datosProspectoRequest.oDatosGenerales.nombreEmpresa,
                    numEmpleado = datosProspectoRequest.oDatosGenerales.numEmpleado,
                    ocupacion = datosProspectoRequest.oDatosGenerales.ocupacion,
                    otrosIngresos = datosProspectoRequest.oDatosGenerales.otrosIngresos == 0 ? -1 : datosProspectoRequest.oDatosGenerales.otrosIngresos,
                    paisExpCedula = datosProspectoRequest.oDatosGenerales.paisExpCedula,
                    paisNacimiento = datosProspectoRequest.oDatosGenerales.paisNacimiento,
                    paisOrigenRec = datosProspectoRequest.oDatosGenerales.paisOrigenRec,
                    pasivos = datosProspectoRequest.oDatosGenerales.pasivos == 0 ? -1 : datosProspectoRequest.oDatosGenerales.pasivos,
                    patrimonio = datosProspectoRequest.oDatosGenerales.patrimonio == 0 ? -1 : datosProspectoRequest.oDatosGenerales.patrimonio,
                    pep = datosProspectoRequest.oDatosGenerales.pep,
                    personasCargo = datosProspectoRequest.oDatosGenerales.personasCargo,
                    primerApellido = datosProspectoRequest.oDatosGenerales.primerApellido,
                    primerNombre = datosProspectoRequest.oDatosGenerales.primerNombre,
                    profesion = datosProspectoRequest.oDatosGenerales.profesion,
                    regimenIva = datosProspectoRequest.oDatosGenerales.regimenIva,
                    restringido = datosProspectoRequest.oDatosGenerales.restringido,
                    segmentacion = datosProspectoRequest.oDatosGenerales.segmentacion,
                    segmesarlaft = datosProspectoRequest.oDatosGenerales.segmesarlaft,
                    segundoApellido = datosProspectoRequest.oDatosGenerales.segundoApellido,
                    segundoNombre = datosProspectoRequest.oDatosGenerales.segundoNombre,
                    sexo = datosProspectoRequest.oDatosGenerales.sexo,
                    tipoCliente = datosProspectoRequest.oDatosGenerales.tipoCliente,
                    tipoContrato = datosProspectoRequest.oDatosGenerales.tipoContrato,
                    ventasAnuales = datosProspectoRequest.oDatosGenerales.ventasAnuales == 0 ? -1 : datosProspectoRequest.oDatosGenerales.ventasAnuales,
                    vincular = datosProspectoRequest.oDatosGenerales.vincular,
                    activosSpecified = true,
                    egresosMensualesSpecified = true,
                    fechaExpCedulaSpecified = true,
                    fechaIngresoSpecified = true,
                    fechaNacimientoSpecified = true,
                    // fechaVenAnualSpecified = true,
                    ingresosMensualesSpecified = true,
                    otrosIngresosSpecified = true,
                    pasivosSpecified = true,
                    patrimonioSpecified = true,
                    ventasAnualesSpecified = true,
                    // fechaVenAnual = datosProspectoRequest.oDatosGenerales.fechaVenAnual,
                };
            }
            #endregion

            #region Datos ubicacion
            if (datosProspectoRequest.datosUbicacion != null)
            {
                int ubicacionLenght = datosProspectoRequest.datosUbicacion.Length;
                if (ubicacionLenght > 0)
                {
                    datosEntrada.datosUbicacion = new GestionProspecto.DatosDireccion[ubicacionLenght];
                    for (int i = 0; i < ubicacionLenght; i++)
                    {
                        datosEntrada.datosUbicacion[i] = new GestionProspecto.DatosDireccion();
                        datosEntrada.datosUbicacion[i].barrio = datosProspectoRequest.datosUbicacion[i].barrio;
                        datosEntrada.datosUbicacion[i].celular = datosProspectoRequest.datosUbicacion[i].celular;
                        datosEntrada.datosUbicacion[i].ciudad = datosProspectoRequest.datosUbicacion[i].ciudad;
                        datosEntrada.datosUbicacion[i].correspondencia = datosProspectoRequest.datosUbicacion[i].correspondencia;
                        datosEntrada.datosUbicacion[i].departamento = datosProspectoRequest.datosUbicacion[i].departamento;
                        datosEntrada.datosUbicacion[i].direccion = datosProspectoRequest.datosUbicacion[i].direccion;
                        datosEntrada.datosUbicacion[i].email = datosProspectoRequest.datosUbicacion[i].email;
                        datosEntrada.datosUbicacion[i].llaveDireccion = datosProspectoRequest.datosUbicacion[i].llaveDireccion;
                        datosEntrada.datosUbicacion[i].pais = datosProspectoRequest.datosUbicacion[i].pais;
                        datosEntrada.datosUbicacion[i].telefono = datosProspectoRequest.datosUbicacion[i].telefono;
                        datosEntrada.datosUbicacion[i].tipoDireccion = datosProspectoRequest.datosUbicacion[i].tipoDireccion;
                        datosEntrada.datosUbicacion[i].extension = datosProspectoRequest.datosUbicacion[i].extension;
                    }
                }
            }
            #endregion

            #region Datos Oficina
            if (datosProspectoRequest.oficinas != null)
            {
                int oficinasLenght = datosProspectoRequest.oficinas.Length;
                if (oficinasLenght > 0)
                {
                    datosEntrada.oficinas = new GestionProspecto.DatosOficina[oficinasLenght];
                    for (int i = 0; i < oficinasLenght; i++)
                    {
                        datosEntrada.oficinas[i] = new GestionProspecto.DatosOficina();
                        datosEntrada.oficinas[i].canalDistri = datosProspectoRequest.oficinas[i].canalDistri;
                        datosEntrada.oficinas[i].codSucursal = datosProspectoRequest.oficinas[i].codSucursal;
                        datosEntrada.oficinas[i].idOrgVentas = datosProspectoRequest.oficinas[i].idOrgVentas;
                        datosEntrada.oficinas[i].lugarRadica = datosProspectoRequest.oficinas[i].lugarRadica;
                        datosEntrada.oficinas[i].sector = datosProspectoRequest.oficinas[i].sector;
                    }
                }
            }
            #endregion

            #region Operacion Extrangera
            if (datosProspectoRequest.operExtranjera != null)
            {
                int operExtranjeraLenght = datosProspectoRequest.operExtranjera.Length;
                if (operExtranjeraLenght > 0)
                {
                    datosEntrada.operExtranjera = new GestionProspecto.DatosOperExtranjera[operExtranjeraLenght];
                    for (int i = 0; i < operExtranjeraLenght; i++)
                    {
                        datosEntrada.operExtranjera[i] = new GestionProspecto.DatosOperExtranjera();
                        datosEntrada.operExtranjera[i].ciudad = datosProspectoRequest.operExtranjera[i].ciudad;
                        datosEntrada.operExtranjera[i].departamento = datosProspectoRequest.operExtranjera[i].departamento;
                        datosEntrada.operExtranjera[i].moneda = datosProspectoRequest.operExtranjera[i].moneda;
                        datosEntrada.operExtranjera[i].montoMensual = datosProspectoRequest.operExtranjera[i].montoMensual;
                        datosEntrada.operExtranjera[i].nombreEntidad = datosProspectoRequest.operExtranjera[i].nombreEntidad;
                        datosEntrada.operExtranjera[i].numProducto = datosProspectoRequest.operExtranjera[i].numProducto;
                        datosEntrada.operExtranjera[i].operacion = datosProspectoRequest.operExtranjera[i].operacion;
                        datosEntrada.operExtranjera[i].operacionME = datosProspectoRequest.operExtranjera[i].operacionME;
                        datosEntrada.operExtranjera[i].pais = datosProspectoRequest.operExtranjera[i].pais;
                        datosEntrada.operExtranjera[i].tipoOperME = datosProspectoRequest.operExtranjera[i].tipoOperME;
                        datosEntrada.operExtranjera[i].tipoProducto = datosProspectoRequest.operExtranjera[i].tipoProducto;
                        datosEntrada.operExtranjera[i].montoMensualSpecified = true;
                    }
                }
            }
            else
            {
                datosEntrada.operExtranjera = new GestionProspecto.DatosOperExtranjera[0];
            }
            #endregion

            #region Relaciones
            if (datosProspectoRequest.relaciones != null)
            {
                int relacionesLenght = datosProspectoRequest.relaciones.Length;
                if (relacionesLenght > 0)
                {
                    datosEntrada.relaciones = new GestionProspecto.DatosRelaciones[relacionesLenght];
                    for (int i = 0; i < relacionesLenght; i++)
                    {
                        datosEntrada.relaciones[i] = new GestionProspecto.DatosRelaciones();
                        datosEntrada.relaciones[i].numeroDocumento = datosProspectoRequest.relaciones[i].numeroDocumento;
                        datosEntrada.relaciones[i].operacion = datosProspectoRequest.relaciones[i].operacion;
                        datosEntrada.relaciones[i].tipoDocumento = datosProspectoRequest.relaciones[i].tipoDocumento;
                        datosEntrada.relaciones[i].tipoFuncion = datosProspectoRequest.relaciones[i].tipoFuncion;
                    }
                }
            }
            else
            {
                datosEntrada.relaciones = new GestionProspecto.DatosRelaciones[0];
            }
            #endregion

            #region Residencia fiscaL
            if (datosProspectoRequest.residenciaFiscal != null)
            {
                int residenciaFiscalLenght = datosProspectoRequest.residenciaFiscal.Length;
                if (residenciaFiscalLenght > 0)
                {
                    datosEntrada.residenciaFiscal = new GestionProspecto.DatosResidenciaFiscal[residenciaFiscalLenght];
                    for (int i = 0; i < residenciaFiscalLenght; i++)
                    {
                        datosEntrada.residenciaFiscal[i] = new GestionProspecto.DatosResidenciaFiscal();
                        datosEntrada.residenciaFiscal[i].codPaisFiscal = datosProspectoRequest.residenciaFiscal[i].codPaisFiscal;
                        datosEntrada.residenciaFiscal[i].numTributario = datosProspectoRequest.residenciaFiscal[i].numTributario;
                        datosEntrada.residenciaFiscal[i].operacion = datosProspectoRequest.residenciaFiscal[i].operacion;
                    }
                }
            }
            else
            {
                datosEntrada.residenciaFiscal = new GestionProspecto.DatosResidenciaFiscal[0];
            }
            #endregion

            #region  Val Externas
            if (datosProspectoRequest.valExternas != null)
            {
                int valExternasLenght = datosProspectoRequest.valExternas.Length;
                if (valExternasLenght > 0)
                {
                    datosEntrada.valExternas = new GestionProspecto.DatosValExternas[valExternasLenght];
                    for (int i = 0; i < valExternasLenght; i++)
                    {
                        datosEntrada.valExternas[i] = new GestionProspecto.DatosValExternas();
                        datosEntrada.valExternas[i].codValExterna = datosProspectoRequest.valExternas[i].codValExterna;
                        datosEntrada.valExternas[i].descripcion = datosProspectoRequest.valExternas[i].descripcion;
                        datosEntrada.valExternas[i].tipoValExterna = datosProspectoRequest.valExternas[i].tipoValExterna;
                    }
                }
            }
            else
            {
                datosEntrada.valExternas = new GestionProspecto.DatosValExternas[0];
            }
            #endregion

            datosEntrada.identificacion = new GestionProspecto.Identificacion
            {
                numeroDocumento = datosProspectoRequest.identificacion.numeroDocumento,
                tipoDocumento = datosProspectoRequest.identificacion.tipoDocumento,
            };

            ModificarPersonaNaturalResponse datosSalida = new ModificarPersonaNaturalResponse();

            try
            {
                AppFacadeWS.GestionProspecto.ResponseHeader responseHeader = new GestionProspecto.ResponseHeader();
                responseHeader = gestionarProspecto.modificarPersonaNatural(requestHeader, datosEntrada, out datosSalida);
                DatosProspectoResponse datosResponse = new DatosProspectoResponse();
                //Cargo datos para persona Natural
                if (datosSalida != null)
                {
                    datosResponse.actEconomica = datosSalida.actEconomica;
                    datosResponse.codigoRespuesta = datosSalida.codigoRespuesta;
                    datosResponse.descripcionRespuesta = datosSalida.descripcionRespuesta;
                    datosResponse.numeroDocumento = datosSalida.numeroDocumento;
                    datosResponse.sector = datosSalida.sector;
                    datosResponse.segmento = datosSalida.segmento;
                    datosResponse.subSector = datosSalida.subSector;
                    datosResponse.subSegmento = datosSalida.subSegmento;
                    datosResponse.tipoDocumento = datosSalida.tipoDocumento;
                }
                return datosResponse;
            }
            catch (FaultException<CrmBusinessException> exc)
            {
                DatosProspectoResponse datosResponseError = new DatosProspectoResponse();
                datosResponseError.codigoRespuesta = exc.Detail.genericException.code;
                datosResponseError.descripcionRespuesta = exc.Detail.genericException.description;
                return datosResponseError;
            }
            catch (FaultException<AppFacadeWS.GestionProspecto.SystemException> exsys)
            {
                DatosProspectoResponse datosResponseError = new DatosProspectoResponse();
                datosResponseError.codigoRespuesta = exsys.Detail.genericException.code;
                datosResponseError.descripcionRespuesta = exsys.Detail.genericException.description + ". Facade";
                return datosResponseError;
            }
            catch (Exception ex)
            {
                DatosProspectoResponse datosResponseError = new DatosProspectoResponse();
                datosResponseError.codigoRespuesta = "999";
                datosResponseError.descripcionRespuesta = ex.Message + ". Facade";
                return datosResponseError;
            }
        }
    }
}