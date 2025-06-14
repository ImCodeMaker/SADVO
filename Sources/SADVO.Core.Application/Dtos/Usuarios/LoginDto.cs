namespace SADVO.Core.Application.Dtos.Usuarios
{
    public class LoginDto
    {
        public required string NombreUsuario { get; set; }
        public required string Contraseña { get; set; }
    }
}