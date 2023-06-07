using System.IO;
using System.Net.Mail;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit.Security;
using MimeKit;
using System.IO.Compression;

public class CorreoService
{
    private readonly string _correo = "xml@jkconsultores.com";
    private readonly string _contraseña = "ihsiCm&H~#T&";
    //Cambiar la ruta si es necesario

    //Ruta para los xml que serán leídos
    private readonly string _carpetaDestino = @"C:\Users\cesar\OneDrive\Documentos\Proyectos\C#\angel-main\Correos\XMLS";
    //Ruta para los archivos restantes
    private readonly string _carpetaRegistro = @"C:\Users\cesar\OneDrive\Documentos\Proyectos\C#\angel-main\Correos\Bitacora";

    public void ProcesarCorreos()
    {
        using (var clienteImap = new ImapClient())
        {
            clienteImap.Connect("mail.jkconsultores.com", 993, SecureSocketOptions.SslOnConnect);
            clienteImap.Authenticate(_correo, _contraseña);

            clienteImap.Inbox.Open(FolderAccess.ReadOnly);

            var mensajes = clienteImap.Inbox.Search(SearchQuery.All);

            foreach (var uid in mensajes)
            {
                var mensaje = clienteImap.Inbox.GetMessage(uid);

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

  
                }
            }

            clienteImap.Disconnect(true);
        }
        Console.WriteLine("Extracción de archivos completada.");
    }

    
}
