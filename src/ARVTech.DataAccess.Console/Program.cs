namespace ARVTech.DataAccess.Console
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using ARVTech.DataAccess.Business.UniPayCheck;
    using ARVTech.DataAccess.Console.Enums;
    using ARVTech.DataAccess.DbManager;
    using ARVTech.DataAccess.DbManager.Enums;
    using ARVTech.Transmission.Engine.UniPayCheck;
    using Microsoft.Extensions.Configuration;

    public static class Program
    {
        private static IConfiguration _configuration;

        private readonly static Assembly _assembly = Assembly.GetExecutingAssembly();

        private readonly static FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(
            _assembly.Location);

        private readonly static string _productName = fvi.ProductName;
        private readonly static string _productVersion = fvi.ProductVersion;

        private readonly static string _arquivoLog = string.Format(
            CultureInfo.InvariantCulture,
            @"{0}\\Log{1}{2}.log",
            AppDomain.CurrentDomain.BaseDirectory,
            _assembly.GetName().Name?.Replace(
                ".Console",
                string.Empty),
            DateTime.Now.ToString("yyyyMMddHHmm"));

        public static void Main(string[] args)
        {
            try
            {
                WriteConsole(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "*** {0} [ Versão {1} ] ***",
                        _productName,
                        _productVersion),
                    bootstrapColor: BootstrapColorEnum.Primary);

                WriteConsole("Limpando Log",
                    newLinesBefore: 2,
                    bootstrapColor: BootstrapColorEnum.Dark);

                ApagarLog();

                WriteConsole(
                    "CARREGANDO as configurações de acesso ao ARVTech.DataAccess®...",
                    newLinesBefore: 2,
                    newLinesAfter: 1,
                    bootstrapColor: BootstrapColorEnum.Dark);

                GetOrCreateConfiguration();

                var singletonDbManager = new ContextDbManager(
                    DatabaseTypeEnum.SqlServer,
                    _configuration);

                using var pessoaJuridicaBusiness = new PessoaJuridicaBusiness(
                    singletonDbManager.UnitOfWork);

                var pessoasJuridicas = pessoaJuridicaBusiness.GetAll();

                foreach (var pessoaJuridica in pessoasJuridicas)
                {
                    WriteConsole(
                        $"PROCESSANDO os Demonstrativos de Pagamento do CNPJ {pessoaJuridica.Cnpj}",
                        newLinesBefore: 1,
                        newLinesAfter: 2,
                        bootstrapColor: BootstrapColorEnum.Dark);

                    using var matriculaDemonstrativoPagamentoBusiness = new MatriculaDemonstrativoPagamentoBusiness(
                        singletonDbManager.UnitOfWork);

                    var pathDirectoryOrFileNameSource =
                        $@"E:\SistemasWEB\ARVTech\ARVTech.Transmission\src\ARVTech.Transmission.Console\bin\{pessoaJuridica.Cnpj}";

                    if (!Directory.Exists(pathDirectoryOrFileNameSource) &&
                        !File.Exists(pathDirectoryOrFileNameSource))
                    {
                        continue;
                    }

                    var transmissionUniPayCheck = new TransmissionUniPayCheck(
                        pathDirectoryOrFileNameSource);

                    var demonstrativosPagamento = transmissionUniPayCheck.GetDemonstrativosPagamento();

                    foreach (var dp in demonstrativosPagamento)
                    {
                        WriteConsole(
                            $"Competência: {dp.Competencia}; Matrícula: {dp.Matricula}; Nome: {dp.Nome}. ",
                            bootstrapColor: BootstrapColorEnum.Dark);

                        try
                        {
                            matriculaDemonstrativoPagamentoBusiness.Import(
                                dp);

                            WriteConsole(
                                "OK",
                                newLinesAfter: 1,
                                bootstrapColor: BootstrapColorEnum.Success,
                                showDate: false);
                        }
                        catch (Exception ex)
                        {
                            WriteConsole(
                                string.Concat(
                                    ex.Message,
                                    " ",
                                    ex.InnerException?.InnerException),
                                newLinesAfter: 1,
                                newLinesBefore: 1,
                                bootstrapColor: BootstrapColorEnum.Danger);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteConsole(
                    string.Concat(
                        ex.Message,
                        " ",
                        ex.InnerException?.InnerException),
                    newLinesBefore: 1,
                    bootstrapColor: BootstrapColorEnum.Danger);
            }
            finally
            {
                WriteConsole(
                    "*** Término da execução do ARVTech.DataAccess®. ***",
                    newLinesBefore: 1);
            }
        }

        //public static void Main(string[] args)
        //{
        //    try
        //    {
        //        WriteConsole(
        //            string.Format(
        //                CultureInfo.InvariantCulture,
        //                "*** {0} [ Versão {1} ] ***",
        //                _productName,
        //                _productVersion),
        //            bootstrapColor: BootstrapColorEnum.Primary);

        //        WriteConsole("Limpando Log",
        //            newLineBefore: true,
        //            newLineAfter: true,
        //            bootstrapColor: BootstrapColorEnum.Dark);

        //        ApagarLog();

        //        WriteConsole(
        //            "CARREGANDO as configurações de acesso ao ARVTech.DataAccess®...",
        //            bootstrapColor: BootstrapColorEnum.Dark);

        //        GetOrCreateConfiguration();

        //        var singletonDbManager = new ContextDbManager(
        //            DatabaseTypeEnum.SqlServer,
        //            _configuration);

        //        //var dtoAssociacao = default(AssociacaoDto);

        //        //var dtoConta = default(ContaDto);

        //        //using (var associacaoBusiness = new AssociacaoBusiness(
        //        //    singletonDbManager.UnitOfWork))
        //        //{
        //        //    dtoAssociacao = associacaoBusiness.Get(
        //        //        2);
        //        //}

        //        //using (var contaBusiness = new ContaBusiness(
        //        //    singletonDbManager.UnitOfWork))
        //        //{
        //        //    var guidConta = new Guid(
        //        //        "98D49545-1543-4B09-8FB2-910EE78E923A");

        //        //    dtoConta = contaBusiness.Get(
        //        //        guidConta);
        //        //}

        //        using (var animalBusiness = new AnimalBusiness(
        //            singletonDbManager.UnitOfWork))
        //        using (var cabanhaBusiness = new CabanhaBusiness(
        //            singletonDbManager.UnitOfWork))
        //        using (var usuarioBusiness = new UsuarioBusiness(
        //            singletonDbManager.UnitOfWork))
        //        {
        //            //var guidConta = new Guid(
        //            //    "98D49545-1543-4B09-8FB2-910EE78E923A");

        //            //var guidUsuario = new Guid(
        //            //    "BE7D302E-9536-42C3-B54D-54B01A4A7E3A");

        //            //var dto = cabanhaBusiness.GetAllWithPermission(
        //            //    guidConta,
        //            //    guidUsuario);

        //            //var dtoInsert = cabanhaBusiness.SaveData(new CabanhaDto
        //            //{
        //            //    Bairro = "Bairro",
        //            //    Cnpj = "84244283000107",
        //            //    Cidade = "Cidade",
        //            //    Endereco = "Endereço",
        //            //    IdAssociacao = (int)dtoAssociacao.Id,
        //            //    GuidConta = (Guid)dtoConta.Guid,
        //            //    NomeFantasia = "Nome Fantasia",
        //            //    RazaoSocial = "Razão Social",
        //            //    Uf = "Uf",

        //            //});

        //            var usuarioDto = usuarioBusiness.GetAll();

        //            var animaisDto = animalBusiness.GetAll();

        //            //var animalDto = animalBusiness.Get(Guid.Parse("016754AF-C8B3-4B58-8108-AE643F92A5D1"));

        //            //var dtoInsert = associacaoBusiness.SaveData(
        //            //    new AssociacaoDto
        //            //    {
        //            //        DescricaoRegistro = "A",
        //            //        Observacoes = "B",
        //            //        RazaoSocial = "C",
        //            //        Sigla = "D",
        //            //    });

        //            //tipoBusiness.Delete(51009);
        //        }

        //        //using (var tipoBusiness = new TipoBusiness(
        //        //    singletonDbManager.UnitOfWork))
        //        //{
        //        //    var tiposDto = tipoBusiness.GetAll();

        //        //    var tipoDto = tipoBusiness.Get(
        //        //        1);

        //        //    //var dtoInsert = associacaoBusiness.SaveData(
        //        //    //    new AssociacaoDto
        //        //    //    {
        //        //    //        DescricaoRegistro = "A",
        //        //    //        Observacoes = "B",
        //        //    //        RazaoSocial = "C",
        //        //    //        Sigla = "D",
        //        //    //    });

        //        //    tipoBusiness.Delete(51009);
        //        //}


        //        //using (var associacaoBusiness = new AssociacaoBusiness(
        //        //    singletonDbManager.UnitOfWork))
        //        //{
        //        //    var associacoesDto = associacaoBusiness.GetAll();

        //        //    var associacaoDto = associacaoBusiness.Get(
        //        //        51010);

        //        //    associacaoDto.DescricaoRegistro = string.Concat("A", associacaoDto.DescricaoRegistro, "A");
        //        //    associacaoDto.Observacoes = string.Concat("B", associacaoDto.Observacoes, "B");
        //        //    associacaoDto.RazaoSocial = string.Concat("C", associacaoDto.RazaoSocial, "C");
        //        //    associacaoDto.Sigla = string.Concat("D", associacaoDto.Sigla, "D");

        //        //    //var dtoInsert = associacaoBusiness.SaveData(
        //        //    //    new AssociacaoDto
        //        //    //    {
        //        //    //        DescricaoRegistro = "A",
        //        //    //        Observacoes = "B",
        //        //    //        RazaoSocial = "C",
        //        //    //        Sigla = "D",
        //        //    //    });

        //        //    var dtoUpdate = associacaoBusiness.SaveData(
        //        //        new AssociacaoDto
        //        //        {
        //        //            Id = 51010,
        //        //            DescricaoRegistro = "AA",
        //        //            Observacoes = "BB",
        //        //            RazaoSocial = "CC",
        //        //            Sigla = "DD",
        //        //        });

        //        //    associacaoBusiness.Delete(51009);
        //        //}

        //        //using (var pelagemBusiness = new PelagemBusiness(
        //        //    singletonDbManager.UnitOfWork))
        //        //{
        //        //    var dto = new PelagemDto
        //        //    {
        //        //        Descricao = "Descrição Teste",
        //        //        Observacoes = "Observações Teste",
        //        //    };

        //        //    var pelagemDto = pelagemBusiness.SaveData(
        //        //        dto);

        //        //    var pelagens = pelagemBusiness.GetAll();

        //        //    pelagemBusiness.Delete(
        //        //        (int)pelagemDto.Id);
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteConsole(
        //            string.Concat(
        //                ex.Message,
        //                " ",
        //                ex.InnerException?.InnerException),
        //            newLineBefore: true,
        //            bootstrapColor: BootstrapColorEnum.Danger);
        //    }
        //    finally
        //    {
        //        WriteConsole(
        //            "*** Término da execução do ARVTech.DataAccess®. ***",
        //            newLineBefore: true);
        //    }

        //    //try
        //    //{
        //    //    WriteConsole(
        //    //        string.Format(
        //    //            CultureInfo.InvariantCulture,
        //    //            "*** {0} [ Versão {1} ] ***",
        //    //            _productName,
        //    //            _productVersion),
        //    //        bootstrapColor: BootstrapColorEnum.Primary);

        //    //    WriteConsole("Limpando Log",
        //    //        newLineBefore: true,
        //    //        newLineAfter: true,
        //    //        bootstrapColor: BootstrapColorEnum.Dark);

        //    //    ApagarLog();

        //    //    WriteConsole(
        //    //        "CARREGANDO as configurações de acesso ao ARVTech.DataAccess®...",
        //    //        bootstrapColor: BootstrapColorEnum.Dark);

        //    //    GetOrCreateConfiguration();

        //    //    var singletonDbManagerSource = new ContextDbManager(
        //    //        DatabaseTypeEnum.Access,
        //    //        _configuration);

        //    //    var singletonDbManagerTarget = new ContextDbManager(
        //    //        DatabaseTypeEnum.SqlServer,
        //    //        _configuration);

        //    //    using (var produtoServiceSource = new Services.Parker.ProdutoService(
        //    //        singletonDbManagerSource.UnitOfWork))
        //    //    {
        //    //        using var produtoServiceTarget = new Services.Empresarius.ProdutoService(
        //    //            singletonDbManagerTarget.UnitOfWork);

        //    //        var produtosSource = produtoServiceSource.Listar();

        //    //        if (produtosSource != null &&
        //    //            produtosSource.Count() > 0)
        //    //        {
        //    //            long contador = 0;

        //    //            foreach (var ps in produtosSource)
        //    //            {
        //    //                contador++;

        //    //                string descricaoProduto = TreatStringWithAccent(
        //    //                        ps.LinhaProduto);

        //    //                WriteConsole($"{contador}: {ps.Item} - {descricaoProduto}", showDate: false);

        //    //                ProdutoModel pm = new ProdutoModel
        //    //                {
        //    //                    Descricao = descricaoProduto,
        //    //                };

        //    //                produtoServiceTarget.Salvar(pm);
        //    //            }
        //    //        }
        //    //    }

        //    //    WriteConsole("Configurações de acesso ao ARVTech.DataAccess carregadas com SUCESSO.",
        //    //        newLineAfter: true,
        //    //        bootstrapColor: BootstrapColorEnum.Dark);

        //    //    WriteConsole(string.Empty,
        //    //        showDate: false);
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    WriteConsole(
        //    //        string.Concat(
        //    //            ex.Message,
        //    //            " ",
        //    //            ex.InnerException?.InnerException),
        //    //        newLineBefore: true,
        //    //        bootstrapColor: BootstrapColorEnum.Danger);
        //    //}
        //    //finally
        //    //{
        //    //    WriteConsole(
        //    //        "*** Término da execução do ARVTech.DataAccess®. ***",
        //    //        newLineBefore: true);
        //    //}
        //}

        //public static void Main(string[] args)
        //{
        //    try
        //    {
        //        WriteConsole(
        //            string.Format(
        //                CultureInfo.InvariantCulture,
        //                "*** {0} [ Versão {1} ] ***",
        //                _productName,
        //                _productVersion),
        //            bootstrapColor: BootstrapColorEnum.Primary);

        //        WriteConsole("Limpando Log",
        //            newLineBefore: true,
        //            newLineAfter: true,
        //            bootstrapColor: BootstrapColorEnum.Dark);

        //        ApagarLog();

        //        WriteConsole(
        //            "CARREGANDO as configurações de acesso ao ARVTech.DataAccess®...",
        //            bootstrapColor: BootstrapColorEnum.Dark);

        //        GetOrCreateConfiguration();

        //        var singletonDbManager = new ContextDbManager(
        //            DatabaseTypeEnum.SqlServer,
        //            _configuration);

        //        using (var associacaoBusiness = new AssociacaoBusiness(
        //            singletonDbManager.UnitOfWork))
        //        {
        //            var associacaoDto = associacaoBusiness.Get(
        //                51010);

        //            associacaoDto.DescricaoRegistro = String.Concat("A", associacaoDto.DescricaoRegistro, "A");
        //            associacaoDto.Observacoes = String.Concat("B", associacaoDto.Observacoes, "B");
        //            associacaoDto.RazaoSocial = String.Concat("C", associacaoDto.RazaoSocial, "C");
        //            associacaoDto.Sigla = String.Concat("D", associacaoDto.Sigla, "D");

        //            var associacoesDto = associacaoBusiness.GetAll();

        //            //var dtoInsert = associacaoBusiness.SaveData(
        //            //    new AssociacaoDto
        //            //    {
        //            //        DescricaoRegistro = "A",
        //            //        Observacoes = "B",
        //            //        RazaoSocial = "C",
        //            //        Sigla = "D",
        //            //    });

        //            var dtoUpdate = associacaoBusiness.SaveData(
        //                new AssociacaoDto
        //                {
        //                    Id = 51010,
        //                    DescricaoRegistro = "AA",
        //                    Observacoes = "BB",
        //                    RazaoSocial = "CC",
        //                    Sigla = "DD",
        //                });

        //            associacaoBusiness.Delete(51009);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteConsole(
        //            string.Concat(
        //                ex.Message,
        //                " ",
        //                ex.InnerException?.InnerException),
        //            newLineBefore: true,
        //            bootstrapColor: BootstrapColorEnum.Danger);
        //    }
        //    finally
        //    {
        //        WriteConsole(
        //            "*** Término da execução do ARVTech.DataAccess®. ***",
        //            newLineBefore: true);
        //    }

        //    //try
        //    //{
        //    //    WriteConsole(
        //    //        string.Format(
        //    //            CultureInfo.InvariantCulture,
        //    //            "*** {0} [ Versão {1} ] ***",
        //    //            _productName,
        //    //            _productVersion),
        //    //        bootstrapColor: BootstrapColorEnum.Primary);

        //    //    WriteConsole("Limpando Log",
        //    //        newLineBefore: true,
        //    //        newLineAfter: true,
        //    //        bootstrapColor: BootstrapColorEnum.Dark);

        //    //    ApagarLog();

        //    //    WriteConsole(
        //    //        "CARREGANDO as configurações de acesso ao ARVTech.DataAccess®...",
        //    //        bootstrapColor: BootstrapColorEnum.Dark);

        //    //    GetOrCreateConfiguration();

        //    //    var singletonDbManagerSource = new ContextDbManager(
        //    //        DatabaseTypeEnum.Access,
        //    //        _configuration);

        //    //    var singletonDbManagerTarget = new ContextDbManager(
        //    //        DatabaseTypeEnum.SqlServer,
        //    //        _configuration);

        //    //    using (var produtoServiceSource = new Services.Parker.ProdutoService(
        //    //        singletonDbManagerSource.UnitOfWork))
        //    //    {
        //    //        using var produtoServiceTarget = new Services.Empresarius.ProdutoService(
        //    //            singletonDbManagerTarget.UnitOfWork);

        //    //        var produtosSource = produtoServiceSource.Listar();

        //    //        if (produtosSource != null &&
        //    //            produtosSource.Count() > 0)
        //    //        {
        //    //            long contador = 0;

        //    //            foreach (var ps in produtosSource)
        //    //            {
        //    //                contador++;

        //    //                string descricaoProduto = TreatStringWithAccent(
        //    //                        ps.LinhaProduto);

        //    //                WriteConsole($"{contador}: {ps.Item} - {descricaoProduto}", showDate: false);

        //    //                ProdutoModel pm = new ProdutoModel
        //    //                {
        //    //                    Descricao = descricaoProduto,
        //    //                };

        //    //                produtoServiceTarget.Salvar(pm);
        //    //            }
        //    //        }
        //    //    }

        //    //    WriteConsole("Configurações de acesso ao ARVTech.DataAccess carregadas com SUCESSO.",
        //    //        newLineAfter: true,
        //    //        bootstrapColor: BootstrapColorEnum.Dark);

        //    //    WriteConsole(string.Empty,
        //    //        showDate: false);
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    WriteConsole(
        //    //        string.Concat(
        //    //            ex.Message,
        //    //            " ",
        //    //            ex.InnerException?.InnerException),
        //    //        newLineBefore: true,
        //    //        bootstrapColor: BootstrapColorEnum.Danger);
        //    //}
        //    //finally
        //    //{
        //    //    WriteConsole(
        //    //        "*** Término da execução do ARVTech.DataAccess®. ***",
        //    //        newLineBefore: true);
        //    //}
        //}

        private static void GetOrCreateConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true);

            _configuration = builder.Build();

            if (_configuration == null)
                throw new Exception("[ERRO] Não foi possível carregar as configurações do Integrador.");
        }

        private static string TreatStringWithAccent(string value)
        {
            Encoding iso = Encoding.GetEncoding("ISO-8859-1");
            Encoding utf8 = Encoding.UTF8;

            byte[] utfBytes = utf8.GetBytes(value);

            byte[] isoBytes = Encoding.Convert(
                utf8,
                iso,
                utfBytes);

            return iso.GetString(isoBytes);
        }

        private static void ApagarLog()
        {
            DateTime dataBase = DateTime.Now.AddDays(-7);

            try
            {
                string[] files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "LOG*.log");

                foreach (string file in files)
                {
                    FileInfo fs = new FileInfo(file);

                    if (fs.LastWriteTime < dataBase)
                        File.Delete(file);
                }
            }
            finally { }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="texto"></param>
        /// <param name="newLinesBefore"></param>
        /// <param name="newLinesAfter"></param>
        /// <param name="bootstrapColor"></param>
        /// <param name="showDate"></param>
        private static void WriteConsole(string texto, int newLinesBefore = 0, int newLinesAfter = 0, BootstrapColorEnum bootstrapColor = BootstrapColorEnum.Secondary, bool showDate = true)
        {
            Console.ForegroundColor = GetColorFromBootstrap(bootstrapColor);

            string content = showDate ? string.Format(
                CultureInfo.InvariantCulture,
                "{0} {1}",
                DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss:ffff"),
                texto) : string.Format(
                    CultureInfo.InvariantCulture,
                    "{0}",
                    texto);

            if (newLinesBefore > 0)
            {
                for (int i = 0; i < newLinesBefore; i++)
                {
                    Console.Write(
                        System.Environment.NewLine);

                    if (!string.IsNullOrEmpty(_arquivoLog))
                        WriteFile(
                            System.Environment.NewLine);
                }
            }

            Console.Write(content);
            if (!string.IsNullOrEmpty(_arquivoLog))
                WriteFile(content);

            if (newLinesAfter > 0)
            {
                for (int i = 0; i < newLinesAfter; i++)
                {
                    Console.Write(
                        System.Environment.NewLine);

                    if (!string.IsNullOrEmpty(_arquivoLog))
                        WriteFile(
                            System.Environment.NewLine);
                }
            }

            static ConsoleColor GetColorFromBootstrap(BootstrapColorEnum bootstrapColor = BootstrapColorEnum.Secondary)
            {
                Console.ResetColor();

                if (bootstrapColor != BootstrapColorEnum.Secondary)
                {
                    switch (bootstrapColor)
                    {
                        case BootstrapColorEnum.Primary:
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            break;

                        case BootstrapColorEnum.Success:
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            break;

                        case BootstrapColorEnum.Danger:
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            break;

                        case BootstrapColorEnum.Warning:
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            break;

                        case BootstrapColorEnum.Info:
                            Console.ForegroundColor = ConsoleColor.Blue;
                            break;

                        case BootstrapColorEnum.Light:
                            Console.ForegroundColor = ConsoleColor.White;
                            break;

                        case BootstrapColorEnum.Dark:
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            break;
                    }
                }

                return Console.ForegroundColor;
            }
        }

        private static void WriteFile(string texto)
        {
            using (StreamWriter streamWriter = new StreamWriter(
                _arquivoLog,
                true))
            {
                streamWriter.Write(texto);
                streamWriter.Flush();

                streamWriter.Close();
            }
        }
    }
}