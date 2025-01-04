namespace ARVTech.DataAccess.Console
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using ARVTech.DataAccess.Business.UniPayCheck;
    using ARVTech.DataAccess.Business.UniPayCheck.Interfaces;
    using ARVTech.DataAccess.Console.Enums;
    using ARVTech.DataAccess.DbManager;
    using ARVTech.DataAccess.DbManager.Enums;
    using ARVTech.DataAccess.DTOs.UniPayCheck;
    using ARVTech.DataAccess.DTOs.UniPayCheck.Enums;
    using ARVTech.Transmission.Engine.UniPayCheck;
    using ARVTech.Transmission.Engine.UniPayCheck.Results;
    using Microsoft.Extensions.Configuration;

    public static class Program
    {
        private static IConfiguration _configuration;

        private readonly static Assembly _assembly = Assembly.GetExecutingAssembly();

        private readonly static FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(
            _assembly.Location);

        private readonly static string _productName = fvi.ProductName;
        //private readonly static string _productVersion = fvi.ProductVersion;
        private readonly static string _fileVersion = fvi.FileVersion;

        private readonly static string _arquivoLog = string.Format(
            CultureInfo.InvariantCulture,
            @"{0}\\Log{1}{2}.log",
            AppDomain.CurrentDomain.BaseDirectory,
            _assembly.GetName().Name?.Replace(
                ".Console",
                string.Empty),
            DateTime.Now.ToString("yyyyMMddHHmm"));

        private static ContextDbManager? _singletonDbManager = default;

        private static IEnumerable<PessoaJuridicaResponseDto>? _pessoasJuridicas = default;

        public static void Main(string[] args)
        {
            try
            {
                // Obtendo as informações de versão do arquivo
                //FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(filePath);

                //_assembly.Location

                // Exibindo a versão do produto
                //string productVersion = versionInfo.ProductVersion;

                // Excluindo qualquer parte do hash ou identificador após o "+"
                //string cleanVersion = extractVersion(_productVersion);

                //Console.WriteLine("Versão do Produto (Formatada): " + cleanVersion);
                //Console.WriteLine("Versão do Arquivo: " + fvi.FileVersion);

                writeConsole(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "*** {0} [ Versão {1} ] ***",
                        _productName,
                        _fileVersion),
                    bootstrapColor: BootstrapColorEnum.Primary);

                writeConsole("Limpando Log",
                    newLinesBefore: 2,
                    bootstrapColor: BootstrapColorEnum.Dark);

                apagarLog();

                writeConsole(
                    "CARREGANDO as configurações de acesso ao ARVTech.DataAccess®...",
                    newLinesBefore: 2,
                    newLinesAfter: 1,
                    bootstrapColor: BootstrapColorEnum.Dark);

                getOrCreateConfiguration();

                _singletonDbManager = new ContextDbManager(
                    DatabaseTypeEnum.SqlServer,
                    _configuration);

                using (var usuarioBusiness = new UsuarioBusiness(
                    _singletonDbManager.UnitOfWork))
                {
                    string username = "UserMain";

                    IEnumerable<UsuarioResponseDto> usuariosResponseDto = usuarioBusiness.GetByUsername(
                        username);

                    if (usuariosResponseDto is null ||
                        usuariosResponseDto.Count() == 0)
                    {
                        var usuarioRequestCreateDto = new UsuarioRequestCreateDto
                        {
                            Username = "UserMain",
                            Password = "(u53rM@1n)",
                            ConfirmPassword = "(u53rM@1n)",
                            DataPrimeiroAcesso = DateTimeOffset.UtcNow,
                            IdPerfilUsuario = PerfilUsuarioEnum.UserMain,
                        };

                        usuarioBusiness.SaveData(
                            usuarioRequestCreateDto);
                    }
                }

                using var pessoaJuridicaBusiness = new PessoaJuridicaBusiness(
                    _singletonDbManager.UnitOfWork);

                //  Importa os Empregadores.
                if (args is null ||
                    args.Length == 0 ||
                    args.Contains("E"))
                    importarEmpregadores();

                _pessoasJuridicas = pessoaJuridicaBusiness.GetAll();

                //  Importa as Matrículas.
                if (args is null ||
                    args.Length == 0 ||
                    args.Contains("M"))
                    importarMatriculas();

                //  Importa os Espelhos de Ponto.
                //if (args is null ||
                //    args.Length == 0 ||
                //    args.Contains("EP"))
                //    importarEspelhosPonto();

                //  Importa os Demonstrativos de Pagamento.
                //if (args is null ||
                //    args.Length == 0 ||
                //    args.Contains("DP"))
                //    importarDemonstrativosPagamento();
            }
            catch (Exception ex)
            {
                writeConsole(
                    string.Concat(
                        ex.Message,
                        " ",
                        ex.InnerException?.InnerException),
                    newLinesBefore: 1,
                    bootstrapColor: BootstrapColorEnum.Danger);
            }
            finally
            {
                writeConsole(
                    "*** Término da execução do ARVTech.DataAccess®. ***",
                    newLinesBefore: 1,
                    bootstrapColor: BootstrapColorEnum.Dark);
            }
        }

        /// <summary>
        /// Método que importa os Empregadores.
        /// </summary>
        private static void importarEmpregadores()
        {
            writeConsole(
                $"PROCESSANDO os Empregadores",
                newLinesBefore: 1,
                newLinesAfter: 2,
                bootstrapColor: BootstrapColorEnum.Dark);

            using var pessoaJuridicaBusiness = new PessoaJuridicaBusiness(
                _singletonDbManager.UnitOfWork);

            var pathDirectoryOrFileNameSource =
                $@"C:\Systemes\ARVTech\ARVTech.Transmission\src\ARVTech.Transmission.Console\bin\Empregadores";

            if (!Directory.Exists(pathDirectoryOrFileNameSource) &&
                !File.Exists(pathDirectoryOrFileNameSource))
                return;

            var transmissionUniPayCheck = new TransmissionUniPayCheck(
                pathDirectoryOrFileNameSource);

            var conteudoEmpregadores = transmissionUniPayCheck.GetConteudoEmpregadores();

            if (string.IsNullOrEmpty(
                conteudoEmpregadores))
            {
                writeConsole(
                    $@"Não encontrado nenhum arquivo de importação de Empregadores no diretório {pathDirectoryOrFileNameSource}.",
                    newLinesAfter: 1,
                    bootstrapColor: BootstrapColorEnum.Warning);
                return;
            }

            try
            {
                var resumoImportacaoEmpregadoresResponseDto = pessoaJuridicaBusiness.ImportFileEmpregadores(
                    conteudoEmpregadores);

                writeConsole(
                    $"Empregadores Atualizados: {resumoImportacaoEmpregadoresResponseDto.QuantidadeRegistrosAtualizados}",
                    newLinesAfter: 1,
                    bootstrapColor: BootstrapColorEnum.Info,
                    showDate: false);

                writeConsole(
                    $"Empregadores Inalterados: {resumoImportacaoEmpregadoresResponseDto.QuantidadeRegistrosInalterados}",
                    newLinesAfter: 1,
                    bootstrapColor: BootstrapColorEnum.Warning,
                    showDate: false);

                writeConsole(
                    $"Empregadores Inseridos: {resumoImportacaoEmpregadoresResponseDto.QuantidadeRegistrosInseridos}",
                    newLinesAfter: 1,
                    bootstrapColor: BootstrapColorEnum.Success,
                    showDate: false);
            }
            catch (Exception ex)
            {
                writeConsole(
                    string.Concat(
                        ex.Message,
                        " ",
                        ex.InnerException?.InnerException),
                    newLinesAfter: 1,
                    newLinesBefore: 1,
                    bootstrapColor: BootstrapColorEnum.Danger);
            }
        }

        /// <summary>
        /// Método que importa as Matrículas.
        /// </summary>
        private static void importarMatriculas()
        {
            foreach (var pessoaJuridica in _pessoasJuridicas)
            {
                writeConsole(
                    $"PROCESSANDO as Matrículas do CNPJ {pessoaJuridica.Cnpj}",
                    newLinesBefore: 1,
                    newLinesAfter: 1,
                    bootstrapColor: BootstrapColorEnum.Dark);

                using var matriculaBusiness = new MatriculaBusiness(
                    _singletonDbManager.UnitOfWork);

                var pathDirectoryOrFileNameSource =
                    $@"C:\Systemes\ARVTech\ARVTech.Transmission\src\ARVTech.Transmission.Console\bin\{pessoaJuridica.Cnpj}";

                if (!Directory.Exists(pathDirectoryOrFileNameSource) &&
                    !File.Exists(pathDirectoryOrFileNameSource))
                {
                    writeConsole(
                        $"Diretório {pathDirectoryOrFileNameSource} não encontrado.",
                        newLinesAfter: 1,
                        bootstrapColor: BootstrapColorEnum.Warning);

                    continue;
                }

                var transmissionUniPayCheck = new TransmissionUniPayCheck(
                    pathDirectoryOrFileNameSource);

                var conteudoMatriculas = transmissionUniPayCheck.GetConteudoMatriculas();

                if (string.IsNullOrEmpty(
                    conteudoMatriculas))
                {
                    writeConsole(
                        $@"Arquivo de importação de Matrículas no diretório {pathDirectoryOrFileNameSource} encontra-se vazio.",
                        newLinesAfter: 1,
                        bootstrapColor: BootstrapColorEnum.Warning);
                    return;
                }

                try
                {
                    var resumoImportacaoMatriculasResponseDto = matriculaBusiness.ImportFileMatriculas(
                        pessoaJuridica.Cnpj,
                        conteudoMatriculas);

                    writeConsole(
                        $"Matrículas Atualizadas: {resumoImportacaoMatriculasResponseDto.QuantidadeRegistrosAtualizados}",
                        newLinesAfter: 1,
                        bootstrapColor: BootstrapColorEnum.Info,
                        showDate: false);

                    writeConsole(
                        $"Matrículas Inalteradas: {resumoImportacaoMatriculasResponseDto.QuantidadeRegistrosInalterados}",
                        newLinesAfter: 1,
                        bootstrapColor: BootstrapColorEnum.Warning,
                        showDate: false);

                    writeConsole(
                        $"Matrículas Inseridas: {resumoImportacaoMatriculasResponseDto.QuantidadeRegistrosInseridos}",
                        newLinesAfter: 1,
                        bootstrapColor: BootstrapColorEnum.Success,
                        showDate: false);
                }
                catch (Exception ex)
                {
                    writeConsole(
                        string.Concat(
                            ex.Message,
                            " ",
                            ex.InnerException?.InnerException),
                        newLinesAfter: 1,
                        newLinesBefore: 1,
                        bootstrapColor: BootstrapColorEnum.Danger);
                }

                /*foreach (var matricula in matriculas)
                {
                    writeConsole(
                        $"Matrícula {matricula.Matricula}; Colaborador CPF: {matricula.Cpf}; Nome: {matricula.Nome}. ",
                        bootstrapColor: BootstrapColorEnum.Dark);

                    try
                    {
                        var executionResponseDto = matriculaBusiness.Import(
                            matricula);

                        string texto = "OK";

                        BootstrapColorEnum bootstrapColor = BootstrapColorEnum.Success;

                        if (!executionResponseDto.Success)
                        {
                            texto = executionResponseDto.Message;
                            bootstrapColor = BootstrapColorEnum.Warning;
                        }

                        writeConsole(
                            texto,
                            newLinesAfter: 1,
                            bootstrapColor: bootstrapColor,
                            showDate: false);
                    }
                    catch (Exception ex)
                    {
                        writeConsole(
                            string.Concat(
                                ex.Message,
                                " ",
                                ex.InnerException?.InnerException),
                            newLinesAfter: 1,
                            newLinesBefore: 1,
                            bootstrapColor: BootstrapColorEnum.Danger);
                    }
                }*/
            }
        }

        /// <summary>
        /// Método que importa os Demonstrativos de Pagamento.
        /// </summary>
        private static void importarDemonstrativosPagamento()
        {
            foreach (var pessoaJuridica in _pessoasJuridicas)
            {
                writeConsole(
                    $"PROCESSANDO os Demonstrativos de Pagamento do CNPJ {pessoaJuridica.Cnpj}",
                    newLinesBefore: 1,
                    newLinesAfter: 2,
                    bootstrapColor: BootstrapColorEnum.Dark);

                using var matriculaDemonstrativoPagamentoBusiness = new MatriculaDemonstrativoPagamentoBusiness(
                    _singletonDbManager.UnitOfWork);

                var pathDirectoryOrFileNameSource =
                    $@"C:\Systemes\ARVTech\ARVTech.Transmission\src\ARVTech.Transmission.Console\bin\{pessoaJuridica.Cnpj}";

                if (!Directory.Exists(pathDirectoryOrFileNameSource) &&
                    !File.Exists(pathDirectoryOrFileNameSource))
                    continue;

                var transmissionUniPayCheck = new TransmissionUniPayCheck(
                    pathDirectoryOrFileNameSource);

                var demonstrativosPagamento = transmissionUniPayCheck.GetDemonstrativosPagamento();

                if (demonstrativosPagamento == null ||
                    demonstrativosPagamento.Count() == 0)
                {
                    writeConsole(
                        $@"Não encontrado nenhum arquivo de importação de Demonstrativo de Pagamento no diretório {pathDirectoryOrFileNameSource}.",
                        newLinesAfter: 1,
                        newLinesBefore: 0,
                        bootstrapColor: BootstrapColorEnum.Warning);

                    continue;
                }

                foreach (var dp in demonstrativosPagamento)
                {
                    writeConsole(
                        $"Demonstrativo de Pagamento ref. Competência: {dp.Competencia}; Matrícula: {dp.Matricula}; Nome: {dp.Nome}. ",
                        bootstrapColor: BootstrapColorEnum.Dark);

                    try
                    {
                        var executionResponseDto = matriculaDemonstrativoPagamentoBusiness.Import(
                            dp);

                        string texto = "OK";

                        BootstrapColorEnum bootstrapColor = BootstrapColorEnum.Success;

                        if (!executionResponseDto.Success)
                        {
                            texto = executionResponseDto.Message;
                            bootstrapColor = BootstrapColorEnum.Warning;
                        }

                        writeConsole(
                            texto,
                            newLinesAfter: 1,
                            bootstrapColor: bootstrapColor,
                            showDate: false);
                    }
                    catch (Exception ex)
                    {
                        writeConsole(
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

        /// <summary>
        /// Método que importa os Espelhos de Ponto.
        /// </summary>
        private static void importarEspelhosPonto()
        {
            foreach (var pessoaJuridica in _pessoasJuridicas)
            {
                writeConsole(
                    $"PROCESSANDO os Espelho de Ponto do CNPJ {pessoaJuridica.Cnpj}",
                    newLinesBefore: 1,
                    newLinesAfter: 2,
                    bootstrapColor: BootstrapColorEnum.Dark);

                using var matriculaEspelhoPontoBusiness = new MatriculaEspelhoPontoBusiness(
                    _singletonDbManager.UnitOfWork);

                var pathDirectoryOrFileNameSource =
                    $@"C:\Systemes\ARVTech\ARVTech.Transmission\src\ARVTech.Transmission.Console\bin\{pessoaJuridica.Cnpj}";

                if (!Directory.Exists(pathDirectoryOrFileNameSource) &&
                    !File.Exists(pathDirectoryOrFileNameSource))
                    continue;

                var transmissionUniPayCheck = new TransmissionUniPayCheck(
                    pathDirectoryOrFileNameSource);

                var espelhosPonto = transmissionUniPayCheck.GetEspelhosPonto();

                if (espelhosPonto == null ||
                    espelhosPonto.Count() == 0)
                {
                    writeConsole(
                        $@"Não encontrado nenhum arquivo de importação de Espelho de Ponto no diretório {pathDirectoryOrFileNameSource}.",
                        newLinesAfter: 1,
                        newLinesBefore: 0,
                        bootstrapColor: BootstrapColorEnum.Warning);

                    continue;
                }

                foreach (var ep in espelhosPonto)
                {
                    writeConsole(
                        $"Espelho de Ponto ref. Competência: {ep.Competencia}; Matrícula: {ep.Matricula}; Nome: {ep.Nome}. ",
                        bootstrapColor: BootstrapColorEnum.Dark);

                    try
                    {
                        var executionResponseDto = matriculaEspelhoPontoBusiness.Import(
                            ep);

                        string texto = "OK";

                        BootstrapColorEnum bootstrapColor = BootstrapColorEnum.Success;

                        if (!executionResponseDto.Success)
                        {
                            texto = executionResponseDto.Message;
                            bootstrapColor = BootstrapColorEnum.Warning;
                        }

                        writeConsole(
                            texto,
                            newLinesAfter: 1,
                            bootstrapColor: bootstrapColor,
                            showDate: false);
                    }
                    catch (Exception ex)
                    {
                        writeConsole(
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

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="Exception"></exception>
        private static void getOrCreateConfiguration()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"; // "Production" é o default

            // Configura o builder de configurações
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)  // arquivo padrão
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);  // arquivo específico do ambiente

            _configuration = builder.Build();

            if (_configuration == null)
                throw new Exception("[ERRO] Não foi possível carregar as configurações do Integrador.");
        }

        /// <summary>
        /// 
        /// </summary>
        private static void apagarLog()
        {
            DateTime dataBase = DateTime.Now.AddDays(-7);

            try
            {
                string[] files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "LOG*.log");

                foreach (string file in files)
                {
                    var fs = new FileInfo(file);

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
        private static void writeConsole(string texto, int newLinesBefore = 0, int newLinesAfter = 0, BootstrapColorEnum bootstrapColor = BootstrapColorEnum.Secondary, bool showDate = true)
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
                        writeFile(
                            System.Environment.NewLine);
                }
            }

            Console.Write(content);
            if (!string.IsNullOrEmpty(_arquivoLog))
                writeFile(content);

            if (newLinesAfter > 0)
            {
                for (int i = 0; i < newLinesAfter; i++)
                {
                    Console.Write(
                        Environment.NewLine);

                    if (!string.IsNullOrEmpty(
                        _arquivoLog))
                        writeFile(
                            Environment.NewLine);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="texto"></param>
        private static void writeFile(string texto)
        {
            using (var streamWriter = new StreamWriter(
                _arquivoLog,
                true))
            {
                streamWriter.Write(texto);
                streamWriter.Flush();

                streamWriter.Close();
            }
        }

        /// <summary>
        /// Função para extrair a versão principal (antes do "+").
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        private static string extractVersion(string version)
        {
            // Verifica se a versão contém um "+" e separa a string
            if (version.Contains('+'))
            {
                // Retorna apenas a parte antes do "+"
                return version.Split('+')[0];
            }
            else
            {
                // Se não houver "+", retorna a versão como está
                return version;
            }
        }

        ///// <summary>
        ///// Formata o valor hexadecimal para o formato de versão.
        ///// </summary>
        ///// <param name="versionValue"></param>
        ///// <returns></returns>
        //private static string formatVersion(uint versionValue)
        //{
        //    // Dividir o valor em major, minor, build, revision
        //    int major = (int)((versionValue >> 24) & 0xFF);
        //    int minor = (int)((versionValue >> 16) & 0xFF);
        //    int build = (int)((versionValue >> 8) & 0xFF);
        //    int revision = (int)(versionValue & 0xFF);

        //    return $"{major}.{minor}.{build}.{revision}";
        //}

        ///// <summary>
        ///// Verifica se o valor é hexadecimal.
        ///// </summary>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //private static bool isHexadecimal(string value)
        //{
        //    foreach (char c in value)
        //    {
        //        if (!Uri.IsHexDigit(c))
        //            return false;
        //    }

        //    return true;
        //}
    }
}