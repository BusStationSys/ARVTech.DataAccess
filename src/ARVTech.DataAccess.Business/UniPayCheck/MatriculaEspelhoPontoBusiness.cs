namespace ARVTech.DataAccess.Business.UniPayCheck
{
    using System;
    using ARVTech.DataAccess.DTOs.UniPayCheck;
    using ARVTech.DataAccess.Core.Entities.UniPayCheck;
    using ARVTech.DataAccess.UnitOfWork.Interfaces;
    using ARVTech.Shared.Extensions;
    using ARVTech.Transmission.Engine.UniPayCheck.Results;
    using AutoMapper;

    public class MatriculaEspelhoPontoBusiness : BaseBusiness
    {
        private readonly int _idExtra050 = 1;
        private readonly int _idExtra070 = 2;
        private readonly int _idExtra100 = 3;
        private readonly int _idAdicionalNoturno = 4;
        private readonly int _idAtestado = 5;
        private readonly int _idPaternidade = 6;
        private readonly int _idSeguro = 7;
        private readonly int _idFaltas = 8;
        private readonly int _idFaltasJustificadas = 9;
        private readonly int _idAtrasos = 10;
        private readonly int _idCreditoBH = 11;
        private readonly int _idDebitoBH = 12;
        private readonly int _idSaldoBH = 13;
        private readonly int _idDispensaNaoRemunerada = 14;
        private readonly int _idGratAdFech = 15;

        private readonly DateTime _dataAdmissaoDefault = new(
            DateTime.Now.Year,
            1,
            1);

        private readonly string _agenciaDefault = "000000000";

        private readonly string _bancoDefault = "000";

        private readonly string _cidadeDefault = "ESTEIO";

        private readonly string _contaDefault = "000000000000000";

        private readonly string _cpfDefault = "00000000000";

        private readonly string _descricaoCargoDefault = "CARGO PADRÃO";

        private readonly string _enderecoDefault = "ENDERECO";

        private readonly string _numeroCtpsDefault = "0000000";

        private readonly decimal _salarioNominalDefault = 0.01M;

        private readonly string _serieCtpsDefault = "0000";

        private readonly string _ufCtpsDefault = "BR";

        private readonly string _ufDefault = "RS";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfWork"></param>
        public MatriculaEspelhoPontoBusiness(IUnitOfWork unitOfWork) :
            base(unitOfWork)
        {
            this._unitOfWork = unitOfWork;

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MatriculaEspelhoPontoDto, MatriculaEspelhoPontoEntity>().ReverseMap();
                cfg.CreateMap<MatriculaEspelhoPontoResponse, MatriculaEspelhoPontoEntity>().ReverseMap();
                cfg.CreateMap<MatriculaDto, MatriculaEntity>().ReverseMap();
                cfg.CreateMap<MatriculaResponse, MatriculaEntity>().ReverseMap();
                cfg.CreateMap<PessoaFisicaDto, PessoaFisicaEntity>().ReverseMap();
                cfg.CreateMap<PessoaJuridicaDto, PessoaJuridicaEntity>().ReverseMap();
                cfg.CreateMap<PessoaDto, PessoaEntity>().ReverseMap();
            });

            this._mapper = new Mapper(mapperConfiguration);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        public void Delete(Guid guid)
        {
            var connection = this._unitOfWork.Create();

            try
            {
                if (guid == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guid));

                connection.BeginTransaction();

                connection.RepositoriesUniPayCheck.MatriculaEspelhoPontoRepository.Delete(
                    guid);

                connection.CommitTransaction();
            }
            catch
            {
                if (connection.Transaction != null)
                {
                    connection.Rollback();
                }

                throw;
            }
            finally
            {
                connection.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="competencia"></param>
        /// <param name="guidMatricula"></param>
        public void DeleteLinksByCompetenciaAndGuidMatricula(string competencia, Guid guidMatricula)
        {
            var connection = this._unitOfWork.Create();

            try
            {
                if (string.IsNullOrEmpty(competencia))
                    throw new ArgumentNullException(
                        nameof(competencia));
                else if (guidMatricula == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guidMatricula));

                connection.BeginTransaction();

                connection.RepositoriesUniPayCheck.MatriculaEspelhoPontoRepository.DeleteLinksByCompetenciaAndGuidMatricula(
                    competencia,
                    guidMatricula);

                connection.CommitTransaction();
            }
            catch
            {
                if (connection.Transaction != null)
                {
                    connection.Rollback();
                }

                throw;
            }
            finally
            {
                connection.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public MatriculaEspelhoPontoResponse Get(Guid guid)
        {
            try
            {
                if (guid == Guid.Empty)
                    throw new ArgumentNullException(
                        nameof(guid));

                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesUniPayCheck.MatriculaDemonstrativoPagamentoRepository.Get(
                        guid);

                    return this._mapper.Map<MatriculaEspelhoPontoResponse>(
                        entity);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="competencia"></param>
        /// <param name="matricula"></param>
        /// <returns></returns>
        public MatriculaEspelhoPontoResponse GetByCompetenciaAndMatricula(string competencia, string matricula)
        {
            try
            {
                if (string.IsNullOrEmpty(competencia))
                    throw new ArgumentNullException(
                        nameof(
                            competencia));
                else if (string.IsNullOrEmpty(matricula))
                    throw new ArgumentNullException(
                        nameof(
                            matricula));

                using (var connection = this._unitOfWork.Create())
                {
                    var entity = connection.RepositoriesUniPayCheck.MatriculaEspelhoPontoRepository.GetByCompetenciaAndMatricula(
                        competencia,
                        matricula);

                    return this._mapper.Map<MatriculaEspelhoPontoResponse>(entity);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="espelhoPontoResult"></param>
        /// <returns></returns>
        public MatriculaEspelhoPontoResponse Import(EspelhoPontoResult espelhoPontoResult)
        {
            var connection = this._unitOfWork.Create();

            try
            {
                connection.BeginTransaction();

                //  Verifica se existe o registro do Empregador.
                var pessoaJuridicaResponse = default(PessoaJuridicaResponse);

                using (var pessoaJuridicaBusiness = new PessoaJuridicaBusiness(this._unitOfWork))
                {
                    pessoaJuridicaResponse = pessoaJuridicaBusiness.GetByRazaoSocial(
                        espelhoPontoResult.RazaoSocial);
                }

                //  Se não existir o registro do Empregador, adiciona.
                if (pessoaJuridicaResponse is null)
                {
                    var pessoaJuridicaDto = new PessoaJuridicaDto
                    {
                        Cnpj = espelhoPontoResult.Cnpj,
                        RazaoSocial = espelhoPontoResult.RazaoSocial.TreatStringWithAccent(),
                        Pessoa = new PessoaDto()
                        {
                            Cidade = this._cidadeDefault,
                            Endereco = this._enderecoDefault,
                            Uf = this._ufDefault,
                        },
                    };

                    using (var pessoaJuridicaBusiness = new PessoaJuridicaBusiness(this._unitOfWork))
                    {
                        pessoaJuridicaResponse = pessoaJuridicaBusiness.SaveData(
                            pessoaJuridicaDto);
                    }
                }

                //  Verifica se existe o registro da Matrícula.
                var matriculaResponse = default(MatriculaResponse);

                using (var matriculaBusiness = new MatriculaBusiness(this._unitOfWork))
                {
                    matriculaResponse = matriculaBusiness.GetByMatricula(
                        espelhoPontoResult.Matricula);
                }

                //  Se não existir o registro da Matrícula, adiciona.
                if (matriculaResponse is null)
                {
                    //  Verifica se existe o registro do Colaborador.
                    var pessoaFisicaResponse = default(PessoaFisicaResponse);

                    using (var pessoaFisicaBusiness = new PessoaFisicaBusiness(this._unitOfWork))
                    {
                        pessoaFisicaResponse = pessoaFisicaBusiness.GetByNome(
                            espelhoPontoResult.Nome.TreatStringWithAccent());
                    }

                    //  Se não existir o registro do Colaborador, adiciona.
                    if (pessoaFisicaResponse is null)
                    {
                        var pessoaFisicaDto = new PessoaFisicaDto
                        {
                            Nome = espelhoPontoResult.Nome.TreatStringWithAccent(),
                            NumeroCtps = this._numeroCtpsDefault,
                            SerieCtps = this._serieCtpsDefault,
                            UfCtps = this._ufCtpsDefault,
                            Cpf = this._cpfDefault,
                            Pessoa = new PessoaDto()
                            {
                                Cidade = this._cidadeDefault,
                                Endereco = this._enderecoDefault,
                                Uf = this._ufDefault,
                            },
                        };

                        using (var pessoaFisicaBusiness = new PessoaFisicaBusiness(this._unitOfWork))
                        {
                            pessoaFisicaResponse = pessoaFisicaBusiness.SaveData(
                                pessoaFisicaDto);
                        }
                    }

                    var matriculaDto = new MatriculaDto
                    {
                        GuidColaborador = pessoaFisicaResponse.Guid,
                        GuidEmpregador = pessoaJuridicaResponse.Guid,
                        Agencia = this._agenciaDefault,
                        Banco = this._bancoDefault,
                        CargaHoraria = Convert.ToDecimal(
                            espelhoPontoResult.CargaHoraria.Replace(".", ",")),
                        Conta = this._contaDefault,
                        DataAdmissao = this._dataAdmissaoDefault,
                        DescricaoCargo = this._descricaoCargoDefault,
                        DescricaoSetor = espelhoPontoResult.DescricaoSetor,
                        Matricula = espelhoPontoResult.Matricula,
                        SalarioNominal = this._salarioNominalDefault,
                    };

                    using (var matriculaBusiness = new MatriculaBusiness(
                        this._unitOfWork))
                    {
                        matriculaResponse = matriculaBusiness.SaveData(
                            matriculaDto);
                    }
                }

                string competencia = string.Concat("01/", espelhoPontoResult.Competencia);
                competencia = Convert.ToDateTime(competencia).ToString("yyyyMM");

                //  Verifica se existe o registro do Espelho Ponto da Matrícula.
                var matriculaEspelhoPontoResponse = default(MatriculaEspelhoPontoResponse);

                using (var matriculaEspelhoPontoBusiness = new MatriculaEspelhoPontoBusiness(
                    this._unitOfWork))
                {
                    //  Independente se existir um ou mais registros de Espelho de Ponto para a Matrícula, deve forçar a limpeza dos Itens dos Espelhos de Ponto que possam estar vinculado à Matrícula dentro da Competência.
                    matriculaEspelhoPontoBusiness.DeleteLinksByCompetenciaAndGuidMatricula(
                        competencia,
                        matriculaResponse.Guid);

                    matriculaEspelhoPontoResponse = matriculaEspelhoPontoBusiness.GetByCompetenciaAndMatricula(
                        competencia,
                        espelhoPontoResult.Matricula);

                    //  Se não existir o registro do Espelho de Ponto da Matrícula, adiciona.
                    if (matriculaEspelhoPontoResponse is null)
                    {
                        var matriculaEspelhoPontoDto = new MatriculaEspelhoPontoDto
                        {
                            GuidMatricula = matriculaResponse.Guid,
                            Competencia = competencia,
                        };

                        matriculaEspelhoPontoResponse = matriculaEspelhoPontoBusiness.SaveData(
                            matriculaEspelhoPontoDto);
                    }

                    // Processa os Cálculos do Espelho de Ponto.
                    if (espelhoPontoResult?.Marcacoes.Count > 0)
                    {
                        foreach (var resultMarcacao in espelhoPontoResult?.Marcacoes)
                        {
                            DateTime data = Convert.ToDateTime(
                                resultMarcacao.Data);

                            // Processa os Vínculos das Marcações.
                            this.processRecordEPMarcacao(
                                matriculaEspelhoPontoResponse.Guid,
                                resultMarcacao);
                        }
                    }

                    // Processa os Vínculos dos Cálculos.
                    decimal totalHE050 = decimal.Zero;
                    decimal totalHE070 = decimal.Zero;
                    decimal totalHE100 = decimal.Zero;
                    decimal totalAdicionalNoturno = decimal.Zero;
                    decimal totalAtestado = decimal.Zero;
                    decimal totalPaternidade = decimal.Zero;
                    decimal totalSeguro = decimal.Zero;
                    decimal totalFaltas = decimal.Zero;
                    decimal totalFaltasJustificadas = decimal.Zero;
                    decimal totalAtrasos = decimal.Zero;
                    decimal totalCreditoBH = decimal.Zero;
                    decimal totalDebitoBH = decimal.Zero;
                    decimal totalSaldoBH = decimal.Zero;
                    decimal totalDispensaNaoRemunerada = decimal.Zero;
                    decimal totalGratAdFech = decimal.Zero;

                    //  Processa as Horas Extras 50%.
                    if (!string.IsNullOrEmpty(espelhoPontoResult.TotalHE050))
                    {
                        totalHE050 = Convert.ToDecimal(
                            espelhoPontoResult.TotalHE050);
                    }

                    this.processRecordEPCalculo(
                        matriculaEspelhoPontoResponse.Guid,
                        this._idExtra050,
                        totalHE050);

                    //  Processa as Horas Extras 70%.
                    if (!string.IsNullOrEmpty(espelhoPontoResult.TotalHE070))
                    {
                        totalHE070 = Convert.ToDecimal(
                            espelhoPontoResult.TotalHE070);
                    }

                    this.processRecordEPCalculo(
                        matriculaEspelhoPontoResponse.Guid,
                        this._idExtra070,
                        totalHE070);

                    //  Processa as Horas Extras 100%.
                    if (!string.IsNullOrEmpty(espelhoPontoResult.TotalHE100))
                    {
                        totalHE100 = Convert.ToDecimal(
                            espelhoPontoResult.TotalHE100);
                    }

                    this.processRecordEPCalculo(
                        matriculaEspelhoPontoResponse.Guid,
                        this._idExtra100,
                        totalHE100);

                    //  Processa o Adicional Noturno.
                    if (!string.IsNullOrEmpty(espelhoPontoResult.TotalAdicionalNoturno))
                    {
                        totalAdicionalNoturno = Convert.ToDecimal(
                            espelhoPontoResult.TotalAdicionalNoturno);
                    }

                    this.processRecordEPCalculo(
                        matriculaEspelhoPontoResponse.Guid,
                        this._idAdicionalNoturno,
                        totalAdicionalNoturno);

                    //  Processa o Atestado.
                    if (!string.IsNullOrEmpty(espelhoPontoResult.TotalAtestado))
                    {
                        totalAtestado = Convert.ToDecimal(
                            espelhoPontoResult.TotalAtestado);
                    }

                    this.processRecordEPCalculo(
                        matriculaEspelhoPontoResponse.Guid,
                        this._idAtestado,
                        totalAtestado);

                    //  Processa o Paternidade.
                    if (!string.IsNullOrEmpty(espelhoPontoResult.TotalPaternidade))
                    {
                        totalPaternidade = Convert.ToDecimal(
                            espelhoPontoResult.TotalPaternidade);
                    }

                    this.processRecordEPCalculo(
                        matriculaEspelhoPontoResponse.Guid,
                        this._idPaternidade,
                        totalPaternidade);

                    //  Processa o Seguro.
                    if (!string.IsNullOrEmpty(espelhoPontoResult.TotalSeguro))
                    {
                        totalSeguro = Convert.ToDecimal(
                            espelhoPontoResult.TotalSeguro);
                    }

                    this.processRecordEPCalculo(
                        matriculaEspelhoPontoResponse.Guid,
                        this._idSeguro,
                        totalSeguro);

                    //  Processa as Faltas.
                    if (!string.IsNullOrEmpty(espelhoPontoResult.TotalFaltas))
                    {
                        totalFaltas = Convert.ToDecimal(
                            espelhoPontoResult.TotalFaltas);
                    }

                    this.processRecordEPCalculo(
                        matriculaEspelhoPontoResponse.Guid,
                        this._idFaltas,
                        totalFaltas);

                    //  Processa as Faltas.
                    if (!string.IsNullOrEmpty(espelhoPontoResult.TotalFaltasJustificadas))
                    {
                        totalFaltasJustificadas = Convert.ToDecimal(
                            espelhoPontoResult.TotalFaltasJustificadas);
                    }

                    this.processRecordEPCalculo(
                        matriculaEspelhoPontoResponse.Guid,
                        this._idFaltasJustificadas,
                        totalFaltasJustificadas);

                    //  Processa o Atrasos.
                    if (!string.IsNullOrEmpty(espelhoPontoResult.TotalAtrasos))
                    {
                        totalAtrasos = Convert.ToDecimal(
                            espelhoPontoResult.TotalAtrasos);
                    }

                    this.processRecordEPCalculo(
                        matriculaEspelhoPontoResponse.Guid,
                        this._idAtrasos,
                        totalAtrasos);

                    //  Processa o Crédito BH.
                    if (!string.IsNullOrEmpty(espelhoPontoResult.TotalCreditoBH))
                    {
                        totalCreditoBH = Convert.ToDecimal(
                            espelhoPontoResult.TotalCreditoBH);
                    }

                    this.processRecordEPCalculo(
                        matriculaEspelhoPontoResponse.Guid,
                        this._idCreditoBH,
                        totalCreditoBH);

                    //  Processa o Débito BH.
                    if (!string.IsNullOrEmpty(espelhoPontoResult.TotalDebitoBH))
                    {
                        totalDebitoBH = Convert.ToDecimal(
                            espelhoPontoResult.TotalDebitoBH);
                    }

                    this.processRecordEPCalculo(
                        matriculaEspelhoPontoResponse.Guid,
                        this._idDebitoBH,
                        totalDebitoBH);

                    //  Processa o Saldo BH.
                    if (!string.IsNullOrEmpty(espelhoPontoResult.TotalSaldoBH))
                    {
                        totalSaldoBH = Convert.ToDecimal(
                            espelhoPontoResult.TotalSaldoBH);
                    }

                    this.processRecordEPCalculo(
                        matriculaEspelhoPontoResponse.Guid,
                        this._idSaldoBH,
                        totalSaldoBH);

                    //  Processa a Dispensa Não Remunerada.
                    if (!string.IsNullOrEmpty(espelhoPontoResult.TotalDispensaNaoRemunerada))
                    {
                        totalDispensaNaoRemunerada = Convert.ToDecimal(
                            espelhoPontoResult.TotalDispensaNaoRemunerada);
                    }

                    this.processRecordEPCalculo(
                        matriculaEspelhoPontoResponse.Guid,
                        this._idDispensaNaoRemunerada,
                        totalDispensaNaoRemunerada);

                    //  Processa a Gratificação Ad. Fechamento.
                    if (!string.IsNullOrEmpty(espelhoPontoResult.TotalGratAdFech))
                    {
                        totalGratAdFech = Convert.ToDecimal(
                            espelhoPontoResult.TotalGratAdFech);
                    }

                    this.processRecordEPCalculo(
                        matriculaEspelhoPontoResponse.Guid,
                        this._idGratAdFech,
                        totalGratAdFech);
                }

                connection.CommitTransaction();

                return matriculaEspelhoPontoResponse;
            }
            catch
            {
                if (connection.Transaction != null)
                {
                    connection.Rollback();
                }

                throw;
            }
            finally
            {
                connection.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public MatriculaEspelhoPontoResponse SaveData(MatriculaEspelhoPontoDto dto)
        {
            var connection = this._unitOfWork.Create();

            try
            {
                var entity = this._mapper.Map<MatriculaEspelhoPontoEntity>(dto);

                connection.BeginTransaction();

                if (dto.Guid != null && dto.Guid != Guid.Empty)
                {
                    entity = connection.RepositoriesUniPayCheck.MatriculaEspelhoPontoRepository.Update(
                        entity);
                }
                else
                {
                    entity = connection.RepositoriesUniPayCheck.MatriculaEspelhoPontoRepository.Create(
                        entity);
                }

                connection.CommitTransaction();

                return this._mapper.Map<MatriculaEspelhoPontoResponse>(
                    entity);
            }
            catch
            {
                if (connection.Transaction != null)
                {
                    connection.Rollback();
                }

                throw;
            }
            finally
            {
                connection.Dispose();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guidMatriculaEspelhoPonto"></param>
        /// <param name="espelhoPontoMarcacaoResult"></param>
        private void processRecordEPMarcacao(Guid guidMatriculaEspelhoPonto, EspelhoPontoMarcacaoResult espelhoPontoMarcacaoResult)
        {
            try
            {
                //  Verifica se existe o registro do vínculo do Espelho de Ponto x Marcação.
                var matriculaEspelhoPontoMarcacaoDto = default(
                    MatriculaEspelhoPontoMarcacaoDto);

                using (var matriculaEspelhoPontoMarcacaoBusiness = new MatriculaEspelhoPontoMarcacaoBusiness(
                    this._unitOfWork))
                {
                    matriculaEspelhoPontoMarcacaoDto = matriculaEspelhoPontoMarcacaoBusiness.GetByGuidMatriculaEspelhoPontoAndData(
                        guidMatriculaEspelhoPonto,
                        espelhoPontoMarcacaoResult.Data);

                    //  Se não existir o registro do do vínculo do Espelho de Ponto x Marcação, adiciona.
                    if (matriculaEspelhoPontoMarcacaoDto is null)
                    {
                        matriculaEspelhoPontoMarcacaoDto = new MatriculaEspelhoPontoMarcacaoDto
                        {
                            GuidMatriculaEspelhoPonto = guidMatriculaEspelhoPonto,
                            Data = espelhoPontoMarcacaoResult.Data,
                        };
                    }

                    matriculaEspelhoPontoMarcacaoDto.Marcacao = espelhoPontoMarcacaoResult.Marcacao;
                    matriculaEspelhoPontoMarcacaoDto.HorasExtras050 = espelhoPontoMarcacaoResult.HE050.ToTimeSpan();
                    matriculaEspelhoPontoMarcacaoDto.HorasExtras070 = espelhoPontoMarcacaoResult.HE070.ToTimeSpan();
                    matriculaEspelhoPontoMarcacaoDto.HorasExtras100 = espelhoPontoMarcacaoResult.HE100.ToTimeSpan();
                    matriculaEspelhoPontoMarcacaoDto.HorasCreditoBH = espelhoPontoMarcacaoResult.CreditoBH.ToTimeSpan();
                    matriculaEspelhoPontoMarcacaoDto.HorasDebitoBH = espelhoPontoMarcacaoResult.DebitoBH.ToTimeSpan();
                    matriculaEspelhoPontoMarcacaoDto.HorasFaltas = espelhoPontoMarcacaoResult.HorasFaltas.ToTimeSpan();
                    matriculaEspelhoPontoMarcacaoDto.HorasTrabalhadas = espelhoPontoMarcacaoResult.HorasTrabalhadas.ToTimeSpan();

                    matriculaEspelhoPontoMarcacaoBusiness.SaveData(
                        matriculaEspelhoPontoMarcacaoDto);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guidMatriculaEspelhoPonto"></param>
        /// <param name="idCalculo"></param>
        /// <param name="valor"></param>
        private void processRecordEPCalculo(Guid guidMatriculaEspelhoPonto, int idCalculo, decimal valor)
        {
            try
            {
                //  Verifica se existe o registro do vínculo do Espelho de Ponto x Cálculo.
                var matriculaEspelhoPontoCalculoResponse = default(
                    MatriculaEspelhoPontoCalculoResponse);

                using (var matriculaEspelhoPontoCalculoBusiness = new MatriculaEspelhoPontoCalculoBusiness(
                    this._unitOfWork))
                {
                    matriculaEspelhoPontoCalculoResponse = matriculaEspelhoPontoCalculoBusiness.GetByGuidMatriculaEspelhoPontoAndIdCalculo(
                        guidMatriculaEspelhoPonto,
                        idCalculo);

                    //  Se não existir o registro do do vínculo do Demonstrativo de Pagamento x Totalizador, adiciona.
                    if (matriculaEspelhoPontoCalculoResponse is null)
                    {
                        var matriculaEspelhoPontoCalculoDto = new MatriculaEspelhoPontoCalculoDto
                        {
                            GuidMatriculaEspelhoPonto = guidMatriculaEspelhoPonto,
                            IdCalculo = idCalculo,
                            Valor = valor,
                        };

                        matriculaEspelhoPontoCalculoBusiness.SaveData(
                            matriculaEspelhoPontoCalculoDto);
                    }
                }
            }
            catch
            {
                throw;
            }
        }
    }
}