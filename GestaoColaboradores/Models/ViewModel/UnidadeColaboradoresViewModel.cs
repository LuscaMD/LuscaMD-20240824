using GestaoColaboradores.Models.Entities;

namespace GestaoColaboradores.Models.ViewModel
{
    public class UnidadeColaboradoresViewModel
    {
        public Unidade Unidade { get; set; }

        public List<Colaborador> Colaboradores { get; set; }
    }
}
