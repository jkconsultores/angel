using System.IO;
using System.Net.Mail;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit.Security;
using MimeKit;
using System.IO.Compression;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;


public class CorreoService
{

    private readonly string _correo = "xml@jkconsultores.com";
    private readonly string _contraseña = "ihsiCm&H~#T&";

    //Cambiar la ruta si es necesario

    //Ruta para los xml que serán leídos
    private readonly string _carpetaDestino = @"C:\Users\cesar\OneDrive\Documentos\Proyectos\C#\JkSmartData";
    //Ruta para los archivos restantes
    private readonly string _carpetaRegistro = @"C:\Users\cesar\OneDrive\Documentos\Proyectos\C#\JkSmartData\OTROS";
    
    private readonly string _connectionString = "Data Source=CESAR;Initial Catalog=JK_pruebas;Integrated Security=True";


    public void ProcesarCorreos()
    {
        using (var clienteImap = new ImapClient())
        {
            clienteImap.Connect("mail.jkconsultores.com", 993, SecureSocketOptions.SslOnConnect);
            clienteImap.Authenticate(_correo, _contraseña);

            clienteImap.Inbox.Open(FolderAccess.ReadWrite);
            //var mensajesNoLeidos = clienteImap.Inbox.Search(SearchQuery.NotSeen);

            var mensajes = clienteImap.Inbox.Search(SearchQuery.All);
            
            //cambiar por (mensajesNoLeidos) en el caso de que no se quiera agregar dos veces los datos
            foreach (var uid in mensajes)
            {

                var mensaje = clienteImap.Inbox.GetMessage(uid);

                DateTimeOffset fechaRecepcionOffset = mensaje.Date;
                DateTime fechaRecepcion = fechaRecepcionOffset.UtcDateTime;

           
   
                var remitente = mensaje.From.ToString();
                var destinatario = mensaje.To.ToString();

                foreach (var adjunto in mensaje.Attachments)
                {
                    if (adjunto is MimePart parte)
                    {
                        if (parte.FileName.StartsWith("R"))
                        {
                            var rutaArchivo = Path.Combine(_carpetaRegistro, parte.FileName);

                            using (var archivo = File.Create(rutaArchivo))
                            {
                                parte.Content.DecodeTo(archivo);
                            }
                        }

                        //Esta condicón se puede cambiar según el tipo de correo
                        else if (parte.FileName.EndsWith(".XML"))
                        {
                            var rutaArchivo = Path.Combine(_carpetaDestino, parte.FileName);

                            using (var archivo = File.Create(rutaArchivo))
                            {
                                parte.Content.DecodeTo(archivo);
                            }
                        }


                        /*Cuando el archivo está comprimido, lo descomprimimos y si es un XML
                         se guarda en la carpeta indicada*/
                        else if (parte.FileName.EndsWith(".zip") || parte.FileName.EndsWith(".rar") || parte.FileName.EndsWith(".ZIP") || parte.FileName.EndsWith(".RAR"))
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                parte.Content.DecodeTo(memoryStream);
                                memoryStream.Seek(0, SeekOrigin.Begin);

                                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Read))
                                {
                                    foreach (var entry in archive.Entries)
                                    {
                                        if (!string.IsNullOrEmpty(entry.Name) && entry.Name.EndsWith(".XML"))
                                        {
                                            var rutaArchivo = Path.Combine(_carpetaRegistro, entry.Name);

                                            using (var archivo = File.Create(rutaArchivo))
                                            {
                                                using (var entryStream = entry.Open())
                                                {
                                                    entryStream.CopyTo(archivo);
                                                }
                                            }
                                        }

                                        //el resto lo mandamos a la carpeta de registro
                                        else
                                        {
                                            var rutaArchivo = Path.Combine(_carpetaRegistro, entry.Name);

                                            using (var archivo = File.Create(rutaArchivo))
                                            {
                                                using (var entryStream = entry.Open())
                                                {
                                                    entryStream.CopyTo(archivo);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else if (parte.FileName.EndsWith(".pdf"))
                        {
                            var rutaArchivo = Path.Combine(_carpetaRegistro, parte.FileName);

                            using (var archivo = File.Create(rutaArchivo))
                            {
                                parte.Content.DecodeTo(archivo);
                            }
                        }

                    }

                    // Insertar datos en la base de datos
                    InsertarEnBaseDeDatos(fechaRecepcion, remitente, destinatario, mensaje.Attachments);

                    // Marcar correo como leído
                    clienteImap.Inbox.SetFlags(uid, MessageFlags.Seen, true);
                }
            }

            clienteImap.Disconnect(true);
        }
        Console.WriteLine("Extracción de archivos completada.");
    }

    private void InsertarEnBaseDeDatos(DateTime fechaRecepcion, string remitente, string destinatario, IEnumerable<MimeEntity> adjuntos)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            foreach (var adjunto in adjuntos)
            {
                if (adjunto is MimePart parte)
                {
                    // Obtener la información necesaria del adjunto
                    var tipoArchivo = Path.GetExtension(parte.FileName)?.ToLower();
                    var rutaDescarga = Path.Combine(_carpetaDestino, parte.FileName);

                    // Insertar datos en la tabla RECEPCIONCORREOS
                    var sql = @"INSERT INTO RECEPCION_CORREOS (FECHARECEPCION, FECHACORREO, REMITENTE, DESTINATARIO, XMLADJUNTO, PDFADJUNTO, CDRADJUNTO, OTROSARCHIVOS, RUTADESCARGA)
                                VALUES (@FechaRecepcion, @FechaCorreo, @Remitente, @Destinatario, @XmlAdjunto, @PdfAdjunto, @CdrAdjunto, @OtrosArchivos, @RutaDescarga)";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@FechaRecepcion", fechaRecepcion);
                        command.Parameters.AddWithValue("@FechaCorreo", fechaRecepcion);
                        command.Parameters.AddWithValue("@Remitente", remitente);
                        command.Parameters.AddWithValue("@Destinatario", destinatario);
                        command.Parameters.AddWithValue("@XmlAdjunto", tipoArchivo == ".xml" ? parte.FileName : DBNull.Value);
                        command.Parameters.AddWithValue("@PdfAdjunto", tipoArchivo == ".pdf" ? parte.FileName : DBNull.Value);
                        command.Parameters.AddWithValue("@CdrAdjunto", tipoArchivo == ".cdr" ? parte.FileName : DBNull.Value);
                        command.Parameters.AddWithValue("@OtrosArchivos", tipoArchivo != ".xml" && tipoArchivo != ".pdf" && tipoArchivo != ".cdr" ? parte.FileName : DBNull.Value);
                        command.Parameters.AddWithValue("@RutaDescarga", rutaDescarga);

                        command.ExecuteNonQuery();
                    }
                }
            }
        }
    }

}
