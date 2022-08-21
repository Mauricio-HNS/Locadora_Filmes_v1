using System;

namespace Locadora_De_Filmes.Domain.Entities
{
    public abstract class BaseEntity
    {
        public virtual string Id { get; set; }
        public virtual DateTime DataRegistro { get; set; }
        public virtual DateTime DataAtualizacao { get; set; }

        protected BaseEntity()
        {
            Id = Guid.NewGuid().ToString();
            DataRegistro = DateTime.Now;
            DataAtualizacao = DateTime.MinValue;
        }
    }
}
