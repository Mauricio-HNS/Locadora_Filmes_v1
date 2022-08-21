using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Locadora_De_Filmes
{
    public class Locacao
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public int FilmeId { get; set; }
        public DateTime Data_locacao { get; set; }
        public DateTime Data_Entrega { get; set; }
        public DateTime Data_Devolvido { get; set; }
    }
}
