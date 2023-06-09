namespace ARVTech.DataAccess.Business.UniPayCheck
{
    using System;
    using ARVTech.DataAccess.DTOs.UniPayCheck;
    using ARVTech.DataAccess.Entities.UniPayCheck;
    using ARVTech.DataAccess.UnitOfWork.Interfaces;
    using ARVTech.Transmission.Engine.UniPayCheck.Results;
    using AutoMapper;

    public class MatriculaDemonstrativoPagamentoBusiness : BaseBusiness
    {
        public MatriculaDemonstrativoPagamentoBusiness(IUnitOfWork unitOfWork) :
            base(unitOfWork)
        {
            this._unitOfWork = unitOfWork;

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MatriculaDemonstrativoPagamentoDto, MatriculaDemonstrativoPagamentoEntity>().ReverseMap();
                cfg.CreateMap<MatriculaDto, MatriculaEntity>().ReverseMap();
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
        public void DeleteByCompetenciaAndGuidMatricula(string competencia, Guid guidMatricula)
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
        public MatriculaDemonstrativoPagamentoDto GetByCompetenciaAndMatricula(string competencia, string matricula)
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
                var pessoaFisicaDto = default(PessoaFisicaDto);

                using (var pessoaFisicaBusiness = new PessoaFisicaBusiness(this._unitOfWork))
                {
                    //pessoaFisicaDto = pessoaFisicaBusiness.GetByNomeNumeroCtpsSerieCtpsAndUfCtps(
                    //    demonstrativoPagamentoResult.Nome,
                    //    demonstrativoPagamentoResult.NumeroCtps,
                    //    demonstrativoPagamentoResult.SerieCtps,
                    //    demonstrativoPagamentoResult.UfCtps);

                    pessoaFisicaDto = pessoaFisicaBusiness.GetByNome(
                        demonstrativoPagamentoResult.Nome);
                }

                //  Se não existir o registro do Colaborador, adiciona.
                if (pessoaFisicaDto is null)
                {
                    pessoaFisicaDto = new PessoaFisicaDto
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
                        pessoaFisicaDto = pessoaFisicaBusiness.SaveData(
                            pessoaFisicaDto);
                    }
                }

                //  Verifica se existe o registro do Empregador.
                var pessoaJuridicaDto = default(PessoaJuridicaDto);

                using (var pessoaJuridicaBusiness = new PessoaJuridicaBusiness(this._unitOfWork))
                {
                    pessoaJuridicaDto = pessoaJuridicaBusiness.GetByRazaoSocial(
                        demonstrativoPagamentoResult.RazaoSocial);
                }

                //  Se não existir o registro do Empregador, adiciona.
                if (pessoaJuridicaDto is null)
                {
                    pessoaJuridicaDto = new PessoaJuridicaDto
                    {
                        Cnpj = demonstrativoPagamentoResult.Cnpj,
                        RazaoSocial = demonstrativoPagamentoResult.RazaoSocial,
                        Pessoa = new PessoaDto()
                        {
                            Cidade = "ESTEIO",
                            Endereco = "ENDERECO",
                            Uf = "RS",
                        },
                    };

                    using (var pessoaJuridicaBusiness = new PessoaJuridicaBusiness(this._unitOfWork))
                    {
                        pessoaJuridicaDto = pessoaJuridicaBusiness.SaveData(
                            pessoaJuridicaDto);
                    }
                }

                //  Verifica se existe o registro da Matrícula.
                var matriculaDto = default(MatriculaDto);

                using (var matriculaBusiness = new MatriculaBusiness(this._unitOfWork))
                {
                    matriculaDto = matriculaBusiness.GetByMatricula(
                        demonstrativoPagamentoResult.Matricula);
                }

                //  Se não existir o registro da Matrícula, adiciona.
                if (matriculaDto is null)
                {
                    matriculaDto = new MatriculaDto
                    {
                        GuidColaborador = pessoaFisicaDto.Guid,
                        GuidEmpregador = pessoaJuridicaDto.Guid,
                        DataAdmissao = Convert.ToDateTime(
                            demonstrativoPagamentoResult.DataAdmissao),
                        Matricula = demonstrativoPagamentoResult.Matricula,
                    };

                    using (var matriculaBusiness = new MatriculaBusiness(this._unitOfWork))
                    {
                        matriculaDto = matriculaBusiness.SaveData(
                            matriculaDto);
                    }
                }

                string competencia = string.Concat("01/", demonstrativoPagamentoResult.Competencia);
                competencia = Convert.ToDateTime(competencia).ToString("yyyyMMdd");

                //  Verifica se existe o registro do Demonstrativo de Pagamento da Matrícula.
                var matriculaDemonstrativoPagamentoDto = default(MatriculaDemonstrativoPagamentoDto);

                using (var matriculaDemonstrativoPagamentoBusiness = new MatriculaDemonstrativoPagamentoBusiness(
                    this._unitOfWork))
                {
                    //  Independente se existir um ou mais registros de Demonstrativos de Pagamento para a Matrícula, deve forçar a limpeza dos Itens dos Demonstrativos de Pagamento que possam estar vinculado à Matrícula dentro da Competência.
                    matriculaDemonstrativoPagamentoBusiness.DeleteByCompetenciaAndGuidMatricula(
                        competencia,
                        (Guid)matriculaDto.Guid);

                    matriculaDemonstrativoPagamentoDto = matriculaDemonstrativoPagamentoBusiness.GetByCompetenciaAndMatricula(
                        competencia,
                        demonstrativoPagamentoResult.Matricula);

                    //  Se não existir o registro do Demonstrativo de Pagamento da Matrícula, adiciona.
                    if (matriculaDemonstrativoPagamentoDto is null)
                    {
                        matriculaDemonstrativoPagamentoDto = new MatriculaDemonstrativoPagamentoDto
                        {
                            GuidMatricula = matriculaDto.Guid,
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
                                eventoDto = eventoBusiness.GetByCodigo(
                                    Convert.ToInt32(
                                        evento.Codigo));
                            }

                            //  Se não existir o registro do Evento, adiciona.
                            if (eventoDto is null)
                            {
                                eventoDto = new EventoDto
                                {
                                    Codigo = Convert.ToInt32(
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

                            //  Verifica se existe o registro do vínculo do Demonstrativo de Pagamento x Evento.
                            var matriculaDemonstrativoPagamentoEventoDto = default(
                                MatriculaDemonstrativoPagamentoEventoDto);

                            using (var matriculaDemonstrativoPagamentoEventoBusiness = new MatriculaDemonstrativoPagamentoEventoBusiness(
                                this._unitOfWork))
                            {
                                matriculaDemonstrativoPagamentoEventoDto = matriculaDemonstrativoPagamentoEventoBusiness.GetByGuidMatriculaDemonstrativoPagamentoAndIdEvento(
                                    (Guid)matriculaDemonstrativoPagamentoDto.Guid,
                                    Convert.ToInt32(
                                        eventoDto.Id));
                            }

                            //  Se não existir o registro do do vínculo do Demonstrativo de Pagamento x Evento, adiciona.
                            if (matriculaDemonstrativoPagamentoEventoDto is null)
                            {
                                matriculaDemonstrativoPagamentoEventoDto = new MatriculaDemonstrativoPagamentoEventoDto
                                {
                                    GuidMatriculaDemonstrativoPagamento = (Guid)matriculaDemonstrativoPagamentoDto.Guid,
                                    IdEvento = Convert.ToInt32(
                                        eventoDto.Id),
                                    Referencia = !string.IsNullOrEmpty(evento.Referencia) ? 
                                        Convert.ToDecimal(
                                            evento.Referencia) : 
                                        default(decimal?),
                                    Valor = Convert.ToDecimal(
                                        evento.Valor),
                                };
                            }

                            using (var matriculaDemonstrativoPagamentoEventoBusiness = new MatriculaDemonstrativoPagamentoEventoBusiness(
                                this._unitOfWork))
                            {
                                matriculaDemonstrativoPagamentoEventoBusiness.SaveData(
                                    matriculaDemonstrativoPagamentoEventoDto);
                            }
                        }
                    }
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
    }
}