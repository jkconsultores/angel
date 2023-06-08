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
            // Crear una instancia del servicio y procesar los correos
            var correoService = new CorreoService();
            correoService.ProcesarCorreos();

            int timeDelay = _timeDelay;
            string filePath = @"D:\JK Empresa\DescargaXML\20100082803-09-T020-00000059.xml";
            Console.WriteLine("file " + filePath);
            LeerXml(filePath);
            await Task.Delay(timeDelay, stoppingToken);
        }

        private void LeerXml(string filePath)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);

            String xml = "";
            StringWriter sw = new StringWriter();
            XmlTextWriter tx = new XmlTextWriter(sw);
            xmlDoc.WriteTo(tx);
            string str = sw.ToString();

            XmlSerializer serializer = new XmlSerializer(typeof(DespatchAdvice));
            DespatchAdvice ArchivoXml = null;

            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                ArchivoXml = (DespatchAdvice)serializer.Deserialize(fileStream);
            }

            //Guarda los tags en guiaElectronica
            GuiaElectronica guiaElectronica = new GuiaElectronica();
            guiaElectronica.NumeroDocumentoRemitente = ArchivoXml.ID;
            guiaElectronica.TipoDocumentoRemitente = ArchivoXml.DespatchAdviceTypeCode.Value;
            guiaElectronica.RazonSocialRemitente = ArchivoXml.DeliveryCustomerParty.Party.PartyLegalEntity.RegistrationName;
            guiaElectronica.RucEmisor = ArchivoXml.Signature.SignatoryParty.PartyIdentification.ID;
            guiaElectronica.FechaEmision = ArchivoXml.IssueDate;
            guiaElectronica.HoraEmision = ArchivoXml.IssueTime;
            guiaElectronica.RazonSocialDestinatario = ArchivoXml.Shipment.ShipmentStage.CarrierParty.PartyLegalEntity.RegistrationName;
            guiaElectronica.RucDestinatario = ArchivoXml.Shipment.ShipmentStage.CarrierParty.PartyIdentification.ID;
            guiaElectronica.MotivoGuia = ArchivoXml.Shipment.HandlingInstructions;
            guiaElectronica.PesoBruto = ArchivoXml.Shipment.GrossWeightMeasure.Value;
            guiaElectronica.DireccionPtoPartida = ArchivoXml.Shipment.Delivery.Despatch.DespatchAddress.AddressLine.Line;
            guiaElectronica.DireccionPtoLlegada = ArchivoXml.Shipment.Delivery.DeliveryAddress.AddressLine.Line;
            guiaElectronica.FechaEntregaBienes = ArchivoXml.Shipment.ShipmentStage.TransitPeriod.StartDate;
            guiaElectronica.CodigoCliente = ArchivoXml.UBLExtensions.UBLExtension[0].ExtensionContent.AdditionalInformation .AdditionalProperties[4].Value;
            guiaElectronica.NumeroPedido = ArchivoXml.UBLExtensions.UBLExtension[0].ExtensionContent.AdditionalInformation.AdditionalProperties[0].Value;
            guiaElectronica.OrdenCompra = ArchivoXml.UBLExtensions.UBLExtension[0].ExtensionContent.AdditionalInformation.AdditionalProperties[3].Value;
            guiaElectronica.NombreVendedor = ArchivoXml.UBLExtensions.UBLExtension[0].ExtensionContent.AdditionalInformation.AdditionalProperties[2].Value;
            guiaElectronica.CondicionPago = ArchivoXml.UBLExtensions.UBLExtension[0].ExtensionContent.AdditionalInformation.AdditionalProperties[1].Value;

            //Lectura de Tags
            Console.WriteLine("NUMERODOCUMENTOREMITENTE  : " + guiaElectronica.NumeroDocumentoRemitente);            
            Console.WriteLine("TipoDocumentoRemitente : " + guiaElectronica.TipoDocumentoRemitente);
            Console.WriteLine("RazonSocialRemitente : " +   guiaElectronica.RazonSocialRemitente);
            Console.WriteLine("RucEmisor : " +              guiaElectronica.RucEmisor);
            Console.WriteLine("FechaEmision : " +           guiaElectronica.FechaEmision);
            Console.WriteLine("HoraEmision  : " +           guiaElectronica.HoraEmision);
            Console.WriteLine("RazonSocialDestinatario : " +guiaElectronica.RazonSocialDestinatario);
            Console.WriteLine("RucDestinatario : " +        guiaElectronica.RucDestinatario);
            Console.WriteLine("MotivoGuia : " +             guiaElectronica.MotivoGuia);
            Console.WriteLine("PesoBruto : " +              guiaElectronica.PesoBruto);
            Console.WriteLine("DireccionPtoPartida : " +    guiaElectronica.DireccionPtoPartida);
            Console.WriteLine("DireccionPtoLlegada : " +    guiaElectronica.DireccionPtoLlegada);
            Console.WriteLine("FechaEntregaBienes : " +     guiaElectronica.FechaEntregaBienes);
            Console.WriteLine("CodigoCliente : " +          guiaElectronica.CodigoCliente);
            Console.WriteLine("NumeroPedido : " +           guiaElectronica.NumeroPedido);
            Console.WriteLine("OrdenCompra : " +            guiaElectronica.OrdenCompra);
            Console.WriteLine("NombreVendedor : " +         guiaElectronica.NombreVendedor);
            Console.WriteLine("CondicionPago : " +          guiaElectronica.CondicionPago);
            GuardarCabeceraGuia(guiaElectronica);

            //Lectura y guarda el Detalle de la Guia
            
            List<DetalleGuia> detallesDispatch = new List<DetalleGuia>();
            
            foreach (DespatchLine listaDetalle in ArchivoXml.DespatchLines)
            {
                DetalleGuia detalleguia = new DetalleGuia();
                detalleguia.NumeroDocumentoRemitente = ArchivoXml.ID;
                detalleguia.Item = listaDetalle.ID;
                detalleguia.Codigo = listaDetalle.Item.SellersItemIdentification.ID;
                detalleguia.Descripcion = listaDetalle.Item.Description;
                detalleguia.Unidad = listaDetalle.OrderLineReference.LineID;
                detalleguia.Cantidad = listaDetalle.DeliveredQuantity.Value;
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

                SqlCommand command = new SqlCommand("INSERT INTO CABECERAGUIA(NUMERODOCUMENTOREMITENTE, TIPODOCUMENTOREMITENTE, RAZONSOCIALREMITENTE, RUCEMISOR, FECHAEMISION,HORAEMISION, RAZONSOCIALDESTINATARIO, RUCDESTINATARIO, MOTIVOGUIA, PESOBRUTO, DIRECCIONPTOPARTIDA, DIRECCIONPTOLLEGADA, FECHAENTREGABIENES, CODIGOCLIENTE, NUMEROPEDIDO, ORDENCOMPRA, NOMBREVENDEDOR, CONDICIONPAGO) " +
                    "values(@NUMERODOCUMENTOREMITENTE, @TIPODOCUMENTOREMITENTE, @RAZONSOCIALREMITENTE , @RUCEMISOR , @FECHAEMISION , @HORAEMISION, @RAZONSOCIALDESTINATARIO, @RUCDESTINATARIO, @MOTIVOGUIA, @PESOBRUTO, @DIRECCIONPTOPARTIDA, @DIRECCIONPTOLLEGADA, @FECHAENTREGABIENES, @CODIGOCLIENTE, @NUMEROPEDIDO, @ORDENCOMPRA, @NOMBREVENDEDOR, @CONDICIONPAGO)", connection);

                command.Parameters.AddWithValue("@NUMERODOCUMENTOREMITENTE", guiaElectronica.NumeroDocumentoRemitente);
                command.Parameters.AddWithValue("@TIPODOCUMENTOREMITENTE",   guiaElectronica.TipoDocumentoRemitente);
                command.Parameters.AddWithValue("@RAZONSOCIALREMITENTE",     guiaElectronica.RazonSocialRemitente);
                command.Parameters.AddWithValue("@RUCEMISOR",                guiaElectronica.RucEmisor);
                command.Parameters.AddWithValue("@FECHAEMISION",             guiaElectronica.FechaEmision);
                command.Parameters.AddWithValue("@HORAEMISION",              guiaElectronica.HoraEmision);
                command.Parameters.AddWithValue("@RAZONSOCIALDESTINATARIO",  guiaElectronica.RazonSocialDestinatario);
                command.Parameters.AddWithValue("@RUCDESTINATARIO",          guiaElectronica.RucDestinatario);
                command.Parameters.AddWithValue("@MOTIVOGUIA",               guiaElectronica.MotivoGuia);
                command.Parameters.AddWithValue("@PESOBRUTO",                guiaElectronica.PesoBruto);
                command.Parameters.AddWithValue("@DIRECCIONPTOPARTIDA",      guiaElectronica.DireccionPtoPartida);
                command.Parameters.AddWithValue("@DIRECCIONPTOLLEGADA",      guiaElectronica.DireccionPtoLlegada);
                command.Parameters.AddWithValue("@FECHAENTREGABIENES",       guiaElectronica.FechaEntregaBienes);
                command.Parameters.AddWithValue("@CODIGOCLIENTE",            guiaElectronica.CodigoCliente);
                command.Parameters.AddWithValue("@NUMEROPEDIDO",             guiaElectronica.NumeroPedido);
                command.Parameters.AddWithValue("@ORDENCOMPRA",              guiaElectronica.OrdenCompra);
                command.Parameters.AddWithValue("@NOMBREVENDEDOR",           guiaElectronica.NombreVendedor);
                command.Parameters.AddWithValue("@CONDICIONPAGO",            guiaElectronica.CondicionPago);

                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();
                Console.WriteLine("Se guardo guiaElectronica en BD");
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
            }
        }
    }
}