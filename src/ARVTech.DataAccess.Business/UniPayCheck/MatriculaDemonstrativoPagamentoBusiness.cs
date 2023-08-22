namespace ARVTech.DataAccess.Business.UniPayCheck
{
    using System;
    using ARVTech.DataAccess.DTOs.UniPayCheck;
    using ARVTech.DataAccess.Core.Entities.UniPayCheck;
    using ARVTech.DataAccess.UnitOfWork.Interfaces;
    using ARVTech.Transmission.Engine.UniPayCheck.Results;
    using AutoMapper;
    using ARVTech.Shared;

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

        public MatriculaDemonstrativoPagamentoBusiness(IUnitOfWork unitOfWork) :
            base(unitOfWork)
        {
            this._unitOfWork = unitOfWork;

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MatriculaDemonstrativoPagamentoDto, MatriculaDemonstrativoPagamentoEntity>().ReverseMap();
                cfg.CreateMap<MatriculaDemonstrativoPagamentoResponse, MatriculaDemonstrativoPagamentoEntity>().ReverseMap();
                cfg.CreateMap<MatriculaDto, MatriculaEntity>().ReverseMap();
                cfg.CreateMap<MatriculaResponse, MatriculaEntity>().ReverseMap();
                cfg.CreateMap<PessoaFisicaDto, PessoaFisicaEntity>().ReverseMap();
                cfg.CreateMap<PessoaFisicaResponse, PessoaFisicaEntity>().ReverseMap();
                cfg.CreateMap<PessoaJuridicaDto, PessoaJuridicaEntity>().ReverseMap();
                cfg.CreateMap<PessoaJuridicaResponse, PessoaJuridicaEntity>().ReverseMap();
                cfg.CreateMap<PessoaDto, PessoaEntity>().ReverseMap();
                cfg.CreateMap<PessoaResponse, PessoaEntity>().ReverseMap();
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
        public MatriculaDemonstrativoPagamentoDto Get(Guid guid)
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

                    return this._mapper.Map<MatriculaDemonstrativoPagamentoDto>(
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
        public MatriculaDemonstrativoPagamentoDto Get(string competencia, string matricula)
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
                    var entity = connection.RepositoriesUniPayCheck.MatriculaDemonstrativoPagamentoRepository.GetByCompetenciaAndMatricula(
                        competencia,
                        matricula);

                    return this._mapper.Map<MatriculaDemonstrativoPagamentoDto>(entity);
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
        public MatriculaDemonstrativoPagamentoDto Import(DemonstrativoPagamentoResult demonstrativoPagamentoResult)
        {
            var connection = this._unitOfWork.Create();

            try
            {
                connection.BeginTransaction();

                //  Verifica se existe o registro do Colaborador.
                var pessoaFisicaResponse = default(PessoaFisicaResponse);

                using (var pessoaFisicaBusiness = new PessoaFisicaBusiness(this._unitOfWork))
                {
                    //pessoaFisicaDto = pessoaFisicaBusiness.GetByNomeNumeroCtpsSerieCtpsAndUfCtps(
                    //    demonstrativoPagamentoResult.Nome,
                    //    demonstrativoPagamentoResult.NumeroCtps,
                    //    demonstrativoPagamentoResult.SerieCtps,
                    //    demonstrativoPagamentoResult.UfCtps);

                    pessoaFisicaResponse = pessoaFisicaBusiness.GetByNome(
                        demonstrativoPagamentoResult.Nome);
                }

                //  Se não existir o registro do Colaborador, deve incluir o registro.
                if (pessoaFisicaResponse is null)
                {
                    var pessoaFisicaDto = new PessoaFisicaDto
                    {
                        Nome = demonstrativoPagamentoResult.Nome,
                        NumeroCtps = demonstrativoPagamentoResult.NumeroCtps,
                        SerieCtps = demonstrativoPagamentoResult.SerieCtps,
                        UfCtps = demonstrativoPagamentoResult.UfCtps,
                        Cpf = demonstrativoPagamentoResult.Cpf,
                        Pessoa = new PessoaDto()
                        {
                            Cidade = "ESTEIO",
                            Endereco = "ENDERECO",
                            Uf = "RS",
                        },
                    };

                    using (var pessoaFisicaBusiness = new PessoaFisicaBusiness(this._unitOfWork))
                    {
                        pessoaFisicaResponse = pessoaFisicaBusiness.SaveData(
                            pessoaFisicaDto);
                    }

                    //throw new Exception(
                    //    $"Colaborador {demonstrativoPagamentoResult.Nome} não encontrado na tabela de Pessoas Físicas. Por gentileza, cadastre com o Nome exibido e os demais campos chaves e obrigatórios.");
                }

                //  Verifica se existe o registro do Empregador.
                var pessoaJuridicaResponse = default(PessoaJuridicaResponse);

                using (var pessoaJuridicaBusiness = new PessoaJuridicaBusiness(this._unitOfWork))
                {
                    pessoaJuridicaResponse = pessoaJuridicaBusiness.GetByRazaoSocial(
                        demonstrativoPagamentoResult.RazaoSocial);
                }

                //  Se não existir o registro do Empregador, deve disparar uma exceção.
                if (pessoaJuridicaResponse is null)
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
                var matriculaResponse = default(MatriculaResponse);

                using (var matriculaBusiness = new MatriculaBusiness(this._unitOfWork))
                {
                    matriculaResponse = matriculaBusiness.GetByMatricula(
                        demonstrativoPagamentoResult.Matricula);
                }

                //  Se não existir o registro da Matrícula, adiciona.
                if (matriculaResponse is null)
                {
                    var matriculaDto = new MatriculaDto
                    {
                        GuidColaborador = pessoaFisicaResponse.Guid,
                        GuidEmpregador = pessoaJuridicaResponse.Guid,
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

                    using (var matriculaBusiness = new MatriculaBusiness(this._unitOfWork))
                    {
                        matriculaResponse = matriculaBusiness.SaveData(
                            matriculaDto);
                    }

                    //throw new Exception(
                    //    $"Matrícula {demonstrativoPagamentoResult.Matricula} não encontrada na tabela de Matrículas para o Colaborador {demonstrativoPagamentoResult.Nome} e Empregador {demonstrativoPagamentoResult.RazaoSocial}. Por gentileza, cadastre com o Matrícula exibida e com os demais campos chaves e obrigatórios.");
                }

                // Verifica se existe o registro do Usuário.
                string username = string.Empty;

                string password = matriculaResponse.DataAdmissao.ToString("yyyyMMdd");

                var usuariosResponse = default(IEnumerable<UsuarioResponse>);

                using (var usuarioBusiness = new UsuarioBusiness(
                    this._unitOfWork))
                {
                    var firstName = Common.GetFirstName(
                        pessoaFisicaResponse.Nome);

                    var lastName = Common.GetLastName(
                        pessoaFisicaResponse.Nome);

                    username = string.Concat(
                        firstName.ToLower(),
                        '.',
                        lastName.ToLower());

                    usuariosResponse = usuarioBusiness.GetByUsername(
                        username);
                }

                //  Se não existir o registro do Usuário, deve incluir o registro.
                if (usuariosResponse?.Count() == 0)
                {
                    var usuarioRequestCreateDto = new UsuarioRequestCreateDto
                    {
                        GuidColaborador = pessoaFisicaResponse.Guid,
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
                string competencia = string.Concat("01/", demonstrativoPagamentoResult.Competencia);

                competencia = Convert.ToDateTime(
                    competencia).ToString("yyyyMMdd");

                var matriculaDemonstrativoPagamentoDto = default(MatriculaDemonstrativoPagamentoDto);

                using (var matriculaDemonstrativoPagamentoBusiness = new MatriculaDemonstrativoPagamentoBusiness(
                    this._unitOfWork))
                {
                    //  Independente se existir um ou mais registros de Demonstrativos de Pagamento para a Matrícula, deve forçar a limpeza dos Itens dos Demonstrativos de Pagamento que possam estar vinculado à Matrícula dentro da Competência.
                    matriculaDemonstrativoPagamentoBusiness.Delete(
                        competencia,
                        (Guid)matriculaResponse.Guid);

                    matriculaDemonstrativoPagamentoDto = matriculaDemonstrativoPagamentoBusiness.Get(
                        competencia,
                        demonstrativoPagamentoResult.Matricula);

                    //  Se não existir o registro do Demonstrativo de Pagamento da Matrícula, adiciona.
                    if (matriculaDemonstrativoPagamentoDto is null)
                    {
                        matriculaDemonstrativoPagamentoDto = new MatriculaDemonstrativoPagamentoDto
                        {
                            GuidMatricula = matriculaResponse.Guid,
                            Competencia = competencia,
                        };

                        matriculaDemonstrativoPagamentoDto = matriculaDemonstrativoPagamentoBusiness.SaveData(
                            matriculaDemonstrativoPagamentoDto);
                    }

                    // Processa os Eventos.
                    if (demonstrativoPagamentoResult?.Eventos.Count > 0)
                    {
                        foreach (var evento in demonstrativoPagamentoResult.Eventos)
                        {
                            //  Verifica se existe o registro do Evento.
                            var eventoDto = default(
                                EventoDto);

                            using (var eventoBusiness = new EventoBusiness(
                                this._unitOfWork))
                            {
                                eventoDto = eventoBusiness.Get(
                                    Convert.ToInt32(
                                        evento.Codigo));
                            }

                            //  Se não existir o registro do Evento, adiciona.
                            if (eventoDto is null)
                            {
                                eventoDto = new EventoDto
                                {
                                    Id = Convert.ToInt32(
                                        evento.Codigo),
                                    Descricao = evento.Descricao,
                                    Tipo = evento.Tipo,
                                };

                                using (var eventoBusiness = new EventoBusiness(
                                    this._unitOfWork))
                                {
                                    eventoDto = eventoBusiness.SaveData(
                                        eventoDto);
                                }
                            }

                            // Processa os Vínculos dos Eventos.
                            this.processRecordMDPEvento(
                                (Guid)matriculaDemonstrativoPagamentoDto.Guid,
                                Convert.ToInt32(
                                    eventoDto.Id),
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
                    {
                        baseFgts = Convert.ToDecimal(
                            demonstrativoPagamentoResult.BaseFgts);
                    }

                    if (!string.IsNullOrEmpty(demonstrativoPagamentoResult.ValorFgts))
                    {
                        valorFgts = Convert.ToDecimal(
                            demonstrativoPagamentoResult.ValorFgts);
                    }

                    if (!string.IsNullOrEmpty(demonstrativoPagamentoResult.TotalVencimentos))
                    {
                        totalVencimentos = Convert.ToDecimal(
                            demonstrativoPagamentoResult.TotalVencimentos);
                    }

                    if (!string.IsNullOrEmpty(demonstrativoPagamentoResult.TotalDescontos))
                    {
                        totalDescontos = Convert.ToDecimal(
                            demonstrativoPagamentoResult.TotalDescontos);
                    }

                    if (!string.IsNullOrEmpty(demonstrativoPagamentoResult.BaseIrrf))
                    {
                        baseIrrf = Convert.ToDecimal(
                            demonstrativoPagamentoResult.BaseIrrf);
                    }

                    if (!string.IsNullOrEmpty(demonstrativoPagamentoResult.BaseInss))
                    {
                        baseInss = Convert.ToDecimal(
                            demonstrativoPagamentoResult.BaseInss);
                    }

                    if (!string.IsNullOrEmpty(demonstrativoPagamentoResult.TotalLiquido))
                    {
                        totalLiquido = Convert.ToDecimal(
                            demonstrativoPagamentoResult.TotalLiquido);
                    }

                    //  Processa a Base Fgts.
                    this.processRecordMDPTotalizador(
                        (Guid)matriculaDemonstrativoPagamentoDto.Guid,
                        this._idBaseFgts,
                        baseFgts);

                    //  Processa o Valor Fgts.
                    this.processRecordMDPTotalizador(
                        (Guid)matriculaDemonstrativoPagamentoDto.Guid,
                        this._idValorFgts,
                        valorFgts);

                    //  Processa o Total de Vencimentos.
                    this.processRecordMDPTotalizador(
                        (Guid)matriculaDemonstrativoPagamentoDto.Guid,
                        this._idTotalVencimentos,
                        totalVencimentos);

                    //  Processa o Total de Descontos.
                    this.processRecordMDPTotalizador(
                        (Guid)matriculaDemonstrativoPagamentoDto.Guid,
                        this._idTotalDescontos,
                        totalDescontos);

                    //  Processa a Base Irrf.
                    this.processRecordMDPTotalizador(
                        (Guid)matriculaDemonstrativoPagamentoDto.Guid,
                        this._idBaseIrrf,
                        baseIrrf);

                    //  Processa a Base Inss.
                    this.processRecordMDPTotalizador(
                        (Guid)matriculaDemonstrativoPagamentoDto.Guid,
                        this._idBaseInss,
                        baseInss);

                    //  Processa o Total Líquido.
                    this.processRecordMDPTotalizador(
                        (Guid)matriculaDemonstrativoPagamentoDto.Guid,
                        this._idTotalLiquido,
                        totalLiquido);
                }

                connection.CommitTransaction();

                return matriculaDemonstrativoPagamentoDto;
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
        public MatriculaDemonstrativoPagamentoDto SaveData(MatriculaDemonstrativoPagamentoDto dto)
        {
            var connection = this._unitOfWork.Create();

            try
            {
                var entity = this._mapper.Map<MatriculaDemonstrativoPagamentoEntity>(dto);

                connection.BeginTransaction();

                if (dto.Guid != null && dto.Guid != Guid.Empty)
                {
                    entity = connection.RepositoriesUniPayCheck.MatriculaDemonstrativoPagamentoRepository.Update(
                        entity);
                }
                else
                {
                    entity = connection.RepositoriesUniPayCheck.MatriculaDemonstrativoPagamentoRepository.Create(
                        entity);
                }

                connection.CommitTransaction();

                return this._mapper.Map<MatriculaDemonstrativoPagamentoDto>(
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
        /// <param name="guidMatriculaDemonstrativoPagamento"></param>
        /// <param name="idEvento"></param>
        /// <param name="referencia"></param>
        /// <param name="valor"></param>
        private void processRecordMDPEvento(Guid guidMatriculaDemonstrativoPagamento, int idEvento, decimal? referencia, decimal valor)
        {
            try
            {
                //  Verifica se existe o registro do vínculo do Demonstrativo de Pagamento x Evento.
                var matriculaDemonstrativoPagamentoEventoDto = default(
                    MatriculaDemonstrativoPagamentoEventoDto);

                using (var matriculaDemonstrativoPagamentoEventoBusiness = new MatriculaDemonstrativoPagamentoEventoBusiness(
                    this._unitOfWork))
                {
                    matriculaDemonstrativoPagamentoEventoDto = matriculaDemonstrativoPagamentoEventoBusiness.GetByGuidMatriculaDemonstrativoPagamentoAndIdEvento(
                        guidMatriculaDemonstrativoPagamento,
                        idEvento);

                    //  Se não existir o registro do do vínculo do Demonstrativo de Pagamento x Evento, adiciona.
                    if (matriculaDemonstrativoPagamentoEventoDto is null)
                    {
                        matriculaDemonstrativoPagamentoEventoDto = new MatriculaDemonstrativoPagamentoEventoDto
                        {
                            GuidMatriculaDemonstrativoPagamento = guidMatriculaDemonstrativoPagamento,
                            IdEvento = idEvento,
                        };
                    }

                    matriculaDemonstrativoPagamentoEventoDto.Referencia = referencia;
                    matriculaDemonstrativoPagamentoEventoDto.Valor = valor;

                    matriculaDemonstrativoPagamentoEventoBusiness.SaveData(
                        matriculaDemonstrativoPagamentoEventoDto);
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
                var matriculaDemonstrativoPagamentoTotalizadorDto = default(
                    MatriculaDemonstrativoPagamentoTotalizadorDto);

                using (var matriculaDemonstrativoPagamentoTotalizadorBusiness = new MatriculaDemonstrativoPagamentoTotalizadorBusiness(
                    this._unitOfWork))
                {
                    matriculaDemonstrativoPagamentoTotalizadorDto = matriculaDemonstrativoPagamentoTotalizadorBusiness.GetByGuidMatriculaDemonstrativoPagamentoAndIdTotalizador(
                        guidMatriculaDemonstrativoPagamento,
                        idTotalizador);

                    //  Se não existir o registro do do vínculo do Demonstrativo de Pagamento x Totalizador, adiciona.
                    if (matriculaDemonstrativoPagamentoTotalizadorDto is null)
                    {
                        matriculaDemonstrativoPagamentoTotalizadorDto = new MatriculaDemonstrativoPagamentoTotalizadorDto
                        {
                            GuidMatriculaDemonstrativoPagamento = guidMatriculaDemonstrativoPagamento,
                            IdTotalizador = idTotalizador,
                        };
                    }

                    matriculaDemonstrativoPagamentoTotalizadorDto.Valor = valor;

                    matriculaDemonstrativoPagamentoTotalizadorBusiness.SaveData(
                        matriculaDemonstrativoPagamentoTotalizadorDto);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}