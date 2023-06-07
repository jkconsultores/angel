namespace JKCorreos.Modelos
{
    public class GuiaElectronica
    {
        
        public string NumeroDocumentoRemitente { get; set; }

        public string TipoDocumentoRemitente { get; set; }                

        public string RazonSocialRemitente { get; set; }

        public string RucEmisor { get; set; }    

        public string FechaEmision { get; set; }

        public string HoraEmision { get; set; }

        public string RazonSocialDestinatario { get; set; }

        public string RucDestinatario { get; set; }        

        public string MotivoGuia { get; set; }

        public string PesoBruto { get; set; }

        public string FechaEntregaBienes { get; set; }

        public string DireccionPtoPartida { get; set; }
            
        public string DireccionPtoLlegada { get; set; }

        public  string CodigoCliente { get; set; }
        public string NumeroPedido { get; set; }
        public string OrdenCompra { get; set; }
        public string NombreVendedor { get; set; }
        public string CondicionPago { get; set; }
    }
}
