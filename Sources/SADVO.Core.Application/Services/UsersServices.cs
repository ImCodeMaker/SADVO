using AutoMapper;
using SADVO.Core.Application.Dtos;
using SADVO.Core.Application.Helpers;
using SADVO.Core.Application.Interfaces;
using SADVO.Core.Domain.Entities;
using SADVO.Core.Domain.Interfaces;

namespace SADVO.Core.Application.Services
{
	public class UsersServices : IUserServices
	{
		private readonly IUserRepository _userRepository;
		private readonly IMapper _mapper;

		public UsersServices(IUserRepository userRepository, IMapper mapper)
		{
			_userRepository = userRepository;
			_mapper = mapper;
		}

		public async Task addAdminUser()
		{
			try
			{
				var getUserList = await _userRepository.GetAllList();
				if (getUserList.Count == 0)
				{
					CrearUsuarioDTO newUser = new()
					{
						Id = 0,
						Nombre = "Admin",
						Apellido = "",
						Email = "admin@gmail.com",
						Contraseña = "10062802@", // Nota: Considera cambiarla a una más segura
						NombreUsuario = "SuperAdmin"
					};

					await CreateAdminUser(newUser);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error in addAdminUser: {ex.Message}");
				throw;
			}
		}
		public async Task<bool> CreateAdminUser(CrearUsuarioDTO crearUsuarioDTO)
		{
			if (crearUsuarioDTO == null)
				throw new ArgumentNullException(nameof(crearUsuarioDTO));

			try
			{
				var mapUsertoDto = _mapper.Map<Usuarios>(crearUsuarioDTO);
				mapUsertoDto.Contraseña = PasswordEncryption.ComputeSha256Hash(crearUsuarioDTO.Contraseña);
				mapUsertoDto.Rol = "Administrador";

				await _userRepository.AddAsync(mapUsertoDto);
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error completo: {ex}");
				throw;
			}
		}

		public async Task<bool> AddAsync(CrearUsuarioDTO crearUsuarioDTO)
		{
			if (crearUsuarioDTO == null)
				throw new ArgumentNullException(nameof(crearUsuarioDTO));

			try
			{
				var mapUsertoDto = _mapper.Map<Usuarios>(crearUsuarioDTO);
				mapUsertoDto.Contraseña = PasswordEncryption.ComputeSha256Hash(crearUsuarioDTO.Contraseña);
				mapUsertoDto.Rol = "Dirigente";

				await _userRepository.AddAsync(mapUsertoDto);
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error en AddAsync: {ex.Message}");
				throw;
			}
		}

		public async Task<bool> UpdateUserAsync(int Id, UsuarioDTO usuarioDTO)
		{
			if (usuarioDTO == null)
				throw new ArgumentNullException(nameof(usuarioDTO));

			try
			{
				if (!string.IsNullOrWhiteSpace(usuarioDTO.Contraseña))
				{
					usuarioDTO.Contraseña = PasswordEncryption.ComputeSha256Hash(usuarioDTO.Contraseña);
				}
				else
				{
					usuarioDTO.Contraseña = null!;
				}

				var MapEntity = _mapper.Map<Usuarios>(usuarioDTO);

				await _userRepository.UpdateAsync(Id, MapEntity);

				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error en UpdateUserAsync: {ex.Message}");
				throw;
			}
		}

		public async Task<bool> DeleteUserAsync(int Id)
		{
			if (Id < 0)
				throw new InvalidOperationException("El campo Id no puede ser menor a 0");

			try
			{
				await _userRepository.DeleteAsync<Usuarios>(Id);
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error en DeleteUserAsync: {ex.Message}");
				throw;
			}
		}

		public async Task<List<UsuarioDTO>> GetAllUsers()
		{
			var userList = await _userRepository.GetAllList();
			return _mapper.Map<List<UsuarioDTO>>(userList);
		}

		public async Task<UsuarioDTO> LoginAsync(LoginDto loginDto)
		{
			if (loginDto == null)
				throw new ArgumentNullException(nameof(loginDto));

			var user = await _userRepository.LoginAsync(loginDto.Email, loginDto.Contraseña);

			if (user == null)
				return null!;

			var mappedUser = _mapper.Map<UsuarioDTO>(user);
			return mappedUser;
		}
	}
}
