namespace ARVTech.DataAccess.Domain.Entities.EquHos
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ANIMAIS")]
    public class AnimalEntity
    {
        [Description("GUID")]
        public Guid Guid { get; set; }

        [Description("SBB")]
        public string Sbb { get; set; }

        [Description("RP")]
        public string Rp { get; set; }

        [Description("NOME")]
        public string Nome { get; set; }

        [Description("SEXO")]
        public string Sexo { get; set; }

        [Description("DATA_NASCIMENTO")]
        public DateTime? DataNascimento { get; set; }

        [Description("IDPELAGEM")]
        public int? IdPelagem { get; set; }

        public PelagemEntity Pelagem { get; set; }

        [Description("GUID_PAI")]
        public Guid? GuidPai { get; set; }

        public AnimalEntity Pai { get; set; }

        [Description("NOME_PAI")]
        public string NomePai { get; set; }

        [Description("GUID_MAE")]
        public Guid? GuidMae { get; set; }

        public AnimalEntity Mae { get; set; }

        [Description("NOME_MAE")]
        public string NomeMae { get; set; }

        [Description("GUIDCONTA")]
        public Guid GuidConta { get; set; }

        public ContaEntity Conta { get; set; }

        [Description("GUIDCABANHA")]
        public Guid GuidCabanha { get; set; }

        public CabanhaEntity Cabanha { get; set; }

        [Description("IDTIPO")]
        public int? IdTipo { get; set; }

        public TipoEntity Tipo { get; set; }

        public override string ToString()
        {
            return $"Animal GUID: {this.Guid}; Nome: {this.Nome}.";
        }
    }
}