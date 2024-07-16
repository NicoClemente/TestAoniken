using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestAoniken.Data;
using TestAoniken.Models;

namespace TestAoniken.Servicios
{
    public class PublicacionService : IPublicacionService
    {
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;

        public PublicacionService(AppDbContext context, IEmailService emailService)
        {
            _context = context;  // DI del DbContext
            _emailService = emailService;  // DI del EmailService
        }

        public async Task<List<Publicacion>> ObtenerPublicacionesPendientesAsync()
        {
            return await _context.Publicaciones
                .Include(p => p.Autor)
                .Where(p => p.PendienteAprobacion)
                .ToListAsync();
        }

        public async Task<bool> AprobarPublicacionAsync(int idPublicacion)
        {
            if (idPublicacion <= 0)
            {
                throw new ArgumentException("El ID de la publicación debe ser un número positivo.");
            }

            var publicacion = await _context.Publicaciones
                .Include(p => p.Autor)
                .FirstOrDefaultAsync(p => p.Id == idPublicacion);

            if (publicacion == null)
            {
                return false;  // Si no se encuentra la publicación, retornar falso
            }

            if (!publicacion.PendienteAprobacion)
            {
                throw new InvalidOperationException("La publicación ya está aprobada.");
            }

            publicacion.PendienteAprobacion = false;  // Cambiar el estado de pendiente a aprobado
            await _context.SaveChangesAsync();  // Guardar los cambios en la base de datos

            // Enviar notificación por correo electrónico
            await _emailService.SendEmailAsync(publicacion.AutorId, "Publicación aprobada",
                $"Tu publicación '{publicacion.Titulo}' ha sido aprobada.");

            return true;  // Retornar verdadero indicando que la operación fue exitosa
        }

        public async Task<bool> RechazarPublicacionAsync(int idPublicacion)
        {
            if (idPublicacion <= 0)
            {
                throw new ArgumentException("El ID de la publicación debe ser un número positivo.");
            }

            var publicacion = await _context.Publicaciones.FindAsync(idPublicacion);  // Buscar la publicación por su ID
            if (publicacion == null)
            {
                return false;  // Si no se encuentra la publicación, retornar falso
            }

            _context.Publicaciones.Remove(publicacion);  // Eliminar la publicación de la base de datos
            await _context.SaveChangesAsync();  // Guardar los cambios en la base de datos
            return true;  // Retornar verdadero indicando que la operación fue exitosa
        }

        public async Task<bool> ActualizarPublicacionAsync(int id, Publicacion publicacionActualizada)
        {
            if (id <= 0)
            {
                throw new ArgumentException("El ID de la publicación debe ser un número positivo.");
            }

            if (publicacionActualizada == null)
            {
                throw new ArgumentNullException(nameof(publicacionActualizada), "La publicación actualizada no puede ser nula.");
            }

            if (string.IsNullOrWhiteSpace(publicacionActualizada.Titulo))
            {
                throw new ArgumentException("El título de la publicación no puede estar vacío.");
            }

            if (string.IsNullOrWhiteSpace(publicacionActualizada.Contenido))
            {
                throw new ArgumentException("El contenido de la publicación no puede estar vacío.");
            }

            var publicacion = await _context.Publicaciones.FindAsync(id);  // Buscar la publicación por su ID
            if (publicacion == null)
            {
                return false;  // Si no se encuentra la publicación, retornar falso
            }

            publicacion.Titulo = publicacionActualizada.Titulo;  // Actualizar el título de la publicación
            publicacion.Contenido = publicacionActualizada.Contenido;  // Actualizar el contenido de la publicación
            await _context.SaveChangesAsync();  // Guardar los cambios en la base de datos
            return true;  // Retornar verdadero indicando que la operación fue exitosa
        }

        public async Task<bool> EliminarPublicacionAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("El ID de la publicación debe ser un número positivo.");
            }

            var publicacion = await _context.Publicaciones.FindAsync(id);  // Buscar la publicación por su ID
            if (publicacion == null)
            {
                return false;  // Si no se encuentra la publicación, retornar falso
            }

            _context.Publicaciones.Remove(publicacion);  // Eliminar la publicación de la base de datos
            await _context.SaveChangesAsync();  // Guardar los cambios en la base de datos
            return true;  // Retornar verdadero indicando que la operación fue exitosa
        }
    }
}