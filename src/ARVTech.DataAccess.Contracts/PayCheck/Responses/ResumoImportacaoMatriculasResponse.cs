namespace ARVTech.DataAccess.Contracts.PayCheck.Responses
{
    using System;

    public class ResumoImportacaoMatriculasResponse
    {
        public DateTime DataInicio { get; set; }

        public DateTime DataFim { get; set; }

        public int QuantidadeRegistrosAtualizados { get; set; }

        public int QuantidadeRegistrosInalterados { get; set; }

        public int QuantidadeRegistrosInseridos { get; set; }
    }
}