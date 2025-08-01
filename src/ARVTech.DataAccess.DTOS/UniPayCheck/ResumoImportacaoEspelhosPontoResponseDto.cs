namespace ARVTech.DataAccess.DTOs.UniPayCheck
{
    using System.ComponentModel;

    public class ResumoImportacaoEspelhosPontoResponseDto
    {
        [Description("DATA_INICIO")]
        public DateTime DataInicio { get; set; }

        [Description("DATA_FIM")]
        public DateTime DataFim { get; set; }

        [Description("QUANTIDADE_REGISTROS_ATUALIZADOS")]
        public int QuantidadeRegistrosAtualizados { get; set; }

        [Description("QUANTIDADE_REGISTROS_INALTERADOS")]
        public int QuantidadeRegistrosInalterados { get; set; }

        [Description("QUANTIDADE_REGISTROS_INSERIDOS")]
        public int QuantidadeRegistrosInseridos { get; set; }

        [Description("QUANTIDADE_REGISTROS_REJEITADOS")]
        public int QuantidadeRegistrosRejeitados { get; set; }
    }
}