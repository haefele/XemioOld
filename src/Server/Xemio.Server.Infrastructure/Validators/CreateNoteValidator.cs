using System;
using FluentValidation;
using Xemio.Shared.Models.Notes;

namespace Xemio.Server.Infrastructure.Validators
{
    public class CreateNoteValidator : AbstractValidator<CreateNote>
    {
        public CreateNoteValidator()
        {
            this.RuleFor(f => f.Title)
                .NotNull()
                .NotEmpty()
                .Length(1, 200);

            this.RuleFor(f => f.FolderId)
                .NotNull();
        }
    }
}