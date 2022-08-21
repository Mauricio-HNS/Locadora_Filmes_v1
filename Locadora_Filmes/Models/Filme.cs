using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Locadora_De_Filmes
{
    public class Filme
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(60, ErrorMessage = "Nome do Filme deve ter entre 3 a 60 caracteres!")]
        [MinLength(3, ErrorMessage =" Nome do Filme deve ter entre 3 a 60 caracteres!")]
        public string Nome { get; set; }
        public bool Ativo { get; set; }
        public bool Disponivel { get; set; }
        public DateTime Data_Cadastro { get; set; }


    }
}
