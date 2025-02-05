﻿namespace ARVTech.DataAccess.Service.UniPayCheck.Interfaces
{
    using ARVTech.DataAccess.DTOs.UniPayCheck;

    public interface IPessoaFisicaService
    {
        void Delete(Guid guid);

        PessoaFisicaResponseDto Get(Guid guid);

        IEnumerable<PessoaFisicaResponseDto> GetAll();

        IEnumerable<PessoaFisicaResponseDto> GetAniversariantes(string periodoInicialString, string periodoFinalString);

        PessoaFisicaResponseDto GetByCpf(string cpf);

        PessoaFisicaResponseDto GetByNome(string nome);

        PessoaFisicaResponseDto GetByNomeNumeroCtpsSerieCtpsAndUfCtps(string nome, string numeroCtps, string serieCtps, string ufCtps);

        PessoaFisicaResponseDto SaveData(PessoaFisicaRequestCreateDto? createDto = null, PessoaFisicaRequestUpdateDto? updateDto = null);
    }
}