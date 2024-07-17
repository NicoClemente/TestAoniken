using System.Collections.Generic;
using System.Threading.Tasks;
using TestAoniken.Models;

namespace TestAoniken.Servicios
{
    public interface IPublicacionService
    {
        Task<List<Publicacion>> ObtenerPublicacionesPendientesAsync();
        Task<OperationResult> AprobarPublicacionAsync(int idPublicacion);
        Task<bool> RechazarPublicacionAsync(int idPublicacion);
        Task<bool> ActualizarPublicacionAsync(int id, Publicacion publicacionActualizada);
        Task<bool> EliminarPublicacionAsync(int id);
    }
}
