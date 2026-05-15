using Castle.DynamicProxy;

namespace ConcesionarioAPI.Aspectos;

// ORIENTADO A ASPECTOS con Castle DynamicProxy
// Este interceptor se "envuelve" alrededor del servicio.
// Cada vez que se llama un método, Castle lo captura aquí primero.
// Así añadimos logging sin tocar nada en CarroServicio.

public class LoggingInterceptor : IInterceptor
{
    public void Intercept(IInvocation invocation)
    {
        var metodo = invocation.Method.Name;

        // ANTES de ejecutar el método
        Console.WriteLine($"[AOP] Ejecutando: {metodo}");

        invocation.Proceed(); // Llama al método real

        // DESPUÉS de ejecutar el método
        Console.WriteLine($"[AOP] Terminado: {metodo}");
    }
}
