using AutoMapper;
using SADVO.Core.Application.Dtos.Votacion;
using SADVO.Core.Application.Interfaces;
using SADVO.Core.Domain.Entities;
using SADVO.Core.Domain.Interfaces;

namespace SADVO.Core.Application.Services
{
	public class VotacionService : IVotacionService
	{
		private readonly IVotacionRepository _votacionRepository;
		private readonly IEmailServices _emailServices;
		private readonly ICiudadanoSession _ciudadanoSession;
		private readonly IMapper _mapper;
		private readonly IVotacionHelperServices _votacionHelper;

		public VotacionService(IVotacionRepository votacionRepository, IMapper mapper, IEmailServices emailServices, IVotacionHelperServices votacionHelper, ICiudadanoSession ciudadanoSession)
		{
			_votacionRepository = votacionRepository;
			_mapper = mapper;
			_emailServices = emailServices;
			_votacionHelper = votacionHelper;
			_ciudadanoSession = ciudadanoSession;
		}

		// Método privado para validar si hay un ciudadano en sesión
		private bool ValidarCiudadanoEnSesion()
		{
			var ciudadano = _ciudadanoSession.GetCiudadanoSession();
			return ciudadano != null && ciudadano.Id > 0;
		}

		public async Task<EleccionVotacionDTO?> GetEleccionParaVotarAsync(int ciudadanoId)
		{
			if (!ValidarCiudadanoEnSesion())
			{
				return null;
			}

			var ciudadanoSesion = _ciudadanoSession.GetCiudadanoSession();
			if (ciudadanoSesion!.Id != ciudadanoId)
			{
				return null;
			}

			var eleccion = await _votacionRepository.GetEleccionActivaAsync();
			if (eleccion == null)
				return null;

			var candidatos = await _votacionRepository.GetCandidatosParaVotacionAsync(eleccion.Id);

			var puestosVotados = await _votacionRepository.GetPuestosVotadosPorCiudadanoAsync(ciudadanoId, eleccion.Id);

			var candidatosPorPuesto = new Dictionary<int, List<CandidatoVotacionDTO>>();

			foreach (var asignacion in candidatos)
			{
				var candidatoDto = new CandidatoVotacionDTO
				{
					CandidatoId = asignacion.CandidatoId,
					NombreCandidato = asignacion.Candidato.Nombre,
					ApellidoCandidato = asignacion.Candidato.Apellido,
					FotoCandidato = asignacion.Candidato.Foto,
					PuestoElectivoId = asignacion.PuestoElectivoId,
					NombrePuestoElectivo = asignacion.puestosElectivos.Nombre,
					PartidoPrincipalId = asignacion.PartidoPoliticoId,
					NombrePartidoPrincipal = asignacion.PartidosPoliticos.Nombre,
					SiglasPartidoPrincipal = asignacion.PartidosPoliticos.Siglas,
				};

				var respaldos = await _votacionRepository.GetRespaldosParaCandidatoAsync(
					asignacion.CandidatoId, asignacion.PuestoElectivoId);

				candidatoDto.PartidosRespaldo = respaldos.Select(r => new PartidoRespaldoDTO
				{
					PartidoId = r.PartidoRespaldaId!.Value,
					Nombre = r.PartidoQueRespalda!.Nombre,
					Siglas = r.PartidoQueRespalda.Siglas,
				}).ToList();

				if (!candidatosPorPuesto.ContainsKey(asignacion.PuestoElectivoId))
				{
					candidatosPorPuesto[asignacion.PuestoElectivoId] = new List<CandidatoVotacionDTO>();
				}

				candidatosPorPuesto[asignacion.PuestoElectivoId].Add(candidatoDto);
			}

			var puestosElectivos = candidatosPorPuesto.Select(kvp => new PuestoElectivoVotacionDTO
			{
				PuestoElectivoId = kvp.Key,
				NombrePuesto = kvp.Value.First().NombrePuestoElectivo,
				Candidatos = kvp.Value,
				YaVotado = puestosVotados.Contains(kvp.Key)
			}).OrderBy(p => p.NombrePuesto).ToList();

			var eleccionDto = new EleccionVotacionDTO
			{
				EleccionId = eleccion.Id,
				NombreEleccion = eleccion.Nombre,
				PuestosElectivos = puestosElectivos,
				YaVotoCompleto = puestosElectivos.Count > 0 && puestosElectivos.All(p => p.YaVotado)
			};

			return eleccionDto;
		}

