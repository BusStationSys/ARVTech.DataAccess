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

        [Description("DATA_NASCIMENTO")]
        public virtual DateTime? DataNascimento { get; set; }

        [Description("IDPELAGEM")]
        public virtual int? IdPelagem { get; set; }

        public virtual PelagemEntity Pelagem { get; set; }

        [Description("GUID_PAI")]
        public virtual Guid? GuidPai { get; set; }

        public virtual AnimalEntity Pai { get; set; }

        [Description("NOME_PAI")]
        public virtual string NomePai { get; set; }

        [Description("GUID_MAE")]
        public virtual Guid? GuidMae { get; set; }

        public virtual AnimalEntity Mae { get; set; }

        [Description("NOME_MAE")]
        public virtual string NomeMae { get; set; }

        [Description("GUIDCONTA")]
        public virtual Guid GuidConta { get; set; }

        public virtual ContaEntity Conta { get; set; }

        [Description("GUIDCABANHA")]
        public virtual Guid GuidCabanha { get; set; }

        public virtual CabanhaEntity Cabanha { get; set; }

        [Description("IDTIPO")]
        public virtual int? IdTipo { get; set; }

        public virtual TipoEntity Tipo { get; set; }

        public override string ToString()
        {
            return $"Animal GUID: {this.Guid}; Nome: {this.Nome}.";
        }
    }
}