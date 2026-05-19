using ConcesionarioAPI.Dominio;

namespace ConcesionarioAPI.Servicios;

// Clase dedicada a reaccionar a los eventos de carro
public class EventoCarroListener
{
    private readonly ICarroServicio _servicio;

    public EventoCarroListener(ICarroServicio servicio)
    {
        _servicio = servicio;
        
        // Suscribirse al evento CarroCreado
        if (servicio is CarroServicio carroServicio)
        {
            carroServicio.CarroCreado += AlCarroCreado;
        }
    }

    // Reacción al evento: se ejecuta automáticamente cuando se crea un carro
    private void AlCarroCreado(object? sender, Carro carro)
    {
        Console.WriteLine($"[EVENTO] ✨ Carro creado exitosamente: {carro.Marca} {carro.Modelo} (Id: {carro.Id})");
    }
}
