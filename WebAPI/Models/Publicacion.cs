using System;

#nullable disable

namespace TestAoniken.Models
{
    public partial class Publicacion
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Contenido { get; set; }
        public int AutorId { get; set; }
        public DateTime FechaEnvio { get; set; }
        public bool PendienteAprobacion { get; set; }

        public virtual Usuario Autor { get; set; }
    }
}
