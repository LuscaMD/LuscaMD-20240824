using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoColaboradores.Models.Entities
{
    public class Unidade
    {
        [Key]
        public int IdUnidade { get; set; }

        [Display(Name = "Codigo da Unidade")]
        public int CodigoUnidade { get; set; }

        [Display(Name = "Nome da Unidade")]
        public string NomeUnidade { get; set; }

        [Display(Name = "Ativo")]
        public bool UnidadeAtiva { get; set; }

        public ICollection<Colaborador> Colaboradores { get; set; } = new List<Colaborador>();


        public Unidade() { }

        public Unidade(int idUnidade, int codigoUnidade, string nomeUnidade, bool unidadeAtiva)
        {
            IdUnidade = idUnidade;
            CodigoUnidade = codigoUnidade;
            NomeUnidade = nomeUnidade;
            UnidadeAtiva = unidadeAtiva;
        }
    }
}
