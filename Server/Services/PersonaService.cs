using SMI.Shared.DTOs;
using SMI.Shared.Interfaces;
using SMI.Shared.Models;

namespace SMI.Server.Services
{
    public class PersonaService
    {
        private readonly IPersona _personaRepository;

        public PersonaService(IPersona personaRepository)
        {
            _personaRepository = personaRepository;
        }

        public async Task<PersonaDto> CrearPersonaConDireccion(PersonaDto personaDto)
        {
            // Mapear DTO a entidad Persona
            var persona = new Persona
            {
                nombre = personaDto.Nombre,
                apellido = personaDto.Apellido,
                id_Genero = personaDto.Id_Genero,
                FechaNacimiento = personaDto.FechaNacimiento,
                Correo = personaDto.Correo,
                Direccion = new PersonaDireccion
                {
                    callePrincipal = personaDto.Direccion.CallePrincipal,
                    calleSecundaria1 = personaDto.Direccion.CalleSecundaria1,
                    calleSecundaria2 = personaDto.Direccion.CalleSecundaria2,
                    numeroCasa = personaDto.Direccion.NumeroCasa,
                    referencia = personaDto.Direccion.Referencia
                }
            };

            var personaId = await _personaRepository.CrearPersonaAsync(persona);

            // Obtener la persona recién creada con todos sus datos
            var personaCreada = await _personaRepository.ObtenerPersonaConDireccionAsync(personaId);

            // Mapear de vuelta a DTO
            return MapToDto(personaCreada);
        }

        public async Task<PersonaDto> ActualizarPersonaConDireccion(PersonaDto personaDto)
        {
            // Obtener persona existente
            var persona = await _personaRepository.ObtenerPersonaConDireccionAsync(personaDto.Id);

            // Actualizar datos básicos
            persona.nombre = personaDto.Nombre;
            persona.apellido = personaDto.Apellido;
            persona.id_Genero = personaDto.Id_Genero;
            persona.FechaNacimiento = personaDto.FechaNacimiento;
            persona.Correo = personaDto.Correo;

            // Manejar la dirección
            if (persona.Direccion != null)
            {
                // Actualizar dirección existente
                persona.Direccion.callePrincipal = personaDto.Direccion.CallePrincipal;
                persona.Direccion.calleSecundaria1 = personaDto.Direccion.CalleSecundaria1;
                persona.Direccion.calleSecundaria2 = personaDto.Direccion.CalleSecundaria2;
                persona.Direccion.numeroCasa = personaDto.Direccion.NumeroCasa;
                persona.Direccion.referencia = personaDto.Direccion.Referencia;

                await _personaRepository.ActualizarDireccionPersonaAsync(persona.Direccion);
            }
            else
            {
                // Crear nueva dirección
                var nuevaDireccion = new PersonaDireccion
                {
                    id_Persona = personaDto.Id,
                    callePrincipal = personaDto.Direccion.CallePrincipal,
                    calleSecundaria1 = personaDto.Direccion.CalleSecundaria1,
                    calleSecundaria2 = personaDto.Direccion.CalleSecundaria2,
                    numeroCasa = personaDto.Direccion.NumeroCasa,
                    referencia = personaDto.Direccion.Referencia
                };

                await _personaRepository.GuardarDireccionPersonaAsync(nuevaDireccion);
            }

            // Obtener la persona actualizada
            var personaActualizada = await _personaRepository.ObtenerPersonaConDireccionAsync(personaDto.Id);

            return MapToDto(personaActualizada);
        }

        private PersonaDto MapToDto(Persona persona)
        {
            return new PersonaDto
            {
                Id = persona.id,
                Nombre = persona.nombre,
                Apellido = persona.apellido,
                Id_Genero = persona.id_Genero,
                FechaNacimiento = persona.FechaNacimiento,
                Correo = persona.Correo,
                Direccion = persona.Direccion != null ? new PersonaDireccionDto
                {
                    IdPersona = persona.Direccion.id_Persona,
                    CallePrincipal = persona.Direccion.callePrincipal,
                    CalleSecundaria1 = persona.Direccion.calleSecundaria1,
                    CalleSecundaria2 = persona.Direccion.calleSecundaria2,
                    NumeroCasa = persona.Direccion.numeroCasa,
                    Referencia = persona.Direccion.referencia
                } : new PersonaDireccionDto()
            };
        }
    }
}
