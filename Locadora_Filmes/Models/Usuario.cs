using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Locadora_De_Filmes.Domain.Entities
{
    public class Usuario : IdentityUser
    {
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Foto { get; set; }
        public bool Ativo { get; set; }
        public bool Excluido { get; set; }
        public string CodigoAlterarSenha { get; set; }
        public DateTime DataRegistro { get; set; }
        public DateTime DataAtualizacao { get; set; }

        public Usuario(string nome, string sobrenome, string email)
        {
            Id=Guid.NewGuid().ToString();
            Nome=nome;
            Sobrenome=sobrenome;
            Email=email.ToLower();
            NormalizedEmail=email.ToUpper();
            UserName=email.ToLower();
            NormalizedUserName=email.ToUpper();
            Ativo=true;
            Excluido=false;
            DataRegistro=DateTime.Now;
            DataAtualizacao=DateTime.MinValue;
        }

        /* EF Relation */
        public IEnumerable<Usuario> Empresas { get; set; }
    }
}
