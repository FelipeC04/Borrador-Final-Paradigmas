using CsvHelper;
using CsvHelper.Configuration;
using ConcesionarioAPI.Dominio;
using System.Globalization;

namespace ConcesionarioAPI.Repositorio;

// DIP: el servicio depende de esta interfaz, no de la implementación concreta
public interface ICarroRepository
{
    List<Carro> ObtenerTodos();
    Carro? ObtenerPorId(int id);
    Carro Crear(Carro carro);
    Carro? Actualizar(int id, ActualizarCarroDto dto);
    bool Eliminar(int id);
}

// SRP: esta clase solo se encarga de leer y escribir datos en CSV
public class CarroCsvRepository : ICarroRepository
{
    private readonly string _rutaCsv;

    private static readonly CsvConfiguration _config = new(CultureInfo.InvariantCulture)
    {
        HasHeaderRecord = true,
        MissingFieldFound = null
    };

    public CarroCsvRepository()
    {
        _rutaCsv = Path.Combine(AppContext.BaseDirectory, "carros.csv");

        // Si el archivo no existe, se crea con las cabeceras automáticamente
        if (!File.Exists(_rutaCsv))
        {
            using var writer = new StreamWriter(_rutaCsv);
            using var csv = new CsvWriter(writer, _config);
            csv.WriteHeader<Carro>();
            csv.NextRecord();
        }
    }

    // Lee todos los registros del CSV
    private List<Carro> Leer()
    {
        using var reader = new StreamReader(_rutaCsv);
        using var csv = new CsvReader(reader, _config);
        return csv.GetRecords<Carro>().ToList();
    }

    // Sobreescribe el CSV completo con la lista en memoria
    private void Guardar(List<Carro> carros)
    {
        using var writer = new StreamWriter(_rutaCsv, append: false);
        using var csv = new CsvWriter(writer, _config);
        csv.WriteRecords(carros);
    }

    public List<Carro> ObtenerTodos() => Leer();

    public Carro? ObtenerPorId(int id) =>
        Leer().FirstOrDefault(c => c.Id == id); // LINQ

    public Carro Crear(Carro carro)
    {
        var carros = Leer();

        // LINQ: calcular el siguiente Id disponible
        carro.Id = carros.Any() ? carros.Max(c => c.Id) + 1 : 1;

        carros.Add(carro);
        Guardar(carros);
        return carro;
    }

    public Carro? Actualizar(int id, ActualizarCarroDto dto)
    {
        var carros = Leer();

        // LINQ: buscar el carro a modificar
        var carro = carros.FirstOrDefault(c => c.Id == id);
        if (carro is null) return null;

        // Solo actualiza los campos que vienen en el DTO (actualización parcial)
        if (dto.Marca       != null) carro.Marca       = dto.Marca;
        if (dto.Modelo      != null) carro.Modelo      = dto.Modelo;
        if (dto.Anio        != null) carro.Anio        = dto.Anio.Value;
        if (dto.Precio      != null) carro.Precio      = dto.Precio.Value;
        if (dto.Kilometraje != null) carro.Kilometraje = dto.Kilometraje.Value;
        if (dto.Color       != null) carro.Color       = dto.Color;
        if (dto.Transmision != null) carro.Transmision = dto.Transmision;
        if (dto.Estado      != null) carro.Estado      = dto.Estado;

        Guardar(carros);
        return carro;
    }

    public bool Eliminar(int id)
    {
        var carros = Leer();
        var carro = carros.FirstOrDefault(c => c.Id == id); // LINQ
        if (carro is null) return false;

        carros.Remove(carro);
        Guardar(carros);
        return true;
    }
}
