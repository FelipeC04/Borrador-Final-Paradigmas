using ConcesionarioAPI.Dominio;
using ConcesionarioAPI.Repositorio;

namespace ConcesionarioAPI.Servicios;

// ISP: interfaz con solo los métodos que necesita el controlador
public interface ICarroServicio
{
    IEnumerable<Carro> ObtenerTodos();
    IEnumerable<Carro> ObtenerDisponibles();
    IEnumerable<Carro> BuscarPorMarca(string marca);
    IEnumerable<Carro> FiltrarPorPrecioMaximo(decimal precioMax);
    Carro? ObtenerPorId(int id);
    Carro Crear(CrearCarroDto dto);
    Carro? Actualizar(int id, ActualizarCarroDto dto);
    bool Eliminar(int id);
}

// SRP: esta clase solo contiene la lógica de negocio del concesionario
// OOP: usa inyección de dependencias en el constructor
public class CarroServicio : ICarroServicio
{
    private readonly ICarroRepository _repositorio;

    // ORIENTADO A EVENTOS: evento que se dispara cuando se crea un carro
    public event EventHandler<Carro>? CarroCreado;

    // DIP: recibe la abstracción, no la clase concreta
    public CarroServicio(ICarroRepository repositorio)
    {
        _repositorio = repositorio;
    }

    // LINQ: devuelve todos ordenados por precio (lazy evaluation)
    public IEnumerable<Carro> ObtenerTodos() =>
        _repositorio.ObtenerTodos()
                    .OrderBy(c => c.Precio);

    // LINQ: filtra solo los disponibles (lazy evaluation)
    public IEnumerable<Carro> ObtenerDisponibles() =>
        _repositorio.ObtenerTodos()
                    .Where(c => c.Estado == "Disponible")
                    .OrderBy(c => c.Precio);

    // LINQ: búsqueda parcial por marca sin importar mayúsculas (lazy evaluation)
    public IEnumerable<Carro> BuscarPorMarca(string marca) =>
        _repositorio.ObtenerTodos()
                    .Where(c => c.Marca.Contains(marca, StringComparison.OrdinalIgnoreCase));

    // LINQ: filtra por precio máximo (lazy evaluation)
    public IEnumerable<Carro> FiltrarPorPrecioMaximo(decimal precioMax) =>
        _repositorio.ObtenerTodos()
                    .Where(c => c.Precio <= precioMax)
                    .OrderBy(c => c.Precio);

    public Carro? ObtenerPorId(int id) => _repositorio.ObtenerPorId(id);

    public Carro Crear(CrearCarroDto dto)
    {
        var carro = new Carro
        {
            Marca       = dto.Marca,
            Modelo      = dto.Modelo,
            Anio        = dto.Anio,
            Precio      = dto.Precio,
            Kilometraje = dto.Kilometraje,
            Color       = dto.Color,
            Transmision = dto.Transmision,
            Estado      = dto.Estado
        };

        var creado = _repositorio.Crear(carro);

        // ORIENTADO A EVENTOS: notifica a los suscriptores que se creó un carro
        CarroCreado?.Invoke(this, creado);

        return creado;
    }

    public Carro? Actualizar(int id, ActualizarCarroDto dto) =>
        _repositorio.Actualizar(id, dto);

    public bool Eliminar(int id) => _repositorio.Eliminar(id);
}
