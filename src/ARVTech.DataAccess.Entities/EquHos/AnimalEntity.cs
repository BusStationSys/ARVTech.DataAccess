namespace ARVTech.DataAccess.Entities.EquHos
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ANIMAIS")]
    public class AnimalEntity
    {
        public virtual Guid Guid { get; set; }

        public virtual string Nome { get; set; }

        public virtual PelagemEntity Pelagem { get; set; }

        public override string ToString()
        {
            return $"Animal GUID: {this.Guid}; Nome: {this.Nome}.";
        }
    }
}
