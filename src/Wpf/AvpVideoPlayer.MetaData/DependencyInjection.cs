using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace AvpVideoPlayer.MetaData;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static IServiceCollection RegisterMetadataServices(this IServiceCollection services)
    {
        var filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                                    "avpmeta.dat");
        return services
            .AddSingleton<IMetaDataContext>(sp => new MetaDataLiteDbContext(filename))
            .AddTransient<IMetaDataService, MetaDataService>()
            .AddTransient<ITaggingService, TaggingService>();
    }
}
