﻿namespace ARVTech.DataAccess.Core.Entities.EquHos
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PELAGENS")]
    public class PelagemEntity
    {
        [Description("ID")]     //  DataAnnotation "Description" é usado para casos em que os campos da tabela estão diferentes das propriedades da classe.
        public virtual int Id { get; set; }

        [Description("DESCRICAO")]
        public virtual string Descricao { get; set; }

        [Description("OBSERVACOES")]
        public virtual string Observacoes { get; set; }

        public virtual ICollection<AnimalEntity> Animais { get; set; }

        public override string ToString()
        {
            return $"Pelagem ID: {this.Id}; Descrição: {this.Descricao}.";
        }
    }
}