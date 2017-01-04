using FluentValidation;
using Xemio.Shared.Models.Notes;

namespace Xemio.Server.Infrastructure.Validators
{
    public class UpdateFolderValidator : AbstractValidator<UpdateFolder>
    {
        public UpdateFolderValidator()
        {
            this.RuleFor(f => f.Name)
                .NotNull()
                .NotEmpty()
                .Length(1, 200)
                .When(f => f.HasName());
        }
    }
}