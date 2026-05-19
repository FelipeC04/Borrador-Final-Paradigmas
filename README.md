# 🚗 API Concesionario - Documentación Completa

Un proyecto de API REST hecho con **.NET 8** para gestionar carros en un concesionario. Es como tener un sistema completo que registra, busca, actualiza y elimina datos de carros que se venden.

**Este es un proyecto educativo** donde aprendemos patrones de arquitectura moderna, buenas prácticas de programación y cómo estructurar una API profesional.

## 🎯 ¿Qué hace?

Esta API te permite:
- ✅ **Ver todos los carros** ordenados por precio
- ✅ **Buscar carros** por marca (con búsqueda parcial)
- ✅ **Filtrar carros** por precio máximo
- ✅ **Ver solo carros disponibles** para vender
- ✅ **Obtener un carro específico** por ID
- ✅ **Crear un nuevo carro** con todos sus datos
- ✅ **Actualizar datos** de un carro (puedes cambiar solo lo que quieras)
- ✅ **Eliminar un carro** del sistema
- ✅ **Logging automático** de todas las operaciones en consola
- ✅ **Eventos** que notifican cuando se crea un carro

## 🛠️ ¿Cómo instalar y ejecutar?

### Requisitos previos
- **Visual Studio Community 2026** (o VS Code)
- **.NET 8 SDK** instalado
- **Git** (para clonar el repositorio, opcional)

### Pasos de instalación

#### 1️⃣ Clonar o descargar el proyecto
```bash
# Si usas Git
git clone https://github.com/FelipeC04/Borrador-Final-Paradigmas.git
cd ConcesionarioAPI

# Si descargaste como ZIP, solo descomprime y abre la carpeta
```

#### 2️⃣ Abrir en Visual Studio
- Abre Visual Studio
- Archivo → Abrir proyecto/solución
- Selecciona `ConcesionarioAPI.csproj`

#### 3️⃣ Restaurar dependencias (automático)
Visual Studio lo hace solo, pero si no:
```bash
dotnet restore
```

#### 4️⃣ Ejecutar el proyecto
- Presiona **F5** o Ctrl+F5 en Visual Studio
- O en terminal:
```bash
dotnet run
```

#### 5️⃣ Abrir Swagger (la interfaz visual)
La aplicación te dice el puerto en la consola:
```
info: Microsoft.Hosting.Lifetime[14]
        Now listening on: https://localhost:7001
```

- Abre en tu navegador: **`https://localhost:7001/swagger`**
- Aquí ves todos los endpoints y puedes hacer pruebas sin escribir código

## 📁 Estructura del proyecto - ¿Para qué sirve cada carpeta?

```
ConcesionarioAPI/
│
├── 📂 Controllers/
│   └── CarrosController.cs
│       → Define los ENDPOINTS (las rutas de la API)
│       → GET /api/carros, POST /api/carros, etc.
│       → Es el "puerta de entrada" de la API
│
├── 📂 Servicios/
│   ├── CarroServicio.cs
│   │   → La LÓGICA DEL NEGOCIO
│   │   → Qué hacer cuando alguien pide los carros disponibles
│   │   → Cómo buscar, filtrar y procesar datos
│   │
│   └── EventoCarroListener.cs
│       → Escucha cuando se crea un carro
│       → Reacciona imprimiendo un mensaje en consola
│
├── 📂 Repositorio/
│   └── CarroCsvRepository.cs
│       → Lee y escribe datos en el archivo CSV
│       → Es el "intermediario" entre la app y los datos
│       → Implementa el PATRÓN REPOSITORY
│
├── 📂 Dominio/
│   ├── Carro.cs
│   │   → La clase principal que representa un carro
│   │   → Tiene propiedades: Id, Marca, Modelo, Precio, etc.
│   │
│   └── CarroDtos.cs
│       → CrearCarroDto: datos para crear un carro
│       → ActualizarCarroDto: datos para actualizar un carro
│       → DTOs = "objetos de transferencia de datos"
│
├── 📂 Middlewares/
│   └── LoggingMiddleware.cs
│       → Intercepta TODAS las peticiones HTTP
│       → Registra qué petición llegó y cuánto tardó
│       → Aparece en la consola con [AOP]
│
├── 📂 Aspectos/
│   └── LoggingInterceptor.cs
│       → Intercepta CADA MÉTODO del servicio
│       → Dice qué método se ejecutó y cuándo terminó
│       → Usa Castle DynamicProxy (librería AOP)
│
├── Program.cs
│    → Configuración PRINCIPAL de la aplicación
│    → Registra todos los servicios
│    → Define el pipeline HTTP
│
├── .gitignore
│    → Archivos que NO se suben a GitHub
│
├── carros.csv (se crea automáticamente)
│    → Base de datos con los carros guardados
│    → Es un archivo de texto separado por comas
│
└── README.md
     → Este archivo
```

