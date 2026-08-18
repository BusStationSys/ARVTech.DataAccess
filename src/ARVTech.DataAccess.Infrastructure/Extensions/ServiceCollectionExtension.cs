namespace ARVTech.DataAccess.Infrastructure.Extensions
{
    using ARVTech.DataAccess.Infrastructure.Repositories.Interfaces.SqlServer.Destin;
    using ARVTech.DataAccess.Infrastructure.Repositories.SqlServer.Destin;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDestinDataAccess(this IServiceCollection serviceCollection)
        {
            //serviceCollection.AddScoped<IModalidadeCommand, ModalidadeCommand>();
            //serviceCollection.AddScoped<IModalidadeQuery, ModalidadeQuery>();
            serviceCollection.AddScoped<IModalidadeRepository, ModalidadeRepository>();

            return serviceCollection;
        }
    }
}