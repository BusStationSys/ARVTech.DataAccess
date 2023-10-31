namespace ARVTech.DataAccess.Business.UniPayCheck
{
    using System;
    using ARVTech.DataAccess.Core.Entities.UniPayCheck;
    using ARVTech.DataAccess.DTOs.UniPayCheck;
    using ARVTech.DataAccess.Infrastructure.UnitOfWork.Interfaces;
    using ARVTech.Shared;
    using ARVTech.Transmission.Engine.UniPayCheck.Results;
    using AutoMapper;

    public class MatriculaDemonstrativoPagamentoBusiness : BaseBusiness
    {
        private readonly int _idBaseFgts = 1;
        private readonly int _idValorFgts = 2;
        private readonly int _idTotalVencimentos = 3;
        private readonly int _idTotalDescontos = 4;
        private readonly int _idBaseIrrf = 5;
        private readonly int _idBaseInss = 6;
        private readonly int _idTotalLiquido = 7;

        private readonly decimal _cargaHorariaDefault = 220M;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfWork"></param>
        public MatriculaDemonstrativoPagamentoBusiness(IUnitOfWork unitOfWork) :
            base(unitOfWork)
        {
            this._unitOfWork = unitOfWork;

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MatriculaDemonstrativoPagamentoRequestCreateDto, MatriculaDemonstrativoPagamentoEntity>().ReverseMap();
                cfg.CreateMap<MatriculaDemonstrativoPagamentoRequestUpdateDto, MatriculaDemonstrativoPagamentoEntity>().ReverseMap();
                cfg.CreateMap<MatriculaDemonstrativoPagamentoResponseDto, MatriculaDemonstrativoPagamentoEntity>().ReverseMap();
                cfg.CreateMap<MatriculaRequestDto, MatriculaEntity>().ReverseMap();
                cfg.CreateMap<MatriculaResponseDto, MatriculaEntity>().ReverseMap();
                cfg.CreateMap<PessoaFisicaRequestDto, PessoaFisicaEntity>().ReverseMap();
                cfg.CreateMap<PessoaFisicaResponseDto, PessoaFisicaEntity>().ReverseMap();
                cfg.CreateMap<PessoaJuridicaRequestDto, PessoaJuridicaEntity>().ReverseMap();
                cfg.CreateMap<PessoaJuridicaResponseDto, PessoaJuridicaEntity>().ReverseMap();
                cfg.CreateMap<PessoaRequestDto, PessoaEntity>().ReverseMap();
                cfg.CreateMap<PessoaResponseDto, PessoaEntity>().ReverseMap();
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

                connection.RepositoriesUniPayCheck.MatriculaDemonstrativoPagamentoRepository.Delete(
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
        public void Delete(string competencia, Guid guidMatricula)
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

                connection.RepositoriesUniPayCheck.MatriculaDemonstrativoPagamentoRepository.DeleteEventosAndTotalizadoresByCompetenciaAndGuidMatricula(
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
        public MatriculaDemonstrativoPagamentoResponseDto Get(Guid guid)
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

                    return this._mapper.Map<MatriculaDemonstrativoPagamentoResponseDto>(
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
        public IEnumerable<MatriculaDemonstrativoPagamentoResponseDto> Get(string competencia, string matricula)
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
                    var entity = connection.RepositoriesUniPayCheck.MatriculaDemonstrativoPagamentoRepository.Get(
                        competencia,
                        matricula);

                    return this._mapper.Map<IEnumerable<MatriculaDemonstrativoPagamentoResponseDto>>(entity);
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
        /// <param name="demonstrativoPagamentoResult"></param>
        /// <returns></returns>
        public MatriculaDemonstrativoPagamentoResponseDto Import(DemonstrativoPagamentoResult demonstrativoPagamentoResult)
        {
            var connection = this._unitOfWork.Create();

            try
            {
                connection.BeginTransaction();

                //  Verifica se existe o registro do Colaborador.
                var pessoaFisicaResponseDto = default(
                    PessoaFisicaResponseDto);

                using (var pessoaFisicaBusiness = new PessoaFisicaBusiness(this._unitOfWork))
                {
                    //PessoaFisicaRequestDto = pessoaFisicaBusiness.GetByNomeNumeroCtpsSerieCtpsAndUfCtps(
                    //    demonstrativoPagamentoResult.Nome,
                    //    demonstrativoPagamentoResult.NumeroCtps,
                    //    demonstrativoPagamentoResult.SerieCtps,
                    //    demonstrativoPagamentoResult.UfCtps);

                    pessoaFisicaResponseDto = pessoaFisicaBusiness.GetByNome(
                        demonstrativoPagamentoResult.Nome);
                }

                //  Se não existir o registro do Colaborador, deve incluir o registro.
                if (pessoaFisicaResponseDto is null)
                {
                    var pessoaFisicaRequestDto = new PessoaFisicaRequestDto
                    {
                        Nome = demonstrativoPagamentoResult.Nome,
                        NumeroCtps = demonstrativoPagamentoResult.NumeroCtps,
                        SerieCtps = demonstrativoPagamentoResult.SerieCtps,
                        UfCtps = demonstrativoPagamentoResult.UfCtps,
                        Cpf = demonstrativoPagamentoResult.Cpf,
                        Pessoa = new PessoaRequestDto()
                        {
                            Cidade = "ESTEIO",
                            Endereco = "ENDERECO",
                            Uf = "RS",
                        },
                    };

                    using (var pessoaFisicaBusiness = new PessoaFisicaBusiness(this._unitOfWork))
                    {
                        pessoaFisicaResponseDto = pessoaFisicaBusiness.SaveData(
                            pessoaFisicaRequestDto);
                    }

                    //throw new Exception(
                    //    $"Colaborador {demonstrativoPagamentoResult.Nome} não encontrado na tabela de Pessoas Físicas. Por gentileza, cadastre com o Nome exibido e os demais campos chaves e obrigatórios.");
                }

                //  Verifica se existe o registro do Empregador.
                var pessoaJuridicaResponseDto = default(
                    PessoaJuridicaResponseDto);

                using (var pessoaJuridicaBusiness = new PessoaJuridicaBusiness(this._unitOfWork))
                {
                    pessoaJuridicaResponseDto = pessoaJuridicaBusiness.GetByRazaoSocial(
                        demonstrativoPagamentoResult.RazaoSocial);
                }

                //  Se não existir o registro do Empregador, deve disparar uma exceção.
                if (pessoaJuridicaResponseDto is null)
                {
                    //pessoaJuridicaDto = new PessoaJuridicaDto
                    //{
                    //    Cnpj = demonstrativoPagamentoResult.Cnpj,
                    //    RazaoSocial = demonstrativoPagamentoResult.RazaoSocial,
                    //    Pessoa = new PessoaDto()
                    //    {
                    //        Cidade = "ESTEIO",
                    //        Endereco = "ENDERECO",
                    //        Uf = "RS",
                    //    },
                    //};

                    //using (var pessoaJuridicaBusiness = new PessoaJuridicaBusiness(this._unitOfWork))
                    //{
                    //    pessoaJuridicaDto = pessoaJuridicaBusiness.SaveData(
                    //        pessoaJuridicaDto);
                    //}

                    throw new Exception(
                        $"Empregador {demonstrativoPagamentoResult.RazaoSocial} não encontrado na tabela de Pessoas Jurídicas. Por gentileza, cadastre com a Razão Social exibida e os demais campos chaves e obrigatórios.");
                }

                //  Verifica se existe o registro da Matrícula.
                var matriculaResponseDto = default(MatriculaResponseDto);

                using (var matriculaBusiness = new MatriculaBusiness(this._unitOfWork))
                {
                    matriculaResponseDto = matriculaBusiness.GetByMatricula(
                        demonstrativoPagamentoResult.Matricula);
                }

                //  Se não existir o registro da Matrícula, adiciona.
                if (matriculaResponseDto is null)
                {
                    var matriculaRequestDto = new MatriculaRequestDto
                    {
                        GuidColaborador = pessoaFisicaResponseDto.Guid,
                        GuidEmpregador = pessoaJuridicaResponseDto.Guid,
                        Agencia = demonstrativoPagamentoResult.Agencia,
                        Banco = demonstrativoPagamentoResult.Banco,
                        CargaHoraria = this._cargaHorariaDefault,
                        Conta = demonstrativoPagamentoResult.Conta,
                        DataAdmissao = Convert.ToDateTime(
                            demonstrativoPagamentoResult.DataAdmissao),
                        DescricaoCargo = demonstrativoPagamentoResult.DescricaoCargo,
                        DescricaoSetor = demonstrativoPagamentoResult.DescricaoSetor,
                        Matricula = demonstrativoPagamentoResult.Matricula,
                        SalarioNominal = Convert.ToDecimal(
                            demonstrativoPagamentoResult.SalarioNominal),
                    };

                    using (var matriculaBusiness = new MatriculaBusiness(
                        this._unitOfWork))
                    {
                        matriculaResponseDto = matriculaBusiness.SaveData(
                            matriculaRequestDto);
                    }
                }

                // Verifica se existe o registro do Usuário.
                string username = string.Empty;

                string password = matriculaResponseDto.DataAdmissao.ToString("yyyyMMdd");

                var usuariosResponseDto = default(
                    IEnumerable<UsuarioResponseDto>);

                using (var usuarioBusiness = new UsuarioBusiness(
                    this._unitOfWork))
                {
                    var firstName = Common.GetFirstName(
                        pessoaFisicaResponseDto.Nome);

                    var lastName = Common.GetLastName(
                        pessoaFisicaResponseDto.Nome);

                    username = string.Concat(
                        firstName.ToLower(),
                        '.',
                        lastName.ToLower());

                    usuariosResponseDto = usuarioBusiness.GetByUsername(
                        username);
                }

                //  Se não existir o registro do Usuário, deve incluir o registro.
                if (usuariosResponseDto?.Count() == 0)
                {
                    var usuarioRequestCreateDto = new UsuarioRequestCreateDto
                    {
                        GuidColaborador = pessoaFisicaResponseDto.Guid,
                        Username = username,
                        ConfirmPassword = password,
                        Password = password,
                    };

                    using (var usuarioBusiness = new UsuarioBusiness(
                        this._unitOfWork))
                    {
                        var usuarioResponse = usuarioBusiness.SaveData(
                            usuarioRequestCreateDto);
                    }
                }

                //  Verifica se existe o registro do Demonstrativo de Pagamento da Matrícula.
                string competencia = string.Concat(
                    "01/",
                    demonstrativoPagamentoResult.Competencia);

                competencia = Convert.ToDateTime(
                    competencia).ToString("yyyyMMdd");

                //  Independente se existir um ou mais registros de Demonstrativos de Pagamento para a Matrícula, deve forçar a limpeza dos Itens dos Demonstrativos de Pagamento que possam estar vinculado à Matrícula dentro da Competência.
                this.Delete(
                    competencia,
                    (Guid)matriculaResponseDto.Guid);

                IEnumerable<MatriculaDemonstrativoPagamentoResponseDto> matriculasDemonstrativosPagamentoResponseDto = this.Get(
                    competencia,
                    demonstrativoPagamentoResult.Matricula);

                //  Se não existir o registro do Demonstrativo de Pagamento da Matrícula, adiciona.
                if (matriculasDemonstrativosPagamentoResponseDto is null ||
                    matriculasDemonstrativosPagamentoResponseDto.Count() == 0)
                {
                    var matriculaDemonstrativoPagamentoRequestCreateDto = new MatriculaDemonstrativoPagamentoRequestCreateDto
                    {
                        GuidMatricula = matriculaResponseDto.Guid,
                        Competencia = competencia,
                    };

                    var newMdp = this.SaveData(
                        matriculaDemonstrativoPagamentoRequestCreateDto);

                    ((List<MatriculaDemonstrativoPagamentoResponseDto>)matriculasDemonstrativosPagamentoResponseDto).Add(
                        newMdp);
                }

                // Processa os Eventos.
                if (demonstrativoPagamentoResult?.Eventos.Count > 0)
                {
                    foreach (var evento in demonstrativoPagamentoResult.Eventos)
                    {
                        //  Verifica se existe o registro do Evento.
                        var eventoResponseDto = default(
                            EventoResponseDto);

                        using (var eventoBusiness = new EventoBusiness(
                            this._unitOfWork))
                        {
                            eventoResponseDto = eventoBusiness.Get(
                                Convert.ToInt32(
                                    evento.Codigo));
                        }

                        //  Se não existir o registro do Evento, adiciona.
                        if (eventoResponseDto is null)
                        {
                            var eventoRequestDto = new EventoRequestDto
                            {
                                Id = Convert.ToInt32(
                                    evento.Codigo),
                                Descricao = evento.Descricao,
                                Tipo = evento.Tipo,
                            };

                            using (var eventoBusiness = new EventoBusiness(
                                this._unitOfWork))
                            {
                                eventoResponseDto = eventoBusiness.SaveData(
                                    eventoRequestDto);
                            }
                        }

                        // Processa os Vínculos dos Eventos.
                        this.processRecordMDPEvento(
                            (Guid)matriculasDemonstrativosPagamentoResponseDto.FirstOrDefault().Guid,
                            Convert.ToInt32(
                                eventoResponseDto.Id),
                            !string.IsNullOrEmpty(
                                evento.Referencia) ? Convert.ToDecimal(
                                    evento.Referencia) : default(decimal?),
                            Convert.ToDecimal(
                                    evento.Valor));
                    }
                }

                // Processa os Vínculos dos Totalizadores.
                decimal baseFgts = decimal.Zero;
                decimal valorFgts = decimal.Zero;
                decimal totalVencimentos = decimal.Zero;
                decimal totalDescontos = decimal.Zero;
                decimal baseIrrf = decimal.Zero;
                decimal baseInss = decimal.Zero;
                decimal totalLiquido = decimal.Zero;

                if (!string.IsNullOrEmpty(demonstrativoPagamentoResult.BaseFgts))
                    baseFgts = Convert.ToDecimal(
                        demonstrativoPagamentoResult.BaseFgts);

                if (!string.IsNullOrEmpty(demonstrativoPagamentoResult.ValorFgts))
                    valorFgts = Convert.ToDecimal(
                        demonstrativoPagamentoResult.ValorFgts);

                if (!string.IsNullOrEmpty(demonstrativoPagamentoResult.TotalVencimentos))
                    totalVencimentos = Convert.ToDecimal(
                        demonstrativoPagamentoResult.TotalVencimentos);

                if (!string.IsNullOrEmpty(demonstrativoPagamentoResult.TotalDescontos))
                    totalDescontos = Convert.ToDecimal(
                        demonstrativoPagamentoResult.TotalDescontos);

                if (!string.IsNullOrEmpty(demonstrativoPagamentoResult.BaseIrrf))
                    baseIrrf = Convert.ToDecimal(
                        demonstrativoPagamentoResult.BaseIrrf);

                if (!string.IsNullOrEmpty(demonstrativoPagamentoResult.BaseInss))
                    baseInss = Convert.ToDecimal(
                        demonstrativoPagamentoResult.BaseInss);

                if (!string.IsNullOrEmpty(demonstrativoPagamentoResult.TotalLiquido))
                    totalLiquido = Convert.ToDecimal(
                        demonstrativoPagamentoResult.TotalLiquido);

                //  Processa a Base Fgts.
                this.processRecordMDPTotalizador(
                    (Guid)matriculasDemonstrativosPagamentoResponseDto.FirstOrDefault().Guid,
                    this._idBaseFgts,
                    baseFgts);

                //  Processa o Valor Fgts.
                this.processRecordMDPTotalizador(
                    (Guid)matriculasDemonstrativosPagamentoResponseDto.FirstOrDefault().Guid,
                    this._idValorFgts,
                    valorFgts);

                //  Processa o Total de Vencimentos.
                this.processRecordMDPTotalizador(
                    (Guid)matriculasDemonstrativosPagamentoResponseDto.FirstOrDefault().Guid,
                    this._idTotalVencimentos,
                    totalVencimentos);

                //  Processa o Total de Descontos.
                this.processRecordMDPTotalizador(
                    (Guid)matriculasDemonstrativosPagamentoResponseDto.FirstOrDefault().Guid,
                    this._idTotalDescontos,
                    totalDescontos);

                //  Processa a Base Irrf.
                this.processRecordMDPTotalizador(
                    (Guid)matriculasDemonstrativosPagamentoResponseDto.FirstOrDefault().Guid,
                    this._idBaseIrrf,
                    baseIrrf);

                //  Processa a Base Inss.
                this.processRecordMDPTotalizador(
                    (Guid)matriculasDemonstrativosPagamentoResponseDto.FirstOrDefault().Guid,
                    this._idBaseInss,
                    baseInss);

                //  Processa o Total Líquido.
                this.processRecordMDPTotalizador(
                    (Guid)matriculasDemonstrativosPagamentoResponseDto.FirstOrDefault().Guid,
                    this._idTotalLiquido,
                    totalLiquido);

                connection.CommitTransaction();

                return matriculasDemonstrativosPagamentoResponseDto.FirstOrDefault();
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
        /// <param name="createDto"></param>
        /// <param name="updateDto"></param>
        /// <returns></returns>
        public MatriculaDemonstrativoPagamentoResponseDto SaveData(MatriculaDemonstrativoPagamentoRequestCreateDto? createDto = null, MatriculaDemonstrativoPagamentoRequestUpdateDto? updateDto = null)
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
                    MatriculaDemonstrativoPagamentoEntity);

                connection.BeginTransaction();

                if (updateDto != null)
                {
                    entity = this._mapper.Map<MatriculaDemonstrativoPagamentoEntity>(
                        updateDto);

                    entity = connection.RepositoriesUniPayCheck.MatriculaDemonstrativoPagamentoRepository.Update(
                        entity.Guid,
                        entity);
                }
                else if (createDto != null)
                {
                    entity = this._mapper.Map<MatriculaDemonstrativoPagamentoEntity>(
                        createDto);

                    entity = connection.RepositoriesUniPayCheck.MatriculaDemonstrativoPagamentoRepository.Create(
                        entity);
                }

                connection.CommitTransaction();

                return this._mapper.Map<MatriculaDemonstrativoPagamentoResponseDto>(
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
        /// <param name="guidMatriculaDemonstrativoPagamento"></param>
        /// <param name="idEvento"></param>
        /// <param name="referencia"></param>
        /// <param name="valor"></param>
        private void processRecordMDPEvento(Guid guidMatriculaDemonstrativoPagamento, int idEvento, decimal? referencia, decimal valor)
        {
            try
            {
                //  Verifica se existe o registro do vínculo do Demonstrativo de Pagamento x Evento.
                var matriculaDemonstrativoPagamentoEventoResponseDto = default(
                    MatriculaDemonstrativoPagamentoEventoResponseDto);

                using (var matriculaDemonstrativoPagamentoEventoBusiness = new MatriculaDemonstrativoPagamentoEventoBusiness(
                    this._unitOfWork))
                {
                    matriculaDemonstrativoPagamentoEventoResponseDto = matriculaDemonstrativoPagamentoEventoBusiness.GetByGuidMatriculaDemonstrativoPagamentoAndIdEvento(
                        guidMatriculaDemonstrativoPagamento,
                        idEvento);

                    //  Se não existir o registro do do vínculo do Demonstrativo de Pagamento x Evento, adiciona.
                    var matriculaDemonstrativoPagamentoEventoRequestDto = default(
                        MatriculaDemonstrativoPagamentoEventoRequestDto);

                    if (matriculaDemonstrativoPagamentoEventoResponseDto is null)
                    {
                        matriculaDemonstrativoPagamentoEventoRequestDto = new MatriculaDemonstrativoPagamentoEventoRequestDto
                        {
                            GuidMatriculaDemonstrativoPagamento = guidMatriculaDemonstrativoPagamento,
                            IdEvento = idEvento,
                        };
                    }
                    else
                    {
                        matriculaDemonstrativoPagamentoEventoRequestDto = new MatriculaDemonstrativoPagamentoEventoRequestDto
                        {
                            Guid = matriculaDemonstrativoPagamentoEventoResponseDto.Guid,
                            GuidMatriculaDemonstrativoPagamento = matriculaDemonstrativoPagamentoEventoResponseDto.GuidMatriculaDemonstrativoPagamento,
                            IdEvento = matriculaDemonstrativoPagamentoEventoResponseDto.IdEvento,
                        };
                    }

                    matriculaDemonstrativoPagamentoEventoRequestDto.Referencia = referencia;
                    matriculaDemonstrativoPagamentoEventoRequestDto.Valor = valor;

                    matriculaDemonstrativoPagamentoEventoBusiness.SaveData(
                        matriculaDemonstrativoPagamentoEventoRequestDto);
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
        /// <param name="guidMatriculaDemonstrativoPagamento"></param>
        /// <param name="idTotalizador"></param>
        /// <param name="valor"></param>
        private void processRecordMDPTotalizador(Guid guidMatriculaDemonstrativoPagamento, int idTotalizador, decimal valor)
        {
            try
            {
                //  Verifica se existe o registro do vínculo do Demonstrativo de Pagamento x Totalizador.
                var matriculaDemonstrativoPagamentoTotalizadorResponseDto = default(
                    MatriculaDemonstrativoPagamentoTotalizadorResponseDto);

                using (var matriculaDemonstrativoPagamentoTotalizadorBusiness = new MatriculaDemonstrativoPagamentoTotalizadorBusiness(
                    this._unitOfWork))
                {
                    matriculaDemonstrativoPagamentoTotalizadorResponseDto = matriculaDemonstrativoPagamentoTotalizadorBusiness.GetByGuidMatriculaDemonstrativoPagamentoAndIdTotalizador(
                        guidMatriculaDemonstrativoPagamento,
                        idTotalizador);

                    //  Se não existir o registro do do vínculo do Demonstrativo de Pagamento x Totalizador, adiciona.
                    var matriculaDemonstrativoPagamentoTotalizadorRequestDto = default(
                        MatriculaDemonstrativoPagamentoTotalizadorRequestDto);

                    if (matriculaDemonstrativoPagamentoTotalizadorResponseDto is null)
                    {
                        matriculaDemonstrativoPagamentoTotalizadorRequestDto = new MatriculaDemonstrativoPagamentoTotalizadorRequestDto
                        {
                            GuidMatriculaDemonstrativoPagamento = guidMatriculaDemonstrativoPagamento,
                            IdTotalizador = idTotalizador,
                        };
                    }

                    matriculaDemonstrativoPagamentoTotalizadorRequestDto.Valor = valor;

                    matriculaDemonstrativoPagamentoTotalizadorBusiness.SaveData(
                        matriculaDemonstrativoPagamentoTotalizadorRequestDto);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}