namespace ARVTech.DataAccess.Entities.EquHos
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ANIMAIS")]
    public class AnimalEntity
    {
        [Description("GUID")]
        public virtual Guid Guid { get; set; }

        [Description("NOME")]
        public virtual string Nome { get; set; }

        public virtual PelagemEntity Pelagem { get; set; }

        public override string ToString()
        {
            return $"Animal GUID: {this.Guid}; Nome: {this.Nome}.";
        }
    }
}
