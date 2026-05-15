namespace ConcesionarioAPI.Middlewares;

// ORIENTADO A ASPECTOS (AOP)
// El middleware intercepta TODAS las peticiones HTTP antes y después
// de que lleguen al controlador, sin tocar la lógica de negocio.
// Es el equivalente nativo de AOP en ASP.NET — mismo concepto que un interceptor.

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;

    public LoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var metodo = context.Request.Method;
        var ruta   = context.Request.Path;

        // Ignora peticiones a Swagger, OpenAPI y health checks
        if (ruta.StartsWithSegments("/swagger") || 
            ruta.StartsWithSegments("/openapi") ||
            ruta.StartsWithSegments("/health") ||
            ruta.Value?.EndsWith(".json") == true)
        {
            await _next(context);
            return;
        }

        var inicio = DateTime.Now;

        // ANTES del controlador (advice "before")
        Console.WriteLine($"[AOP] ► {metodo} {ruta} — {inicio:HH:mm:ss}");

        await _next(context); // Ejecuta el controlador

        // DESPUÉS del controlador (advice "after")
        var duracion = (DateTime.Now - inicio).TotalMilliseconds;
        var status   = context.Response.StatusCode;
        Console.WriteLine($"[AOP] ✔ {metodo} {ruta} — Status: {status} — {duracion:F0}ms");
    }
}
