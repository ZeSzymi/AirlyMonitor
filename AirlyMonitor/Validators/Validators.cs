using AirlyMonitor.Models.Dtos;
using FluentValidation;

namespace AirlyMonitor.Validators
{
    public class AlertDefinitionDtoValidator : AbstractValidator<AlertDefinitionDto>
    {
        public AlertDefinitionDtoValidator()
        {
            RuleFor(x => x.CheckEvery).GreaterThan(0);
        }
    }
}
