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

// AOP con Castle: instanciar ProxyGenerator y LoggingInterceptor una sola vez
builder.Services.AddSingleton<ProxyGenerator>();
builder.Services.AddSingleton<LoggingInterceptor>();

// Registrar CarroServicio con proxy
builder.Services.AddScoped<ICarroServicio>(provider =>
{
    var repositorio  = provider.GetRequiredService<ICarroRepository>();
    var generator    = provider.GetRequiredService<ProxyGenerator>();
    var interceptor  = provider.GetRequiredService<LoggingInterceptor>();
    var servicioReal = new CarroServicio(repositorio);

    return generator.CreateInterfaceProxyWithTarget<ICarroServicio>(servicioReal, interceptor);
});

// Registrar el listener de eventos
builder.Services.AddScoped<EventoCarroListener>();

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

app.Run();
