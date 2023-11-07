namespace ARVTech.DataAccess.Business.UniPayCheck
{
    using System;
    using ARVTech.DataAccess.Core.Entities.UniPayCheck;
    using ARVTech.DataAccess.DTOs.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.UnitOfWork.Interfaces;
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
        /// Initializes a new instance of the <see cref="MatriculaEspelhoPontoBusiness"/> class.
        /// </summary>
        /// <param name="unitOfWork"></param>
        public MatriculaEspelhoPontoBusiness(IUnitOfWork unitOfWork) :
            base(unitOfWork)
        {
            this._unitOfWork = unitOfWork;

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CalculoRequestDto, CalculoEntity>().ReverseMap();
                cfg.CreateMap<CalculoResponseDto, CalculoEntity>().ReverseMap();
                cfg.CreateMap<MatriculaEspelhoPontoCalculoRequestDto, MatriculaEspelhoPontoCalculoEntity>().ReverseMap();
                cfg.CreateMap<MatriculaEspelhoPontoCalculoResponseDto, MatriculaEspelhoPontoCalculoEntity>().ReverseMap();
                cfg.CreateMap<MatriculaEspelhoPontoMarcacaoResponseDto, MatriculaEspelhoPontoMarcacaoEntity>().ReverseMap();
                cfg.CreateMap<MatriculaEspelhoPontoMarcacaoRequestDto, MatriculaEspelhoPontoMarcacaoEntity>().ReverseMap();
                cfg.CreateMap<MatriculaEspelhoPontoMarcacaoResponseDto, MatriculaEspelhoPontoEntity>().ReverseMap();
                cfg.CreateMap<MatriculaEspelhoPontoRequestCreateDto, MatriculaEspelhoPontoEntity>().ReverseMap();
                cfg.CreateMap<MatriculaEspelhoPontoRequestUpdateDto, MatriculaEspelhoPontoEntity>().ReverseMap();
                cfg.CreateMap<MatriculaEspelhoPontoResponseDto, MatriculaEspelhoPontoEntity>().ReverseMap();
                cfg.CreateMap<MatriculaRequestDto, MatriculaEntity>().ReverseMap();
                cfg.CreateMap<MatriculaResponseDto, MatriculaEntity>().ReverseMap();
                cfg.CreateMap<PessoaFisicaRequestCreateDto, PessoaFisicaEntity>().ReverseMap();
                cfg.CreateMap<PessoaFisicaRequestUpdateDto, PessoaFisicaEntity>().ReverseMap();
                cfg.CreateMap<PessoaFisicaResponseDto, PessoaFisicaEntity>().ReverseMap();
                cfg.CreateMap<PessoaJuridicaRequestCreateDto, PessoaJuridicaEntity>().ReverseMap();
                cfg.CreateMap<PessoaJuridicaRequestUpdateDto, PessoaJuridicaEntity>().ReverseMap();
                cfg.CreateMap<PessoaJuridicaResponseDto, PessoaJuridicaEntity>().ReverseMap();
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
        public void DeleteCalculosAndMarcacoesByCompetenciaAndGuidMatricula(string competencia, Guid guidMatricula)
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

                connection.RepositoriesUniPayCheck.MatriculaEspelhoPontoRepository.DeleteCalculosAndMarcacoesByCompetenciaAndGuidMatricula(
                    competencia,
                    guidMatricula);

                connection.CommitTransaction();
            }
            catch
            {
                if (connection.Transaction != null)
                    connection.Rollback();

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
        public MatriculaEspelhoPontoResponseDto Get(Guid guid)
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

                    return this._mapper.Map<MatriculaEspelhoPontoResponseDto>(
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
        public IEnumerable<MatriculaEspelhoPontoResponseDto> Get(string competencia, string matricula)
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
                    var entity = connection.RepositoriesUniPayCheck.MatriculaEspelhoPontoRepository.Get(
                        competencia,
                        matricula);

                    return this._mapper.Map<IEnumerable<MatriculaEspelhoPontoResponseDto>>(entity);
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
        public MatriculaEspelhoPontoResponseDto Import(EspelhoPontoResult espelhoPontoResult)
        {
            var connection = this._unitOfWork.Create();

            try
            {
                connection.BeginTransaction();

                //  Verifica se existe o registro do Empregador.
                var pessoaJuridicaResponseDto = default(
                    PessoaJuridicaResponseDto);

                using (var pessoaJuridicaBusiness = new PessoaJuridicaBusiness(
                    this._unitOfWork))
                {
                    pessoaJuridicaResponseDto = pessoaJuridicaBusiness.GetByRazaoSocial(
                        espelhoPontoResult.RazaoSocial);
                }

                //  Se não existir o registro do Empregador, adiciona.
                if (pessoaJuridicaResponseDto is null)
                {
                    var pessoaJuridicaDto = new PessoaJuridicaRequestCreateDto
                    {
                        Cnpj = espelhoPontoResult.Cnpj,
                        RazaoSocial = espelhoPontoResult.RazaoSocial.TreatStringWithAccent(),
                        Pessoa = new PessoaRequestCreateDto()
                        {
                            Cidade = this._cidadeDefault,
                            Endereco = this._enderecoDefault,
                            Uf = this._ufDefault,
                        },
                    };

                    using (var pessoaJuridicaBusiness = new PessoaJuridicaBusiness(this._unitOfWork))
                    {
                        pessoaJuridicaResponseDto = pessoaJuridicaBusiness.SaveData(
                            pessoaJuridicaDto);
                    }
                }

                //  Verifica se existe o registro da Matrícula.
                var matriculaResponseDto = default(
                    MatriculaResponseDto);

                using (var matriculaBusiness = new MatriculaBusiness(this._unitOfWork))
                {
                    matriculaResponseDto = matriculaBusiness.GetByMatricula(
                        espelhoPontoResult.Matricula);
                }

                //  Se não existir o registro da Matrícula, adiciona.
                if (matriculaResponseDto is null)
                {
                    //  Verifica se existe o registro do Colaborador.
                    var pessoaFisicaResponseDto = default(PessoaFisicaResponseDto);

                    using (var pessoaFisicaBusiness = new PessoaFisicaBusiness(
                        this._unitOfWork))
                    {
                        pessoaFisicaResponseDto = pessoaFisicaBusiness.GetByNome(
                            espelhoPontoResult.Nome.TreatStringWithAccent());
                    }

                    //  Se não existir o registro do Colaborador, adiciona.
                    if (pessoaFisicaResponseDto is null)
                    {
                        var PessoaFisicaRequestDto = new PessoaFisicaRequestCreateDto
                        {
                            Nome = espelhoPontoResult.Nome.TreatStringWithAccent(),
                            NumeroCtps = this._numeroCtpsDefault,
                            SerieCtps = this._serieCtpsDefault,
                            UfCtps = this._ufCtpsDefault,
                            Cpf = this._cpfDefault,
                            Pessoa = new PessoaRequestCreateDto
                            {
                                Cidade = this._cidadeDefault,
                                Endereco = this._enderecoDefault,
                                Uf = this._ufDefault,
                            },
                        };

                        using (var pessoaFisicaBusiness = new PessoaFisicaBusiness(this._unitOfWork))
                        {
                            pessoaFisicaResponseDto = pessoaFisicaBusiness.SaveData(
                                PessoaFisicaRequestDto);
                        }
                    }

                    var matriculaRequestDto = new MatriculaRequestDto
                    {
                        GuidColaborador = pessoaFisicaResponseDto.Guid,
                        GuidEmpregador = pessoaJuridicaResponseDto.Guid,
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
                        matriculaResponseDto = matriculaBusiness.SaveData(
                            matriculaRequestDto);
                    }
                }

                string competencia = string.Concat(
                    "01/",
                    espelhoPontoResult.Competencia);

                competencia = Convert.ToDateTime(competencia).ToString("yyyyMM");

                //  Verifica se existe o registro do Espelho Ponto da Matrícula.
                var matriculaEspelhoPontoResponseDto = default(MatriculaEspelhoPontoResponseDto);

                //  Independente se existir um ou mais registros de Espelho de Ponto para a Matrícula, deve forçar a limpeza dos Itens dos Espelhos de Ponto que possam estar vinculado à Matrícula dentro da Competência.
                this.DeleteCalculosAndMarcacoesByCompetenciaAndGuidMatricula(
                    competencia,
                    matriculaResponseDto.Guid);

                matriculaEspelhoPontoResponseDto = this.Get(
                    competencia,
                    espelhoPontoResult.Matricula).FirstOrDefault();

                //  Se não existir o registro do Espelho de Ponto da Matrícula, adiciona.
                if (matriculaEspelhoPontoResponseDto is null)
                {
                    var matriculaEspelhoPontoRequestCreateDto = new MatriculaEspelhoPontoRequestCreateDto
                    {
                        GuidMatricula = matriculaResponseDto.Guid,
                        Competencia = competencia,
                    };

                    matriculaEspelhoPontoResponseDto = this.SaveData(
                        matriculaEspelhoPontoRequestCreateDto);
                }

                // Processa os Cálculos do Espelho de Ponto.
                if (espelhoPontoResult?.Marcacoes.Count > 0)
                {
                    foreach (var resultMarcacao in espelhoPontoResult?.Marcacoes)
                    {
                        // Processa os Vínculos das Marcações.
                        this.processRecordEPMarcacao(
                            matriculaEspelhoPontoResponseDto.Guid,
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
                    matriculaEspelhoPontoResponseDto.Guid,
                    this._idExtra050,
                    totalHE050);

                //  Processa as Horas Extras 70%.
                if (!string.IsNullOrEmpty(espelhoPontoResult.TotalHE070))
                {
                    totalHE070 = Convert.ToDecimal(
                        espelhoPontoResult.TotalHE070);
                }

                this.processRecordEPCalculo(
                    matriculaEspelhoPontoResponseDto.Guid,
                    this._idExtra070,
                    totalHE070);

                //  Processa as Horas Extras 100%.
                if (!string.IsNullOrEmpty(espelhoPontoResult.TotalHE100))
                {
                    totalHE100 = Convert.ToDecimal(
                        espelhoPontoResult.TotalHE100);
                }

                this.processRecordEPCalculo(
                    matriculaEspelhoPontoResponseDto.Guid,
                    this._idExtra100,
                    totalHE100);

                //  Processa o Adicional Noturno.
                if (!string.IsNullOrEmpty(espelhoPontoResult.TotalAdicionalNoturno))
                {
                    totalAdicionalNoturno = Convert.ToDecimal(
                        espelhoPontoResult.TotalAdicionalNoturno);
                }

                this.processRecordEPCalculo(
                    matriculaEspelhoPontoResponseDto.Guid,
                    this._idAdicionalNoturno,
                    totalAdicionalNoturno);

                //  Processa o Atestado.
                if (!string.IsNullOrEmpty(espelhoPontoResult.TotalAtestado))
                {
                    totalAtestado = Convert.ToDecimal(
                        espelhoPontoResult.TotalAtestado);
                }

                this.processRecordEPCalculo(
                    matriculaEspelhoPontoResponseDto.Guid,
                    this._idAtestado,
                    totalAtestado);

                //  Processa o Paternidade.
                if (!string.IsNullOrEmpty(espelhoPontoResult.TotalPaternidade))
                {
                    totalPaternidade = Convert.ToDecimal(
                        espelhoPontoResult.TotalPaternidade);
                }

                this.processRecordEPCalculo(
                    matriculaEspelhoPontoResponseDto.Guid,
                    this._idPaternidade,
                    totalPaternidade);

                //  Processa o Seguro.
                if (!string.IsNullOrEmpty(espelhoPontoResult.TotalSeguro))
                {
                    totalSeguro = Convert.ToDecimal(
                        espelhoPontoResult.TotalSeguro);
                }

                this.processRecordEPCalculo(
                    matriculaEspelhoPontoResponseDto.Guid,
                    this._idSeguro,
                    totalSeguro);

                //  Processa as Faltas.
                if (!string.IsNullOrEmpty(espelhoPontoResult.TotalFaltas))
                {
                    totalFaltas = Convert.ToDecimal(
                        espelhoPontoResult.TotalFaltas);
                }

                this.processRecordEPCalculo(
                    matriculaEspelhoPontoResponseDto.Guid,
                    this._idFaltas,
                    totalFaltas);

                //  Processa as Faltas.
                if (!string.IsNullOrEmpty(espelhoPontoResult.TotalFaltasJustificadas))
                {
                    totalFaltasJustificadas = Convert.ToDecimal(
                        espelhoPontoResult.TotalFaltasJustificadas);
                }

                this.processRecordEPCalculo(
                    matriculaEspelhoPontoResponseDto.Guid,
                    this._idFaltasJustificadas,
                    totalFaltasJustificadas);

                //  Processa o Atrasos.
                if (!string.IsNullOrEmpty(espelhoPontoResult.TotalAtrasos))
                {
                    totalAtrasos = Convert.ToDecimal(
                        espelhoPontoResult.TotalAtrasos);
                }

                this.processRecordEPCalculo(
                    matriculaEspelhoPontoResponseDto.Guid,
                    this._idAtrasos,
                    totalAtrasos);

                //  Processa o Crédito BH.
                if (!string.IsNullOrEmpty(espelhoPontoResult.TotalCreditoBH))
                {
                    totalCreditoBH = Convert.ToDecimal(
                        espelhoPontoResult.TotalCreditoBH);
                }

                this.processRecordEPCalculo(
                    matriculaEspelhoPontoResponseDto.Guid,
                    this._idCreditoBH,
                    totalCreditoBH);

                //  Processa o Débito BH.
                if (!string.IsNullOrEmpty(espelhoPontoResult.TotalDebitoBH))
                {
                    totalDebitoBH = Convert.ToDecimal(
                        espelhoPontoResult.TotalDebitoBH);
                }

                this.processRecordEPCalculo(
                    matriculaEspelhoPontoResponseDto.Guid,
                    this._idDebitoBH,
                    totalDebitoBH);

                //  Processa o Saldo BH.
                if (!string.IsNullOrEmpty(espelhoPontoResult.TotalSaldoBH))
                {
                    totalSaldoBH = Convert.ToDecimal(
                        espelhoPontoResult.TotalSaldoBH);
                }

                this.processRecordEPCalculo(
                    matriculaEspelhoPontoResponseDto.Guid,
                    this._idSaldoBH,
                    totalSaldoBH);

                //  Processa a Dispensa Não Remunerada.
                if (!string.IsNullOrEmpty(espelhoPontoResult.TotalDispensaNaoRemunerada))
                {
                    totalDispensaNaoRemunerada = Convert.ToDecimal(
                        espelhoPontoResult.TotalDispensaNaoRemunerada);
                }

                this.processRecordEPCalculo(
                    matriculaEspelhoPontoResponseDto.Guid,
                    this._idDispensaNaoRemunerada,
                    totalDispensaNaoRemunerada);

                //  Processa a Gratificação Ad. Fechamento.
                if (!string.IsNullOrEmpty(espelhoPontoResult.TotalGratAdFech))
                {
                    totalGratAdFech = Convert.ToDecimal(
                        espelhoPontoResult.TotalGratAdFech);
                }

                this.processRecordEPCalculo(
                    matriculaEspelhoPontoResponseDto.Guid,
                    this._idGratAdFech,
                    totalGratAdFech);

                connection.CommitTransaction();

                return matriculaEspelhoPontoResponseDto;
            }
            catch
            {
                if (connection.Transaction != null)
                    connection.Rollback();

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
        /// <param name="createDto"></param>
        /// <param name="updateDto"></param>
        /// <returns></returns>
        public MatriculaEspelhoPontoResponseDto SaveData(MatriculaEspelhoPontoRequestCreateDto? createDto = null, MatriculaEspelhoPontoRequestUpdateDto? updateDto = null)
        {
            var connection = this._unitOfWork.Create();

            try
            {
                if (createDto != null && updateDto != null)
                    throw new InvalidOperationException($"{nameof(createDto)} e {nameof(updateDto)} não podem estar preenchidos ao mesmo tempo.");
                else if (createDto is null && updateDto is null)
                    throw new InvalidOperationException($"{nameof(createDto)} e {nameof(updateDto)} não podem estar vazios ao mesmo tempo.");
                else if (updateDto != null && updateDto.Guid == Guid.Empty)
                    throw new InvalidOperationException($"É necessário o preenchimento do {nameof(updateDto.Guid)}.");

                var entity = default(
                    MatriculaEspelhoPontoEntity);

                connection.BeginTransaction();

                if (updateDto != null)
                {
                    entity = this._mapper.Map<MatriculaEspelhoPontoEntity>(
                        updateDto);

                    entity = connection.RepositoriesUniPayCheck.MatriculaEspelhoPontoRepository.Update(
                        entity.Guid,
                        entity);
                }
                else if (createDto != null)
                {
                    entity = this._mapper.Map<MatriculaEspelhoPontoEntity>(
                        createDto);

                    entity = connection.RepositoriesUniPayCheck.MatriculaEspelhoPontoRepository.Create(
                        entity);
                }

                connection.CommitTransaction();

                return this._mapper.Map<MatriculaEspelhoPontoResponseDto>(
                    entity);
            }
            catch
            {
                if (connection.Transaction != null)
                    connection.Rollback();

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
                var matriculaEspelhoPontoMarcacaoResponseDto = default(
                    MatriculaEspelhoPontoMarcacaoResponseDto);

                using (var matriculaEspelhoPontoMarcacaoBusiness = new MatriculaEspelhoPontoMarcacaoBusiness(
                    this._unitOfWork))
                {
                    matriculaEspelhoPontoMarcacaoResponseDto = matriculaEspelhoPontoMarcacaoBusiness.GetByGuidMatriculaEspelhoPontoAndData(
                        guidMatriculaEspelhoPonto,
                        espelhoPontoMarcacaoResult.Data);

                    //  Se não existir o registro do do vínculo do Espelho de Ponto x Marcação, adiciona.
                    var matriculaEspelhoPontoMarcacaoRequestDto = default(
                        MatriculaEspelhoPontoMarcacaoRequestDto);

                    if (matriculaEspelhoPontoMarcacaoResponseDto is null)
                    {
                        matriculaEspelhoPontoMarcacaoRequestDto = new MatriculaEspelhoPontoMarcacaoRequestDto
                        {
                            GuidMatriculaEspelhoPonto = guidMatriculaEspelhoPonto,
                            Data = espelhoPontoMarcacaoResult.Data,
                        };
                    }
                    else
                    {
                        matriculaEspelhoPontoMarcacaoRequestDto = new MatriculaEspelhoPontoMarcacaoRequestDto
                        {
                            GuidMatriculaEspelhoPonto = matriculaEspelhoPontoMarcacaoResponseDto.GuidMatriculaEspelhoPonto,
                            Data = matriculaEspelhoPontoMarcacaoResponseDto.Data,
                        };
                    }

                    matriculaEspelhoPontoMarcacaoRequestDto.Marcacao = espelhoPontoMarcacaoResult.Marcacao;
                    matriculaEspelhoPontoMarcacaoRequestDto.HorasExtras050 = espelhoPontoMarcacaoResult.HE050.ToTimeSpan();
                    matriculaEspelhoPontoMarcacaoRequestDto.HorasExtras070 = espelhoPontoMarcacaoResult.HE070.ToTimeSpan();
                    matriculaEspelhoPontoMarcacaoRequestDto.HorasExtras100 = espelhoPontoMarcacaoResult.HE100.ToTimeSpan();
                    matriculaEspelhoPontoMarcacaoRequestDto.HorasCreditoBH = espelhoPontoMarcacaoResult.CreditoBH.ToTimeSpan();
                    matriculaEspelhoPontoMarcacaoRequestDto.HorasDebitoBH = espelhoPontoMarcacaoResult.DebitoBH.ToTimeSpan();
                    matriculaEspelhoPontoMarcacaoRequestDto.HorasFaltas = espelhoPontoMarcacaoResult.HorasFaltas.ToTimeSpan();
                    matriculaEspelhoPontoMarcacaoRequestDto.HorasTrabalhadas = espelhoPontoMarcacaoResult.HorasTrabalhadas.ToTimeSpan();

                    matriculaEspelhoPontoMarcacaoBusiness.SaveData(
                        matriculaEspelhoPontoMarcacaoRequestDto);
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
                var matriculaEspelhoPontoCalculoResponseDto = default(
                    MatriculaEspelhoPontoCalculoResponseDto);

                using (var matriculaEspelhoPontoCalculoBusiness = new MatriculaEspelhoPontoCalculoBusiness(
                    this._unitOfWork))
                {
                    matriculaEspelhoPontoCalculoResponseDto = matriculaEspelhoPontoCalculoBusiness.GetByGuidMatriculaEspelhoPontoAndIdCalculo(
                        guidMatriculaEspelhoPonto,
                        idCalculo);

                    //  Se não existir o registro do do vínculo do Demonstrativo de Pagamento x Totalizador, adiciona.
                    if (matriculaEspelhoPontoCalculoResponseDto is null)
                    {
                        var matriculaEspelhoPontoCalculoRequestDto = new MatriculaEspelhoPontoCalculoRequestDto
                        {
                            GuidMatriculaEspelhoPonto = guidMatriculaEspelhoPonto,
                            IdCalculo = idCalculo,
                            Valor = valor,
                        };

                        matriculaEspelhoPontoCalculoBusiness.SaveData(
                            matriculaEspelhoPontoCalculoRequestDto);
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