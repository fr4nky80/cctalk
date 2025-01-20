using CashSystem.Protocols.CCTalk.Polls;
using Microsoft.Extensions.DependencyInjection;

namespace CashSystem.Protocols.CCTalk.IoC
{
    public static class ServicesCollectionExtension
    {
        public static IServiceCollection AddCCTalk(this IServiceCollection services, string comPort)
        {
            services.AddKeyedSingleton<ICCTalkCommunicationManager, CCTalkCommunicationManager>(comPort);

            services.AddKeyedScoped<IHandler, PollHandler>()

            return services;
        }
    }
}
