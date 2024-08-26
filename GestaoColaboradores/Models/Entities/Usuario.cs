using System.ComponentModel.DataAnnotations;

namespace GestaoColaboradores.Models.Entities
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }

        [Display(Name = "Nome Login")]
        public string NomeLogin { get; set; }

        public string Senha { get; set; }

        [Display(Name = "Ativo")]
        public bool UsuarioAtivo { get; set; }

        public Usuario() { }

        public Usuario(int idUsuario, string nomeLogin, string senha, bool usuarioAtivo)
        {
            IdUsuario = idUsuario;
            NomeLogin = nomeLogin;
            Senha = senha;
            UsuarioAtivo = usuarioAtivo;
        }
    }
}
