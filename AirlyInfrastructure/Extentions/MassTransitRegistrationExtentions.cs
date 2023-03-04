using AirlyInfrastructure.Models.Messages;
using AirlyInfrastructure.Models.Options;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;


namespace AirlyInfrastructure.Extentions
{
    public static class MassTransitRegistrationExtentions
    {
        public static void RegisterMassTransit(this IServiceCollection services, RabbitConfigurationOptions rabbitConfiguration)
        {
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitConfiguration.Address, "/", h => {
                        h.Username(rabbitConfiguration.UserName);
                        h.Password(rabbitConfiguration.Password);
                    });

                    cfg.Message<PushNotificationMessage>(x => x.SetEntityName("PushNotificationMessage"));

                    cfg.ConfigureEndpoints(context);
                });
            });
        }
    }
}
