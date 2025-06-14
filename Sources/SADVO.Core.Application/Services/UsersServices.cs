using AutoMapper;
using SADVO.Core.Application.Dtos.Usuarios;
using SADVO.Core.Application.Helpers;
using SADVO.Core.Application.Interfaces;
using SADVO.Core.Domain.Entities;
using SADVO.Core.Domain.Interfaces;

namespace SADVO.Core.Application.Services
{
	public class UsersServices : GenericService<UsuarioDTO, Usuarios>, IUserServices
	{
		private readonly IUserRepository _userRepository;

		public UsersServices(IUserRepository userRepository, IMapper mapper)
			: base(userRepository, mapper)
		{
			_userRepository = userRepository;
		}
		public override async Task<bool> AddAsync(UsuarioDTO dto)
		{
			if (dto == null) throw new ArgumentNullException(nameof(dto));

			if (string.IsNullOrWhiteSpace(dto.Email))
				throw new Exception("El email es obligatorio.");

			dto.Contraseña = PasswordEncryption.ComputeSha256Hash(dto.Contraseña);

			return await base.AddAsync(dto);
		}
		public override async Task<bool> UpdateAsync(int id, UsuarioDTO dto)
		{
			if (dto == null) throw new ArgumentNullException(nameof(dto));

			if (!string.IsNullOrWhiteSpace(dto.Contraseña))
				dto.Contraseña = PasswordEncryption.ComputeSha256Hash(dto.Contraseña);
			else
				dto.Contraseña = null!; // Para no modificar la contraseña si no viene en dto

			return await base.UpdateAsync(id, dto);
		}

		// Método para crear usuario admin si no existe ninguno
		public async Task AddAdminUser()
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
					Contraseña = "10062802@",
					NombreUsuario = "SuperAdmin"
				};
				await CreateAdminUser(newUser);
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

		public async Task<UsuarioDTO> LoginAsync(LoginDto loginDto)
		{
			var user = await _userRepository.LoginAsync(loginDto.NombreUsuario, loginDto.Contraseña);
			return _mapper.Map<UsuarioDTO>(user);
		}
	}
}
