using Common.Services;
using Common.Services.Interfaces;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class ServiceCollectionExtension
	{
		public static IServiceCollection AddRepositoriesFactory(this IServiceCollection services)
		{
			return services
				.AddScoped<IRepositoriesFactory, DaprRepositoriesFactory>();
		}
	}
}
