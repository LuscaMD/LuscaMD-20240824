using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoColaboradores.Models.Entities
{
    public class Colaborador
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? IdColaborador { get; set; }

        public string Nome { get; set; }

        [Display(Name = "Codigo da Unidade")]
        public int CodigoUnidade { get; set; }

        public int IdUsuario { get; set; }

        public Unidade? Unidade { get; set; }

        public Colaborador() { }

        public Colaborador(string nome, int unidade, int usuario)
        {
            Nome = nome;
            CodigoUnidade = unidade;
            IdUsuario = usuario;
        }
    }
}
