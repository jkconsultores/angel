using JKCorreos;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHostedService<XmlLectura>();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.Run();