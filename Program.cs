using Castle.DynamicProxy;
using ConcesionarioAPI.Aspectos;
using ConcesionarioAPI.Middlewares;
using ConcesionarioAPI.Repositorio;
using ConcesionarioAPI.Servicios;

var builder = WebApplication.CreateBuilder(args);

// ── Servicios base de ASP.NET ────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ── Inyección de dependencias ────────────────────────────────
builder.Services.AddSingleton<ICarroRepository, CarroCsvRepository>();

// AOP con Castle: en vez de registrar CarroServicio directamente,
// lo envolvemos en un proxy que intercepta cada llamada con LoggingInterceptor
builder.Services.AddScoped<ICarroServicio>(provider =>
{
    var repositorio  = provider.GetRequiredService<ICarroRepository>();
    var servicioReal = new CarroServicio(repositorio);
    var generator    = new ProxyGenerator();
    var interceptor  = new LoggingInterceptor();

    // Castle crea un proxy que "rodea" el servicio real
    return generator.CreateInterfaceProxyWithTarget<ICarroServicio>(servicioReal, interceptor);
});

var app = builder.Build();

// ── Pipeline HTTP ────────────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<LoggingMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// ORIENTADO A EVENTOS: suscribirse al evento CarroCreado
using (var scope = app.Services.CreateScope())
{
    var servicio = scope.ServiceProvider.GetRequiredService<ICarroServicio>() as CarroServicio;
    if (servicio != null)
    {
        servicio.CarroCreado += (sender, carro) =>
            Console.WriteLine($"[EVENTO] Carro creado: {carro.Marca} {carro.Modelo} - Id {carro.Id}");
    }
}

app.Run();
