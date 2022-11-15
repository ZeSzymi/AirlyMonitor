using AirlyMonitor.Models.Dtos;
using FluentValidation;

namespace AirlyMonitor.Extensions
{
    public static class FluentValidationExtentions
    {
        public static void AddFluentValidatiors(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<AlertDefinitionDto>();
        }
    }
}
