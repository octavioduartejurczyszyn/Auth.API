using Auth.API.Data;
using Auth.API.Models;
using Auth.API.Models.DTOs;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;

namespace Auth.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;

        private readonly JwtService _jwtService;

        public AuthService(AppDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<UserDto> RegisterAsync(RegisterDto model)
        {
            // Verificar que el email no exista
            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
                throw new Exception("El email ya está registrado");

            var user = new User
            {
                Nombre = model.Nombre,
                Email = model.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                RoleId = model.RoleId
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Map a UserDto
            return new UserDto
            {
                Id = user.Id,
                Nombre = user.Nombre,
                Email = user.Email,
                Role = (await _context.Roles.FindAsync(user.RoleId))!.Nombre
            };
        }

        public async Task<string> LoginAsync(LoginDto model)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user == null)
                throw new Exception("Credenciales incorrectas");

            if (!BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
                throw new Exception("Credenciales incorrectas");

            // Generar el JWT
            return _jwtService.GenerateToken(user);
        }
    }
}

