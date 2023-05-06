﻿namespace ARVTech.DataAccess.Entities.EquHos
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CATEGORIAS_USUARIOS")]
    public class CategoriaUsuarioEntity
    {
        [Description("ID")]     //  DataAnnotation "Description" é usado para casos em que os campos da tabela estão diferentes das propriedades da classe.
        public virtual int Id { get; set; }

        [Description("DESCRICAO")]
        public virtual string Descricao { get; set; }

        public override string ToString()
        {
            return $"Tipo ID: {this.Id}; Descrição: {this.Descricao}.";
        }
    }
}