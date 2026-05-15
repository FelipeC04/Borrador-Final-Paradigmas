namespace ConcesionarioAPI.Dominio;

// SRP: DTOs separados, cada uno con una sola responsabilidad

public class CrearCarroDto
{
    public string Marca { get; set; } = string.Empty;
    public string Modelo { get; set; } = string.Empty;
    public int Anio { get; set; }
    public decimal Precio { get; set; }
    public int Kilometraje { get; set; }
    public string Color { get; set; } = string.Empty;
    public string Transmision { get; set; } = "Manual";
    public string Estado { get; set; } = "Disponible";
}

public class ActualizarCarroDto
{
    public string? Marca { get; set; }
    public string? Modelo { get; set; }
    public int? Anio { get; set; }
    public decimal? Precio { get; set; }
    public int? Kilometraje { get; set; }
    public string? Color { get; set; }
    public string? Transmision { get; set; }
    public string? Estado { get; set; }
}
