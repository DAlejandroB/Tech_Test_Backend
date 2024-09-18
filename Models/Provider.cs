namespace Tech_Test_Backend.Models
{
    public class Provider
    {
        public Guid Id { get; set; }
        public string? TaxId { get; set; } // NIT
        public string? BusinessName { get; set; } // Razón social
        public string? Address { get; set; } // Dirección
        public string? City { get; set; } // Ciudad
        public string? Department { get; set; } // Departamento
        public string? Email { get; set; } // Correo
        public bool IsActive { get; set; } // Activo
        public DateTime CreatedAt { get; set; } // Fecha de creación
        public string? ContactName { get; set; } // Nombre de contacto
        public string? ContactEmail { get; set; } // Correo de contacto
    }
}
