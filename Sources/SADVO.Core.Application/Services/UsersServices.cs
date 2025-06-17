using AutoMapper;
using SADVO.Core.Application.Dtos.Usuarios;
using SADVO.Core.Application.Helpers;
using SADVO.Core.Application.Interfaces;
using SADVO.Core.Domain.Entities;
using SADVO.Core.Domain.Interfaces;
using System.Diagnostics;

namespace SADVO.Core.Application.Services
{
	public class UsersServices : GenericService<CrearUsuarioDTO, UpdateUsuarioDTO, UsuarioDTO, Usuarios>, IUserServices
	{
		private readonly IUserRepository _userRepository;

		public UsersServices(IUserRepository userRepository, IMapper mapper)
			: base(userRepository, mapper)
		{
			_userRepository = userRepository;
		}

		public override async Task<bool> AddAsync(CrearUsuarioDTO dto)
		{
			if (dto == null) throw new ArgumentNullException(nameof(dto));

			if (string.IsNullOrWhiteSpace(dto.Email))
				throw new Exception("El email es obligatorio.");

			var getAllUsers = await _userRepository.GetAllList();
			bool userExist = getAllUsers.Any(u => u.NombreUsuario == dto.NombreUsuario || u.Email == dto.Email);

			if (userExist) throw new InvalidOperationException("Este usuario o correo ya está siendo utilizado.");

			dto.Contraseña = PasswordEncryption.ComputeSha256Hash(dto.Contraseña);
			return await base.AddAsync(dto);
		}
		public override async Task<bool> UpdateAsync(int id, UpdateUsuarioDTO dto) 
		{
			var currentUser = await _userRepository.GetById(id);
			if (currentUser == null) return false;

			if (!string.IsNullOrWhiteSpace(dto.Contraseña))
			{
				dto.Contraseña = PasswordEncryption.ComputeSha256Hash(dto.Contraseña);
			}


			return await base.UpdateAsync(id, dto);
		}

		public async Task AddAdminUser()
		{
			var getUserList = await _userRepository.GetAllList();
			if (getUserList.Count == 0)
			{
				await CreateAdminUser(new CrearUsuarioDTO
				{
					Id = 0,
					Nombre = "Admin",
					Apellido = "",
					Email = "admin@gmail.com",
					Contraseña = "10062802@",
					NombreUsuario = "SuperAdmin"
				});
			}
		}

		public async Task<bool> CreateAdminUser(CrearUsuarioDTO crearUsuarioDTO)
		{
			var usuario = _mapper.Map<Usuarios>(crearUsuarioDTO);
			usuario.Contraseña = PasswordEncryption.ComputeSha256Hash(crearUsuarioDTO.Contraseña);
			usuario.Rol = "Administrador";

			await _userRepository.AddAsync(usuario);
			return true;
		}

		public async Task<List<UsuarioDTO>> GetActiveUsersAsync()
		{
			var allUsers = await _userRepository.GetAllList();

			var activeUsers = allUsers.Where(u => u.Estado && u.Rol == "Dirigente").ToList();

			return _mapper.Map<List<UsuarioDTO>>(activeUsers);
		}


		public async Task<UsuarioDTO> LoginAsync(LoginDto loginDto)
		{
			var user = await _userRepository.LoginAsync(loginDto.NombreUsuario, loginDto.Contraseña);

			if (user == null) return null!;

			if (!user.Estado)
				throw new Exception("Usuario inactivo");

			return _mapper.Map<UsuarioDTO>(user);
		}


	}
}