		public async Task<ResultadoVotoDTO> RegistrarVotoAsync(RegistrarVotoDTO dto)
		{
			var resultado = new ResultadoVotoDTO();

			try
			{
				if (!ValidarCiudadanoEnSesion())
				{
					resultado.Errores.Add("Acceso no autorizado. Debe iniciar sesión como ciudadano para votar.");
					return resultado;
				}

				var ciudadanoSesion = _ciudadanoSession.GetCiudadanoSession();
				if (ciudadanoSesion!.Id != dto.CiudadanoId)
				{
					resultado.Errores.Add("No tiene autorización para realizar esta acción.");
					return resultado;
				}

				if (dto.EleccionId <= 0 || dto.CiudadanoId <= 0 || dto.PuestoElectivoId <= 0 || dto.CandidatoId <= 0)
				{
					resultado.Errores.Add("Los datos del voto no son válidos. Por favor, verifica tu selección.");
					return resultado;
				}

				var eleccion = await _votacionRepository.GetEleccionActivaAsync();
				if (eleccion == null)
				{
					resultado.Errores.Add("No hay ninguna elección activa en este momento. Inténtalo más tarde.");
					return resultado;
				}

				if (eleccion.Id != dto.EleccionId)
				{
					resultado.Errores.Add("La elección especificada no es válida. Por favor, actualiza la página e intenta nuevamente.");
					return resultado;
				}

				if (await _votacionRepository.YaVotoEnPuestoAsync(dto.CiudadanoId, dto.EleccionId, dto.PuestoElectivoId))
				{
					resultado.Errores.Add("Ya has emitido tu voto para este puesto electivo. Tu participación ya fue registrada.");
					return resultado;
				}

				var voto = new Votos
				{
					Id = 0,
					EleccionId = dto.EleccionId,
					CiudadanoId = dto.CiudadanoId,
					PuestoElectivoId = dto.PuestoElectivoId,
					CandidatoId = dto.CandidatoId,
					PartidoPoliticoId = dto.PartidoPoliticoId,
					FechaVoto = DateTime.Now
				};

				var votoRegistrado = await _votacionRepository.RegistrarVotoAsync(voto);
				if (!votoRegistrado)
				{
					resultado.Errores.Add("Hubo un problema al registrar tu voto. Por favor, intenta nuevamente.");
					return resultado;
				}

				var eleccionCompleta = await GetEleccionParaVotarAsync(dto.CiudadanoId);
				bool votoCompleto = eleccionCompleta?.YaVotoCompleto ?? false;

				if (votoCompleto)
				{
					var historial = new HistorialVotaciones
					{
						Id = 0,
						EleccionId = dto.EleccionId,
						CiudadanoId = dto.CiudadanoId,
						HaVotado = true,
						FechaVotacion = DateTime.Now,
						EmailEnviado = false
					};

					var historialRegistrado = await _votacionRepository.RegistrarHistorialVotacionAsync(historial);
				}

				resultado.Exitoso = true;
				if (votoCompleto)
				{
					resultado.Mensaje = "¡Felicitaciones! Has completado toda tu votación exitosamente. Tu voz cuenta y hace la diferencia.";

					try
					{
						var ciudadano = await _votacionHelper.GetCiudadanosById(dto.CiudadanoId);
						if (ciudadano != null && !string.IsNullOrEmpty(ciudadano.Email))
						{
							await SendVotingConfirmationEmailAsync(ciudadano.Email, ciudadano.NombreCompleto, eleccion.Nombre);
						}
					}
					catch (Exception emailEx)
					{
						Console.WriteLine($"Error enviando email de confirmación: {emailEx.Message}");
					}
				}
				else
				{
					var puestosRestantes = eleccionCompleta?.PuestosElectivos?.Count(p => !p.YaVotado) ?? 0;
					resultado.Mensaje = $"Perfecto! Tu voto ha sido registrado exitosamente. Aún puedes votar en {puestosRestantes} puesto(s) más para completar tu participación.";
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error en RegistrarVotoAsync: {ex}");
				resultado.Errores.Add($"Error interno del sistema: {ex.Message}. Nuestro equipo ha sido notificado.");
			}

			return resultado;
		}

		public async Task<bool> PuedeVotarAsync(int ciudadanoId)
		{
			if (!ValidarCiudadanoEnSesion())
			{
				return false;
			}

			var ciudadanoSesion = _ciudadanoSession.GetCiudadanoSession();
			if (ciudadanoSesion!.Id != ciudadanoId)
			{
				return false;
			}

			var eleccion = await _votacionRepository.GetEleccionActivaAsync();
			if (eleccion == null)
				return false;

			var eleccionCompleta = await GetEleccionParaVotarAsync(ciudadanoId);
			return !(eleccionCompleta?.YaVotoCompleto ?? false);
		}

		private async Task SendVotingConfirmationEmailAsync(string ciudadanoMail, string ciudadanoName, string eleccionName)
		{
			try
			{
				string subject = "🎉 ¡Confirmación de Votación Exitosa! - " + eleccionName;
				string message = $"¡Hola {ciudadanoName}! 👋\n\n" +
								$"🎊 ¡Felicitaciones! Has completado exitosamente tu participación en: {eleccionName}\n\n" +
								$"✨ Tu voto ha sido registrado de forma segura y confidencial.\n" +
								$"🗳️ Gracias por ejercer tu derecho democrático y hacer que tu voz sea escuchada.\n\n" +
								$"🏛️ Cada voto cuenta para construir un mejor futuro.\n\n" +
								$"📧 Este es un mensaje automático del Sistema de Votación Electrónica\n" +
								$"🔐 Tu participación es completamente confidencial y segura.";

				await _emailServices.SendEmailAsync(ciudadanoMail, subject, message);
				Console.WriteLine($"Confirmation email sent successfully to {ciudadanoName}");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error sending confirmation email: {ex.Message}");
			}
		}
	}
}