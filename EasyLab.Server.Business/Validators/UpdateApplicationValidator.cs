using FluentValidation;

namespace EasyLab.Server.Business.Validators
{
    class UpdateApplicationValidator : AbstractValidator<EasyLab.Server.DTOs.Application>
    {
        public UpdateApplicationValidator()
        {
            RuleFor(o => o.Id).Required();
            RuleFor(o => o.Id).MaxLength(2);
            RuleFor(o => o.Name).AllowNullButNotEmpty();
            RuleFor(o => o.Name).MaxLength(50);
            RuleFor(o => o.Version).MaxLength(15);
            RuleFor(o => o.DbVersion).MaxLength(15);
        }
    }
}
