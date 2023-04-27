namespace ARVTech.DataAccess.Console
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using ARVTech.DataAccess.Business.EquHos;
    using ARVTech.DataAccess.Console.Enums;
    using ARVTech.DataAccess.DbManager;
    using ARVTech.DataAccess.DbManager.Enums;
    using ARVTech.DataAccess.DTOs.EquHos;
    using Microsoft.Extensions.Configuration;

    public static class Program
    {
        private static IConfiguration _configuration = null;

        private readonly static string _arquivoLog = string.Format(
            CultureInfo.InvariantCulture,
            @"{0}\\{1}.log",
            AppDomain.CurrentDomain.BaseDirectory,
            "LogPaymentsTool_" + DateTime.Now.ToString("yyyyMMddHHmm"));

        private readonly static Assembly assembly = Assembly.GetExecutingAssembly();
        private readonly static FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

        private readonly static string _productName = fvi.ProductName;
        private readonly static string _productVersion = fvi.ProductVersion;

        //private readonly static string _version = Assembly.GetEntryAssembly().GetName().Version.ToString();

        //Assembly assembly = Assembly.GetExecutingAssembly();
        //FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
        //var companyName = fvi.CompanyName;
        //var productName = fvi.ProductName;
        //var productVersion = fvi.ProductVersion;

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
                    newLineBefore: true,
                    newLineAfter: true,
                    bootstrapColor: BootstrapColorEnum.Dark);

                ApagarLog();

                WriteConsole(
                    "CARREGANDO as configurações de acesso ao ARVTech.DataAccess®...",
                    bootstrapColor: BootstrapColorEnum.Dark);

                GetOrCreateConfiguration();

                var singletonDbManager = new ContextDbManager(
                    DatabaseTypeEnum.SqlServer,
                    _configuration);

                using (var animalBusiness = new AnimalBusiness(
                    singletonDbManager.UnitOfWork))
                {
                    var animaisDto = animalBusiness.GetAll();

                    var animalDto = animalBusiness.Get(Guid.Parse("016754AF-C8B3-4B58-8108-AE643F92A5D1"));

                    //var dtoInsert = associacaoBusiness.SaveData(
                    //    new AssociacaoDto
                    //    {
                    //        DescricaoRegistro = "A",
                    //        Observacoes = "B",
                    //        RazaoSocial = "C",
                    //        Sigla = "D",
                    //    });

                    //tipoBusiness.Delete(51009);
                }

                //using (var tipoBusiness = new TipoBusiness(
                //    singletonDbManager.UnitOfWork))
                //{
                //    var tiposDto = tipoBusiness.GetAll();

                //    var tipoDto = tipoBusiness.Get(
                //        1);

                //    //var dtoInsert = associacaoBusiness.SaveData(
                //    //    new AssociacaoDto
                //    //    {
                //    //        DescricaoRegistro = "A",
                //    //        Observacoes = "B",
                //    //        RazaoSocial = "C",
                //    //        Sigla = "D",
                //    //    });

                //    tipoBusiness.Delete(51009);
                //}


                //using (var associacaoBusiness = new AssociacaoBusiness(
                //    singletonDbManager.UnitOfWork))
                //{
                //    var associacoesDto = associacaoBusiness.GetAll();

                //    var associacaoDto = associacaoBusiness.Get(
                //        51010);

                //    associacaoDto.DescricaoRegistro = string.Concat("A", associacaoDto.DescricaoRegistro, "A");
                //    associacaoDto.Observacoes = string.Concat("B", associacaoDto.Observacoes, "B");
                //    associacaoDto.RazaoSocial = string.Concat("C", associacaoDto.RazaoSocial, "C");
                //    associacaoDto.Sigla = string.Concat("D", associacaoDto.Sigla, "D");

                //    //var dtoInsert = associacaoBusiness.SaveData(
                //    //    new AssociacaoDto
                //    //    {
                //    //        DescricaoRegistro = "A",
                //    //        Observacoes = "B",
                //    //        RazaoSocial = "C",
                //    //        Sigla = "D",
                //    //    });

                //    var dtoUpdate = associacaoBusiness.SaveData(
                //        new AssociacaoDto
                //        {
                //            Id = 51010,
                //            DescricaoRegistro = "AA",
                //            Observacoes = "BB",
                //            RazaoSocial = "CC",
                //            Sigla = "DD",
                //        });

                //    associacaoBusiness.Delete(51009);
                //}

                //using (var pelagemBusiness = new PelagemBusiness(
                //    singletonDbManager.UnitOfWork))
                //{
                //    var dto = new PelagemDto
                //    {
                //        Descricao = "Descrição Teste",
                //        Observacoes = "Observações Teste",
                //    };

                //    var pelagemDto = pelagemBusiness.SaveData(
                //        dto);

                //    var pelagens = pelagemBusiness.GetAll();

                //    pelagemBusiness.Delete(
                //        (int)pelagemDto.Id);
                //}
            }
            catch (Exception ex)
            {
                WriteConsole(
                    string.Concat(
                        ex.Message,
                        " ",
                        ex.InnerException?.InnerException),
                    newLineBefore: true,
                    bootstrapColor: BootstrapColorEnum.Danger);
            }
            finally
            {
                WriteConsole(
                    "*** Término da execução do ARVTech.DataAccess®. ***",
                    newLineBefore: true);
            }

            //try
            //{
            //    WriteConsole(
            //        string.Format(
            //            CultureInfo.InvariantCulture,
            //            "*** {0} [ Versão {1} ] ***",
            //            _productName,
            //            _productVersion),
            //        bootstrapColor: BootstrapColorEnum.Primary);

            //    WriteConsole("Limpando Log",
            //        newLineBefore: true,
            //        newLineAfter: true,
            //        bootstrapColor: BootstrapColorEnum.Dark);

            //    ApagarLog();

            //    WriteConsole(
            //        "CARREGANDO as configurações de acesso ao ARVTech.DataAccess®...",
            //        bootstrapColor: BootstrapColorEnum.Dark);

            //    GetOrCreateConfiguration();

            //    var singletonDbManagerSource = new ContextDbManager(
            //        DatabaseTypeEnum.Access,
            //        _configuration);

            //    var singletonDbManagerTarget = new ContextDbManager(
            //        DatabaseTypeEnum.SqlServer,
            //        _configuration);

            //    using (var produtoServiceSource = new Services.Parker.ProdutoService(
            //        singletonDbManagerSource.UnitOfWork))
            //    {
            //        using var produtoServiceTarget = new Services.Empresarius.ProdutoService(
            //            singletonDbManagerTarget.UnitOfWork);

            //        var produtosSource = produtoServiceSource.Listar();

            //        if (produtosSource != null &&
            //            produtosSource.Count() > 0)
            //        {
            //            long contador = 0;

            //            foreach (var ps in produtosSource)
            //            {
            //                contador++;

            //                string descricaoProduto = TreatStringWithAccent(
            //                        ps.LinhaProduto);

            //                WriteConsole($"{contador}: {ps.Item} - {descricaoProduto}", showDate: false);

            //                ProdutoModel pm = new ProdutoModel
            //                {
            //                    Descricao = descricaoProduto,
            //                };

            //                produtoServiceTarget.Salvar(pm);
            //            }
            //        }
            //    }

            //    WriteConsole("Configurações de acesso ao ARVTech.DataAccess carregadas com SUCESSO.",
            //        newLineAfter: true,
            //        bootstrapColor: BootstrapColorEnum.Dark);

            //    WriteConsole(string.Empty,
            //        showDate: false);
            //}
            //catch (Exception ex)
            //{
            //    WriteConsole(
            //        string.Concat(
            //            ex.Message,
            //            " ",
            //            ex.InnerException?.InnerException),
            //        newLineBefore: true,
            //        bootstrapColor: BootstrapColorEnum.Danger);
            //}
            //finally
            //{
            //    WriteConsole(
            //        "*** Término da execução do ARVTech.DataAccess®. ***",
            //        newLineBefore: true);
            //}
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
            DateTime dataBase = DateTime.Now.AddDays(-10);

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

        private static void WriteConsole(string texto, bool newLineBefore = false, bool newLineAfter = false, BootstrapColorEnum bootstrapColor = BootstrapColorEnum.Secondary, bool showDate = true)
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

            if (newLineBefore)
            {
                Console.WriteLine(string.Empty);
                if (!string.IsNullOrEmpty(_arquivoLog))
                    WriteFile(string.Empty);
            }

            Console.WriteLine(content);
            if (!string.IsNullOrEmpty(_arquivoLog))
                WriteFile(content);

            if (newLineAfter)
            {
                Console.WriteLine(string.Empty);
                if (!string.IsNullOrEmpty(_arquivoLog))
                    WriteFile(string.Empty);
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
                streamWriter.WriteLine(texto);
                streamWriter.Flush();

                streamWriter.Close();
            }
        }
    }
}