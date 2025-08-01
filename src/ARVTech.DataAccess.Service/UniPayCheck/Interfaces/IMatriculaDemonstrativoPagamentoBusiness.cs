namespace ARVTech.DataAccess.Service.UniPayCheck.Interfaces
{
    using System;
    using System.Collections.Generic;
    using ARVTech.DataAccess.Domain.Enums.UniPayCheck;
    using ARVTech.DataAccess.DTOs.UniPayCheck;

    public interface IMatriculaDemonstrativoPagamentoService
    {
        MatriculaDemonstrativoPagamentoResponseDto Get(Guid guid);

        IEnumerable<MatriculaDemonstrativoPagamentoResponseDto> Get(string competencia, string matricula);

        IEnumerable<MatriculaDemonstrativoPagamentoResponseDto> GetAll();

        IEnumerable<MatriculaDemonstrativoPagamentoResponseDto> GetByGuidColaborador(Guid guidColaborador);

        IEnumerable<MatriculaDemonstrativoPagamentoResponseDto> GetPendencias(DateTime competenciaInicial, DateTime competenciaFinal, SituacaoPendenciaDemonstrativoPagamentoEnum situacao = SituacaoPendenciaDemonstrativoPagamentoEnum.Todos);

        ResumoImportacaoDemonstrativosPagamentoResponseDto ImportFileDemonstrativosPagamento(string cnpj, string content);

        MatriculaDemonstrativoPagamentoResponseDto SaveData(MatriculaDemonstrativoPagamentoRequestCreateDto? createDto = null, MatriculaDemonstrativoPagamentoRequestUpdateDto? updateDto = null);
    }
}