## 🔧 Conceptos principales - Aprendizaje

### 1. **Patrón Repository** 📚
**¿Qué es?** Es un "intermediario" entre tu aplicación y los datos.

**¿Para qué sirve?**
- Toda lectura/escritura de datos pasa por aquí
- Si quieres cambiar de CSV a SQL, solo modificas esta clase
- El resto del código NO cambia

**Ejemplo en el código:**
```csharp
// El servicio pide datos al repositorio, NO accede directo al CSV
var carros = _repositorio.ObtenerTodos();
```

**Ventaja:** Desacoplamiento. El servicio no sabe si usa CSV, SQL o una API.

---

### 2. **DTOs (Data Transfer Objects)** 📦
**¿Qué son?** Clases especiales para recibir/enviar datos del cliente.

**¿Por qué no usar directamente la clase `Carro`?**
- `CrearCarroDto` NO tiene `Id` porque el servidor lo asigna
- `CrearCarroDto` NO tiene `Estado` porque empieza como "Disponible"
- Esto **protege** la lógica de negocio

**Ejemplo:**
```csharp
public class CrearCarroDto
{
    public string Marca { get; set; }     // El cliente envía esto
    public string Modelo { get; set; }
    // NO tiene Id - el servidor lo genera
    // NO tiene Estado - el servidor lo asigna
}
```

---

### 3. **Inyección de Dependencias (DI)** 🔌
**¿Qué es?** La aplicación te proporciona lo que necesitas automáticamente.

**Sin DI (mala práctica):**
```csharp
public class CarroServicio
{
    private ICarroRepository _repositorio = new CarroCsvRepository();  // ❌ Acoplado
}
```

**Con DI (buena práctica):**
```csharp
public class CarroServicio
{
    private readonly ICarroRepository _repositorio;

    public CarroServicio(ICarroRepository repositorio)  // ✅ Se recibe
    {
        _repositorio = repositorio;
    }
}
```

En `Program.cs`, ASP.NET Core inyecta automáticamente:
```csharp
builder.Services.AddScoped<ICarroServicio>(provider =>
{
    var repositorio = provider.GetRequiredService<ICarroRepository>();
    // ... aquí crea e inyecta todo
});
```

---

### 4. **Eventos en C#** 🔔
**¿Qué son?** Un sistema para que una acción dispare otras acciones automáticamente.

**En el código:**
```csharp
// En CarroServicio.cs
public event EventHandler<Carro>? CarroCreado;  // Declara el evento

public Carro Crear(CrearCarroDto dto)
{
    // ... crea el carro ...
    CarroCreado?.Invoke(this, creado);  // DISPARA el evento
}
```

**¿Quién escucha?**
```csharp
// En EventoCarroListener.cs
public EventoCarroListener(ICarroServicio servicio)
{
    if (servicio is CarroServicio carroServicio)
    {
        carroServicio.CarroCreado += AlCarroCreado;  // SE SUSCRIBE
    }
}

private void AlCarroCreado(object? sender, Carro carro)
{
    Console.WriteLine($"[EVENTO] Carro creado: {carro.Marca}");
}
```

**¿Por qué?** El servicio NO sabe quién lo escucha. Otros componentes reaccionan automáticamente.

---

### 5. **AOP (Aspectos Orientados a Objetos)** 🎯
**¿Qué es?** Agregar funcionalidad (como logging) sin modificar el código original.

**En el código, usamos Castle DynamicProxy:**
```csharp
// LoggingInterceptor.cs
public void Intercept(IInvocation invocation)
{
    Console.WriteLine($"[AOP] Ejecutando: {invocation.Method.Name}");
    invocation.Proceed();  // Ejecuta el método real
    Console.WriteLine($"[AOP] Terminado: {invocation.Method.Name}");
}
```

**¿Ventaja?** Registras qué método se ejecuta sin tocar el código del método.

---

### 6. **Middleware** 🚦
**¿Qué es?** Intercepta las peticiones HTTP ANTES de llegar al controlador.

**En el código:**
```csharp
// LoggingMiddleware.cs
public async Task InvokeAsync(HttpContext context)
{
    Console.WriteLine($"[AOP] ► {metodo} {ruta}");  // ANTES
    await _next(context);  // Ejecuta el controlador
    Console.WriteLine($"[AOP] ✔ {metodo} {ruta}");  // DESPUÉS
}
```

**¿Diferencia con AOP?**
- **Middleware** = Intercepta peticiones HTTP (más alto nivel)
- **AOP** = Intercepta métodos individuales (más bajo nivel)

---

### 7. **LINQ** ✨
**¿Qué es?** Un lenguaje de consulta integrado en C# para filtrar/transformar datos.

