using System.Collections.Generic;

#nullable disable

namespace TestAoniken.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            Publicaciones = new HashSet<Publicacion>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Rol { get; set; }
        public string Email { get; set; }

        public virtual ICollection<Publicacion> Publicaciones { get; set; }
    }
}
