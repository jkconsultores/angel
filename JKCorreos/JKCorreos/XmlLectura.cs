using JKCorreos.Modelos;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Xml.Serialization;

namespace JKCorreos
{
    public class XmlLectura : BackgroundService
    {
        private readonly int _timeDelay = 10;
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Crear una instancia del servicio y procesa los correos
            var correoService = new CorreoService();
            correoService.ProcesarCorreos();

            int timeDelay = _timeDelay;
            string filePath = @"D:\JK Empresa\LeerXml\20100082803-09-T020-00000072.xml";

            Console.WriteLine("file " + filePath);
            LeerXml(filePath);
            await Task.Delay(timeDelay, stoppingToken);
        }

        private void LeerXml(string filePath)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);

            // Obtener el valor del atributo schemeID TipoDocumentoRemitente
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xmlDoc.NameTable);
            namespaceManager.AddNamespace("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            namespaceManager.AddNamespace("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
            string schemeIdRemitente = xmlDoc.SelectSingleNode("//cac:DespatchSupplierParty/cac:Party/cac:PartyIdentification/cbc:ID/@schemeID", namespaceManager)?.Value;
            

            // Obtener el valor del atributo schemeID TipoDocumentoDestinatario
            XmlNode idNode = xmlDoc.SelectSingleNode("//cac:DeliveryCustomerParty/cac:Party/cac:PartyIdentification/cbc:ID", namespaceManager);
            string schemeIdDestinatario = idNode.Attributes["schemeID"].Value;
            

            // Obtener el valor del atributo schemeID para TipoDocumentoTransportista
            XmlNamespaceManager namespaceTrans = new XmlNamespaceManager(xmlDoc.NameTable);
            namespaceTrans.AddNamespace("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            namespaceTrans.AddNamespace("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
            XmlNode schemeIDValorTransp = xmlDoc.SelectSingleNode("//cac:PartyIdentification/cbc:ID/@schemeID", namespaceTrans);
            string schemeIDTransporte = schemeIDValorTransp.Value;            

            StringWriter sw = new StringWriter();
            XmlTextWriter tx = new XmlTextWriter(sw);
            xmlDoc.WriteTo(tx);            

            XmlSerializer serializer = new XmlSerializer(typeof(DespatchAdvice));
            DespatchAdvice? ArchivoXml = null;

            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                ArchivoXml = (DespatchAdvice)serializer.Deserialize(fileStream);
            }

            //Guarda los tags en GUIAELECTRONICA
            GuiaElectronica guiaElectronica = new GuiaElectronica();
            guiaElectronica.NumeroDocumentoRemitente =  ArchivoXml.Signature.SignatoryParty.PartyIdentification.ID;
            guiaElectronica.TipoDocumentoRemitente =    schemeIdRemitente;
            guiaElectronica.RazonSocialRemitente =      ArchivoXml.DespatchSupplierParty.Party.PartyLegalEntity.RegistrationName;
            guiaElectronica.SerieNumeroGuia =           ArchivoXml.ID;
            guiaElectronica.TipoDocumentoGuia =         ArchivoXml.DespatchAdviceTypeCode.Value;
            guiaElectronica.FechaEmision =              ArchivoXml.IssueDate;
            guiaElectronica.HoraEmision =               ArchivoXml.IssueTime;
            guiaElectronica.RazonSocialDestinatario =   ArchivoXml.DeliveryCustomerParty.Party.PartyLegalEntity.RegistrationName;
            guiaElectronica.NumeroDocumentoDestinatario=ArchivoXml.DeliveryCustomerParty.Party.PartyIdentification.ID;
            guiaElectronica.TipoDocumentoDestinatario = schemeIdDestinatario;
            guiaElectronica.MotivoGuia =                ArchivoXml.Shipment.HandlingCode.Value;
            guiaElectronica.DescripcionMotivo =         ArchivoXml.Shipment.HandlingInstructions;
            guiaElectronica.PesoBruto =                 ArchivoXml.Shipment.GrossWeightMeasure.Value;
            guiaElectronica.UnidadMedidaPeso =          ArchivoXml.Shipment.GrossWeightMeasure.UnitCode;
            guiaElectronica.ModalidadTraslado =         ArchivoXml.Shipment.ShipmentStage.TransportModeCode.Value;
            
            if (guiaElectronica.ModalidadTraslado == "01")
                {
                guiaElectronica.FechaInicioTraslado = "";
                guiaElectronica.FechaEntregaBienes = ArchivoXml.Shipment.ShipmentStage.TransitPeriod.StartDate;
                }

            else if (guiaElectronica.ModalidadTraslado == "02")
            {
                guiaElectronica.FechaInicioTraslado = ArchivoXml.Shipment.ShipmentStage.TransitPeriod.StartDate;
                guiaElectronica.FechaEntregaBienes = "";
            }

            guiaElectronica.NumeroBultos =              ArchivoXml.Shipment.TotalTransportHandlingUnitQuantity;
            guiaElectronica.UbigeoPtoPartida =          ArchivoXml.Shipment.Delivery.Despatch.DespatchAddress.ID;
            guiaElectronica.CodigoPtoPartida =          ArchivoXml.Shipment.Delivery.Despatch?.DespatchAddress?.AddressTypeCode?.Value ?? "";                     
            guiaElectronica.DireccionPtoPartida =       ArchivoXml.Shipment.Delivery.Despatch.DespatchAddress.AddressLine.Line;
            guiaElectronica.NumeroDocumentoPartida =    ArchivoXml.Shipment.Delivery.Despatch?.DespatchAddress?.AddressTypeCode?.ListID ?? "";                        
            guiaElectronica.UbigeoPtoLLegada =          ArchivoXml.Shipment.Delivery.DeliveryAddress.ID;
            guiaElectronica.CodigoPtoLLegada =          ArchivoXml.Shipment.Delivery.DeliveryAddress?.AddressTypeCode?.Value ?? "";               
            guiaElectronica.DireccionPtoLlegada =       ArchivoXml.Shipment.Delivery.DeliveryAddress?.AddressLine?.Line ?? "";
            guiaElectronica.NumeroDocumentoPtoLlegada = ArchivoXml.Shipment.Delivery.DeliveryAddress?.AddressTypeCode?.ListID ?? "";                       
            guiaElectronica.RazonSocialTransportista =  ArchivoXml.Shipment.ShipmentStage.CarrierParty?.PartyLegalEntity?.RegistrationName ?? "";
            guiaElectronica.NumeroRucTransportista =    ArchivoXml.Shipment.ShipmentStage.CarrierParty?.PartyIdentification?.ID ?? "";
            guiaElectronica.TipoDocumentoTransportista =schemeIDTransporte;                 
            guiaElectronica.CodigoPuerto =              "";
            guiaElectronica.CodigoAeropuerto =          "";
            guiaElectronica.DescripcionPuerto =         "";
                        
            //Los siguientes en blanco
            guiaElectronica.Estado =                "";
            guiaElectronica.SerieNumeroTransporte = "";
            guiaElectronica.FechaRecepcion =        "";
            guiaElectronica.FechaCanje =            "";
            guiaElectronica.UsuarioCanje =          "";


            //Lectura de Tags
            Console.WriteLine("------LECTURA DE DATOS XML:--------" );
            Console.WriteLine("NUMERODOCUMENTOREMITENTE :"+ guiaElectronica.NumeroDocumentoRemitente);
            Console.WriteLine("TipoDocumentoRemitente : " + guiaElectronica.TipoDocumentoRemitente);
            Console.WriteLine("RazonSocialRemitente : " +   guiaElectronica.RazonSocialRemitente);
            Console.WriteLine("SerieNumeroGuia : " +        guiaElectronica.SerieNumeroGuia);
            Console.WriteLine("TIPODOCUMENTOGUIA : " +      guiaElectronica.TipoDocumentoGuia);
            Console.WriteLine("FechaEmision : " +           guiaElectronica.FechaEmision);
            Console.WriteLine("HoraEmision  : " +           guiaElectronica.HoraEmision);
            Console.WriteLine("RazonSocialDestinatario : " +guiaElectronica.RazonSocialDestinatario);
            Console.WriteLine("NumrDocumentoDestinatario:"+ guiaElectronica.NumeroDocumentoDestinatario);
            Console.WriteLine("TipoDocumentoDestinatario:" +guiaElectronica.TipoDocumentoDestinatario);
            Console.WriteLine("MotivoGuia : " +             guiaElectronica.MotivoGuia);
            Console.WriteLine("DescripcionMotivo : " +      guiaElectronica.DescripcionMotivo);
            Console.WriteLine("PesoBruto : " +              guiaElectronica.PesoBruto);
            Console.WriteLine("UnidadMedidaPeso : " +       guiaElectronica.UnidadMedidaPeso);
            Console.WriteLine("FechaInicioTraslado : " +    guiaElectronica.FechaInicioTraslado);
            Console.WriteLine("FechaEntregaBienes : " +     guiaElectronica.FechaEntregaBienes);
            Console.WriteLine("NumeroBultos : " +           guiaElectronica.NumeroBultos);
            Console.WriteLine("UbigeoPtoPartida : " +       guiaElectronica.UbigeoPtoPartida);
            Console.WriteLine("CodigoPtoPartida : " +       guiaElectronica.CodigoPtoPartida); 
            Console.WriteLine("DireccionPtoPartida : " +    guiaElectronica.DireccionPtoPartida);
            Console.WriteLine("NumeroDocumentoPartida : " + guiaElectronica.NumeroDocumentoPartida);
            Console.WriteLine("UbigeoPtoLLegada : " +       guiaElectronica.UbigeoPtoLLegada);
            Console.WriteLine("CodigoPtoLLegada : " +       guiaElectronica.CodigoPtoLLegada);
            Console.WriteLine("DireccionPtoLlegada : " +    guiaElectronica.DireccionPtoLlegada);
            Console.WriteLine("NumeroDocumentoPtoLlegada :"+guiaElectronica.NumeroDocumentoPtoLlegada);
            Console.WriteLine("NumeroRucTransportista : " + guiaElectronica.NumeroRucTransportista);
            Console.WriteLine("TipoDocumentoTransportista:"+guiaElectronica.TipoDocumentoTransportista);
            Console.WriteLine("CodigoPuerto : " +           guiaElectronica.CodigoPuerto);
            Console.WriteLine("CodigoAeropuerto : " +       guiaElectronica.CodigoAeropuerto);
            Console.WriteLine("DescripcionPuerto : " +      guiaElectronica.DescripcionPuerto);
            Console.WriteLine("Estado : " +                 guiaElectronica.Estado);
            Console.WriteLine("SerieNumeroTransporte : " +  guiaElectronica.SerieNumeroTransporte);
            Console.WriteLine("FechaRecepcion : " +         guiaElectronica.FechaRecepcion);
            Console.WriteLine("FechaCanje : " +             guiaElectronica.FechaCanje);
            Console.WriteLine("UsuarioCanje : " +           guiaElectronica.UsuarioCanje);            
            GuardarCabeceraGuia(guiaElectronica);

            //Lectura y guarda el Detalle de la Guia            
            List<DetalleGuia> detallesDispatch = new List<DetalleGuia>();
            
            foreach (DespatchLine listaDetalle in ArchivoXml.DespatchLines)
            {
                DetalleGuia detalleguia = new DetalleGuia();
                detalleguia.NumeroDocumentoRemitente = ArchivoXml.ID;
                detalleguia.Item =       listaDetalle.ID;
                detalleguia.Codigo =     listaDetalle.Item.SellersItemIdentification.ID;
                detalleguia.Descripcion =listaDetalle.Item.Description;
                detalleguia.Unidad =     listaDetalle.DeliveredQuantity.UnitCode;
                detalleguia.Cantidad =   listaDetalle.DeliveredQuantity.Value;
                detallesDispatch.Add(detalleguia);
            }
            
            foreach (DetalleGuia DetalleGuiaCompleto in detallesDispatch)
            {
                GuardarDetalleGuia(DetalleGuiaCompleto);
            }  

        }                

        private void GuardarCabeceraGuia(GuiaElectronica guiaElectronica)
        {
            // Conexión Base de datos
            SqlConnectionStringBuilder connectionStringBuilder = new();                      
            string connectionString = "Data Source=DESKTOP-D7VGCJ9;Initial Catalog=JKCORREO;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {                    
                connection.Open();
                Console.WriteLine("Conectado : " + connection.State);                

                SqlCommand command = new SqlCommand("INSERT INTO CABECERAGUIA(NumeroDocumentoRemitente, TipoDocumentoRemitente, RazonSocialRemitente," +
                    " SerieNumeroGuia, TipoDocumentoGuia, FechaEmision, HoraEmision, RazonSocialDestinatario, NumeroDocumentoDestinatario, TipoDocumentoDestinatario," +
                    " MotivoGuia, DescripcionMotivo, PesoBruto, UnidadMedidaPeso, ModalidadTraslado, FechaInicioTraslado, FechaEntregaBienes, NumeroBultos, UbigeoPtoPartida," +
                    "CodigoPtoPartida, DireccionPtoPartida, NumeroDocumentoPartida, UbigeoPtoLLegada, CodigoPtoLLegada, DireccionPtoLlegada,NumeroDocumentoPtoLlegada," +
                    "RazonSocialTransportista, NumeroRucTransportista, TipoDocumentoTransportista, CodigoPuerto, CodigoAeropuerto, DescripcionPuerto, Estado, " +
                    "SerieNumeroTransporte, FechaRecepcion, FechaCanje, UsuarioCanje) " +
                    "values(@NumeroDocumentoRemitente, @TipoDocumentoRemitente, @RazonSocialRemitente , @SerieNumeroGuia , @TipoDocumentoGuia , @FechaEmision," +
                    "@HoraEmision, @RazonSocialDestinatario, @NumeroDocumentoDestinatario, @TipoDocumentoDestinatario, @MotivoGuia, @DescripcionMotivo, @PesoBruto," +
                    "@UnidadMedidaPeso, @ModalidadTraslado, @FechaInicioTraslado, @FechaEntregaBienes, @NumeroBultos, @UbigeoPtoPartida, @CodigoPtoPartida, @DireccionPtoPartida," +
                    "@NumeroDocumentoPartida, @UbigeoPtoLLegada, @CodigoPtoLLegada, @DireccionPtoLlegada, @NumeroDocumentoPtoLlegada, @RazonSocialTransportista, @NumeroRucTransportista," +
                    "@TipoDocumentoTransportista, @CodigoPuerto, @CodigoAeropuerto, @DescripcionPuerto, @Estado, @SerieNumeroTransporte, @FechaRecepcion," +
                    "@FechaCanje, @UsuarioCanje)", connection);                                

                command.Parameters.AddWithValue("@NumeroDocumentoRemitente",    guiaElectronica.NumeroDocumentoRemitente);
                command.Parameters.AddWithValue("@TipoDocumentoRemitente",      guiaElectronica.TipoDocumentoRemitente);
                command.Parameters.AddWithValue("@RazonSocialRemitente",        guiaElectronica.RazonSocialRemitente);
                command.Parameters.AddWithValue("@SerieNumeroGuia",             guiaElectronica.SerieNumeroGuia);
                command.Parameters.AddWithValue("@TipoDocumentoGuia",           guiaElectronica.TipoDocumentoGuia);
                command.Parameters.AddWithValue("@FechaEmision",                guiaElectronica.FechaEmision);
                command.Parameters.AddWithValue("@HoraEmision",                 guiaElectronica.HoraEmision);
                command.Parameters.AddWithValue("@RazonSocialDestinatario",     guiaElectronica.RazonSocialDestinatario);
                command.Parameters.AddWithValue("@NumeroDocumentoDestinatario", guiaElectronica.NumeroDocumentoDestinatario);
                command.Parameters.AddWithValue("@TipoDocumentoDestinatario",   guiaElectronica.TipoDocumentoDestinatario);
                command.Parameters.AddWithValue("@MotivoGuia",                  guiaElectronica.MotivoGuia);
                command.Parameters.AddWithValue("@DescripcionMotivo",           guiaElectronica.DescripcionMotivo);
                command.Parameters.AddWithValue("@PesoBruto",                   guiaElectronica.PesoBruto);
                command.Parameters.AddWithValue("@UnidadMedidaPeso",            guiaElectronica.UnidadMedidaPeso);
                command.Parameters.AddWithValue("@ModalidadTraslado",           guiaElectronica.ModalidadTraslado);
                command.Parameters.AddWithValue("@FechaInicioTraslado",         guiaElectronica.FechaInicioTraslado);
                command.Parameters.AddWithValue("@FechaEntregaBienes",          guiaElectronica.FechaEntregaBienes);
                command.Parameters.AddWithValue("@NumeroBultos",                guiaElectronica.NumeroBultos);
                command.Parameters.AddWithValue("@UbigeoPtoPartida",            guiaElectronica.UbigeoPtoPartida);
                command.Parameters.AddWithValue("@CodigoPtoPartida",            guiaElectronica.CodigoPtoPartida != null ? guiaElectronica.CodigoPtoPartida : DBNull.Value);
                command.Parameters.AddWithValue("@DireccionPtoPartida",         guiaElectronica.DireccionPtoPartida);
                command.Parameters.AddWithValue("@NumeroDocumentoPartida",      guiaElectronica.NumeroDocumentoPartida != null ? guiaElectronica.NumeroDocumentoPartida : DBNull.Value); 
                command.Parameters.AddWithValue("@UbigeoPtoLLegada",            guiaElectronica.UbigeoPtoLLegada);
                command.Parameters.AddWithValue("@CodigoPtoLLegada",            guiaElectronica.CodigoPtoLLegada != null ? guiaElectronica.CodigoPtoLLegada : DBNull.Value);
                command.Parameters.AddWithValue("@DireccionPtoLlegada",         guiaElectronica.DireccionPtoLlegada);
                command.Parameters.AddWithValue("@NumeroDocumentoPtoLlegada",   guiaElectronica.NumeroDocumentoPtoLlegada != null ? guiaElectronica.NumeroDocumentoPtoLlegada : DBNull.Value);
                command.Parameters.AddWithValue("@RazonSocialTransportista",    guiaElectronica.RazonSocialTransportista != null ? guiaElectronica.RazonSocialTransportista : DBNull.Value);
                command.Parameters.AddWithValue("@NumeroRucTransportista",      guiaElectronica.NumeroRucTransportista != null ? guiaElectronica.NumeroRucTransportista : DBNull.Value);
                command.Parameters.AddWithValue("@TipoDocumentoTransportista",  guiaElectronica.TipoDocumentoTransportista);
                command.Parameters.AddWithValue("@CodigoPuerto",                guiaElectronica.CodigoPuerto);
                command.Parameters.AddWithValue("@CodigoAeropuerto",            guiaElectronica.CodigoAeropuerto);
                command.Parameters.AddWithValue("@DescripcionPuerto",           guiaElectronica.DescripcionPuerto);

                command.Parameters.AddWithValue("@Estado",                      guiaElectronica.Estado);
                command.Parameters.AddWithValue("@SerieNumeroTransporte",       guiaElectronica.SerieNumeroTransporte);
                command.Parameters.AddWithValue("@FechaRecepcion",              guiaElectronica.FechaRecepcion);
                command.Parameters.AddWithValue("@FechaCanje",                  guiaElectronica.FechaCanje);
                command.Parameters.AddWithValue("@UsuarioCanje",                guiaElectronica.UsuarioCanje);

                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();
                Console.WriteLine("Se guardo guiaElectronica en BD");
                connection.Close();
            }
        }

        private void GuardarDetalleGuia(DetalleGuia detalleguia)
        {
            // Conexión Base de datos
            SqlConnectionStringBuilder connectionStringBuilder = new();
            string connectionString = "Data Source=DESKTOP-D7VGCJ9;Initial Catalog=JKCORREO;Integrated Security=True";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                Console.WriteLine("Conectado : " + connection.State);

                SqlCommand command = new SqlCommand("INSERT INTO DETALLEGUIA(NUMERODOCUMENTOREMITENTE, NUMEROITEM, CODIGO, DESCRIPCION, UNIDAD, CANTIDAD) " +
                    "values(@NUMERODOCUMENTOREMITENTE, @NUMEROITEM, @CODIGO, @DESCRIPCION, @UNIDAD, @CANTIDAD)", connection);

                command.Parameters.AddWithValue("@NUMERODOCUMENTOREMITENTE", detalleguia.NumeroDocumentoRemitente);
                command.Parameters.AddWithValue("@NUMEROITEM",               detalleguia.Item);
                command.Parameters.AddWithValue("@CODIGO",                   detalleguia.Codigo);
                command.Parameters.AddWithValue("@DESCRIPCION" ,             detalleguia.Descripcion);
                command.Parameters.AddWithValue("@UNIDAD",                   detalleguia.Unidad);
                command.Parameters.AddWithValue("@CANTIDAD",                 detalleguia.Cantidad);
                                 
                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();
                Console.WriteLine("Se guardo detalleguia en BD");
                connection.Close();
            }
        }
    }
}