**En el código:**
```csharp
// Obtener solo carros disponibles, ordenados por precio
public IEnumerable<Carro> ObtenerDisponibles() =>
    _repositorio.ObtenerTodos()
                .Where(c => c.Estado == "Disponible")  // FILTRO
                .OrderBy(c => c.Precio);               // ORDEN
```

**Sin LINQ (mucho código):**
```csharp
var disponibles = new List<Carro>();
foreach (var carro in _repositorio.ObtenerTodos())
{
    if (carro.Estado == "Disponible")
        disponibles.Add(carro);
}
disponibles = disponibles.OrderBy(c => c.Precio).ToList();
```

## 📝 Ejemplos prácticos - Cómo usar cada endpoint

### GET - Obtener todos los carros
```
GET /api/carros

Respuesta:
[
  {
    "id": 1,
    "marca": "Toyota",
    "modelo": "Corolla",
    "anio": 2024,
    "precio": 20000,
    "kilometraje": 0,
    "color": "Blanco",
    "transmision": "Automático",
    "estado": "Disponible"
  }
]
```

### POST - Crear un nuevo carro
```
POST /api/carros

ENVIAMOS (body):
{
  "marca": "Honda",
  "modelo": "Civic",
  "anio": 2023,
  "precio": 22000,
  "kilometraje": 5000,
  "color": "Negro",
  "transmision": "Manual",
  "estado": "Disponible"
}

RESPUESTA:
{
  "id": 2,
  "marca": "Honda",
  "modelo": "Civic",
  ...
}

EN LA CONSOLA VES:
[AOP] ► POST /api/carros — 14:35:20
[AOP] Ejecutando: Crear
[EVENTO] ✨ Carro creado exitosamente: Honda Civic (Id: 2)
[AOP] Terminado: Crear
[AOP] ✔ POST /api/carros — Status: 201 — 45ms
```

### GET - Buscar por marca
```
GET /api/carros/buscar?marca=Honda

Respuesta:
[
  {
    "id": 2,
    "marca": "Honda",
    "modelo": "Civic",
    ...
  }
]
```

### GET - Filtrar por precio máximo
```
GET /api/carros/precio?max=21000

Respuesta (solo carros ≤ 21000):
[
  {
    "id": 1,
    "marca": "Toyota",
    "precio": 20000,
    ...
  }
]
```

### GET - Solo carros disponibles
```
GET /api/carros/disponibles

Respuesta:
[
  {
    "id": 1,
    "estado": "Disponible",
    ...
  }
]
```

### GET - Un carro específico
```
GET /api/carros/1

Respuesta:
{
  "id": 1,
  "marca": "Toyota",
  ...
}

Si no existe:
{
  "error": "No existe un carro con Id 1."
}
(Status: 404)
```

### PUT - Actualizar un carro (actualización parcial)
```
PUT /api/carros/2

ENVIAMOS (solo lo que queremos cambiar):
{
  "precio": 21000,
  "estado": "Vendido"
}

RESPUESTA (con todos los datos, algunos modificados):
{
  "id": 2,
  "marca": "Honda",
  "modelo": "Civic",
  "precio": 21000,  ← CAMBIÓ
  "estado": "Vendido",  ← CAMBIÓ
  "anio": 2023,     ← NO CAMBIÓ
  ...
}
```

### DELETE - Eliminar un carro
```
DELETE /api/carros/2

Respuesta:
(vacía, status 204 No Content)

EN LA CONSOLA:
[AOP] ► DELETE /api/carros/2 — 14:40:15
[AOP] ✔ DELETE /api/carros/2 — Status: 204 — 12ms
```

## 💾 ¿Dónde se guardan los datos? 

### Ubicación del archivo CSV
```
bin/Debug/net8.0/carros.csv
```

Se crea **automáticamente** la primera vez que ejecutas el programa.

### Contenido del archivo (ejemplo)
Si lo abres con Notepad o Excel:
```csv
Id,Marca,Modelo,Anio,Precio,Kilometraje,Color,Transmision,Estado
1,Toyota,Corolla,2024,20000,0,Blanco,Automático,Disponible
2,Honda,Civic,2023,22000,5000,Negro,Manual,Disponible
3,Ford,Focus,2022,19000,15000,Plata,Automático,Vendido
```

### ¿Cómo funciona?
1. **Primera fila** = encabezados (Id, Marca, Modelo...)
2. **Cada fila después** = un carro guardado
3. **Separados por comas** = por eso se llama CSV (Comma-Separated Values)

### Persistencia de datos
- ✅ Cuando cierras el programa, los datos quedan guardados
- ✅ Cuando vuelves a ejecutar, se cargan automáticamente
- ✅ No necesitas base de datos SQL

### Límites
- ⚠️ CSV es lento si hay miles de registros
- ⚠️ No es recomendado para producción
- ✅ Para este proyecto educativo es perfecto

