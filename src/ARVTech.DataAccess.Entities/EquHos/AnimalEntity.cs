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

        [Description("SBB")]
        public virtual string Sbb { get; set; }

        [Description("RP")]
        public virtual string Rp { get; set; }

        [Description("NOME")]
        public virtual string Nome { get; set; }

        [Description("SEXO")]
        public virtual string Sexo { get; set; }

        public virtual AnimalEntity Pai { get; set; }

        [Description("NOME_PAI")]
        public virtual string NomePai { get; set; }

        public virtual AnimalEntity Mae { get; set; }

        [Description("NOME_MAE")]
        public virtual string NomeMae { get; set; }

        public virtual ContaEntity Conta { get; set; }

        public virtual CabanhaEntity Cabanha { get; set; }

        public virtual PelagemEntity Pelagem { get; set; }

        public virtual TipoEntity Tipo { get; set; }

        public override string ToString()
        {
            return $"Animal GUID: {this.Guid}; Nome: {this.Nome}.";
        }
    }
}