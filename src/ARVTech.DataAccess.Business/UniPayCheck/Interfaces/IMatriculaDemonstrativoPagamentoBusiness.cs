namespace ARVTech.DataAccess.Business.UniPayCheck.Interfaces
{
    using System;
    using System.Collections.Generic;
    using ARVTech.DataAccess.Core.Entities.UniPayCheck;
    using ARVTech.DataAccess.Core.Enums;
    using ARVTech.DataAccess.DTOs.UniPayCheck;
    using ARVTech.DataAccess.Enums;

    public interface IMatriculaDemonstrativoPagamentoBusiness
    {
        MatriculaDemonstrativoPagamentoResponseDto Get(Guid guid);

        IEnumerable<MatriculaDemonstrativoPagamentoResponseDto> Get(string competencia, string matricula);

        IEnumerable<MatriculaDemonstrativoPagamentoResponseDto> GetAll();

        IEnumerable<MatriculaDemonstrativoPagamentoResponseDto> GetByGuidColaborador(Guid guidColaborador);

        IEnumerable<MatriculaDemonstrativoPagamentoResponseDto> GetPendencias(DateTime competenciaInicial, DateTime competenciaFinal, SituacaoPendenciaDemonstrativoPagamentoEnum situacao = SituacaoPendenciaDemonstrativoPagamentoEnum.Todos);

        MatriculaDemonstrativoPagamentoResponseDto SaveData(MatriculaDemonstrativoPagamentoRequestCreateDto? createDto = null, MatriculaDemonstrativoPagamentoRequestUpdateDto? updateDto = null);
    }
}