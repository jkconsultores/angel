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

        public string NumeroDocumentoDestinatario { get; set; }        

        public string MotivoGuia { get; set; }

        public string PesoBruto { get; set; }

        public string FechaEntregaBienes { get; set; }

        public string DireccionPtoPartida { get; set; }
            
        public string DireccionPtoLlegada { get; set; }
        public string DireccionPtoLLegada { get; internal set; }
        public  string CodigoCliente { get; set; }
        public string NumeroPedido { get; set; }
        public string OrdenCompra { get; set; }
        public string NombreVendedor { get; set; }
        public string CondicionPago { get; set; }
        public string TipoDocumentoGuia { get; internal set; }
        public string SerieNumeroGuia { get; internal set; }
        public string DescripcionMotivo { get; internal set; }
        public string UnidadMedidaPeso { get; internal set; }
        public string NumeroBultos { get; internal set; }
        public string UbigeoPtoPartida { get; internal set; }
        public string CodigoPtoPartida { get; internal set; }
        public string UbigeoPtoLLegada { get; internal set; }
        public string CodigoPtoLLegada { get; internal set; }
        public string NumeroDocumentoPartida { get; internal set; }
        public string NumeroDocumentoLlegada { get; internal set; }
    }
}
