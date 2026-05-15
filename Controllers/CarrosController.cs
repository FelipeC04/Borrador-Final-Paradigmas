using Microsoft.AspNetCore.Mvc;
using ConcesionarioAPI.Dominio;
using ConcesionarioAPI.Servicios;

namespace ConcesionarioAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarrosController : ControllerBase
{
    // INYECCIÓN DE DEPENDENCIAS en el constructor (DIP)
    private readonly ICarroServicio _servicio;

    public CarrosController(ICarroServicio servicio)
    {
        _servicio = servicio;
    }

    // GET api/carros
    [HttpGet]
    public ActionResult<List<Carro>> ObtenerTodos()
    {
        var carros = _servicio.ObtenerTodos();
        return Ok(carros);
    }

    // GET api/carros/5
    [HttpGet("{id}")]
    public ActionResult<Carro> ObtenerPorId(int id)
    {
        var carro = _servicio.ObtenerPorId(id);
        if (carro is null) return NotFound($"No existe un carro con Id {id}.");
        return Ok(carro);
    }

    // GET api/carros/disponibles
    [HttpGet("disponibles")]
    public ActionResult<List<Carro>> ObtenerDisponibles()
    {
        return Ok(_servicio.ObtenerDisponibles());
    }

    // GET api/carros/buscar?marca=Toyota
    [HttpGet("buscar")]
    public ActionResult<List<Carro>> BuscarPorMarca([FromQuery] string marca)
    {
        return Ok(_servicio.BuscarPorMarca(marca));
    }

    // GET api/carros/precio?max=25000
    [HttpGet("precio")]
    public ActionResult<List<Carro>> FiltrarPorPrecio([FromQuery] decimal max)
    {
        return Ok(_servicio.FiltrarPorPrecioMaximo(max));
    }

    // POST api/carros
    [HttpPost]
    public ActionResult<Carro> Crear([FromBody] CrearCarroDto dto)
    {
        var creado = _servicio.Crear(dto);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = creado.Id }, creado);
    }

    // PUT api/carros/5
    [HttpPut("{id}")]
    public ActionResult<Carro> Actualizar(int id, [FromBody] ActualizarCarroDto dto)
    {
        var actualizado = _servicio.Actualizar(id, dto);
        if (actualizado is null) return NotFound($"No existe un carro con Id {id}.");
        return Ok(actualizado);
    }

    // DELETE api/carros/5
    [HttpDelete("{id}")]
    public IActionResult Eliminar(int id)
    {
        var eliminado = _servicio.Eliminar(id);
        if (!eliminado) return NotFound($"No existe un carro con Id {id}.");
        return NoContent();
    }
}
