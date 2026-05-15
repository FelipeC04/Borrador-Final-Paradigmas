namespace ConcesionarioAPI.Dominio;

// OOP: clase con propiedades encapsuladas que representan el negocio
public class Carro
{
    public int Id { get; set; }
    public string Marca { get; set; } = string.Empty;
    public string Modelo { get; set; } = string.Empty;
    public int Anio { get; set; }
    public decimal Precio { get; set; }
    public int Kilometraje { get; set; }
    public string Color { get; set; } = string.Empty;
    public string Transmision { get; set; } = "Manual";
    public string Estado { get; set; } = "Disponible"; // Disponible | Vendido | Reservado
}