## 🔍 ¿Qué ves en la consola cuando ejecutas?

### Salida normal (cuando creas un carro)
```
[AOP] ► POST /api/carros — 14:35:20
[AOP] Ejecutando: Crear
[AOP] Terminado: Crear
[AOP] ✔ POST /api/carros — Status: 201 — 45ms
[EVENTO] ✨ Carro creado exitosamente: Honda Civic (Id: 2)
```

### Explicación línea por línea:
1. `[AOP] ► POST /api/carros` = **Middleware** dice: "Llegó una petición POST"
2. `[AOP] Ejecutando: Crear` = **Interceptor** dice: "Entré al método Crear"
3. `[AOP] Terminado: Crear` = **Interceptor** dice: "Salí del método Crear"
4. `[AOP] ✔ POST /api/carros — Status: 201 — 45ms` = **Middleware** dice: "Petición terminada en 45 ms, status 201 (Creado)"
5. `[EVENTO] ✨ Carro creado...` = **EventoCarroListener** reacciona al evento

### Cuando buscas carros:
```
[AOP] ► GET /api/carros/disponibles — 14:36:10
[AOP] Ejecutando: ObtenerDisponibles
[AOP] Terminado: ObtenerDisponibles
[AOP] ✔ GET /api/carros/disponibles — Status: 200 — 15ms
```

### Cuando no existe un carro:
```
[AOP] ► GET /api/carros/999 — 14:37:00
[AOP] Ejecutando: ObtenerPorId
[AOP] Terminado: ObtenerPorId
[AOP] ✔ GET /api/carros/999 — Status: 404 — 5ms
```

### Status codes importantes:
- `200` = OK (petición exitosa)
- `201` = Created (recurso creado)
- `204` = No Content (eliminado exitosamente)
- `404` = Not Found (recurso no existe)
- `500` = Internal Server Error (error del servidor)

## 📚 Lo que aprendí haciendo este proyecto

### Conceptos de arquitectura:
- ✅ **Patrón Repository** - Separar la lógica de datos del resto
- ✅ **Inyección de Dependencias** - No crear manualmente lo que necesitas
- ✅ **Principios SOLID**:
  - **S**ingle Responsibility = Cada clase una responsabilidad
  - **Open/Closed = Abierto a extensión, cerrado a modificación
  - **Liskov Substitution = Las interfaces se pueden intercambiar
  - **Interface Segregation = Interfaces pequeñas y específicas
  - **Dependency Inversion = Depender de abstracciones, no de implementaciones

### Patrones de diseño:
- ✅ **DTOs** - Objetos para transferir datos entre capas
- ✅ **Repository Pattern** - Intermediario de acceso a datos
- ✅ **Factory Pattern** - Crear objetos dinámicamente (Castle)
- ✅ **Observer Pattern** - Sistema de eventos

### Tecnologías:
- ✅ **ASP.NET Core 8** - Framework para APIs
- ✅ **LINQ** - Consultas de datos funcionales
- ✅ **Castle DynamicProxy** - AOP (Aspectos)
- ✅ **CsvHelper** - Lectura/escritura de CSV
- ✅ **Swagger/OpenAPI** - Documentación automática

### Buenas prácticas:
- ✅ Usar interfaces para desacoplar
- ✅ Logging para debuggear
- ✅ Eventos para comunicación entre componentes
- ✅ Validación de datos


### Herramientas:
- ✅ Visual Studio Community
- ✅ Git y GitHub
- ✅ .NET CLI (dotnet run, dotnet build, etc.)

## 📌 Notas importantes antes de usar

### Comportamiento esperado:
- ✅ El CSV se crea automáticamente en primera ejecución
- ✅ Los **IDs se generan automáticamente** (no puedes ponerlos tú)
- ✅ Los estados válidos son: `Disponible`, `Vendido`, `Reservado`
- ✅ Cuando **actualizas un carro**, solo cambias los campos que envíes
- ✅ Los datos se **persisten** (se guardan entre ejecuciones)

### Estados de un carro:
```
Disponible  → Se puede vender
Vendido     → Ya fue vendido
Reservado   → Cliente lo reservó pero no pagó
```

### Tipos de actualización:
```
ACTUALIZACIÓN TOTAL (mala):
PUT /api/carros/1
{ Envías TODOS los campos }

ACTUALIZACIÓN PARCIAL (buena) ✅:
PUT /api/carros/1
{ "precio": 18000 }  ← Cambias solo lo que necesitas
```

### Transmisiones válidas:
- `Manual`
- `Automático`
- `CVT`
- `Semi-automático`

### Validaciones implícitas:
- ❌ No puedes crear un carro sin marca o modelo
- ❌ El precio no puede ser negativo
- ❌ El año tiene que ser número
- ✅ Si envías datos inválidos, Swagger te lo dice

