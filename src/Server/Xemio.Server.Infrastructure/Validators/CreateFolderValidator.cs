using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using Xemio.Shared.Models.Notes;

namespace Xemio.Server.Infrastructure.Validators
{
    public class CreateFolderValidator : AbstractValidator<CreateFolder>
    {
        public CreateFolderValidator()
        {
            this.RuleFor(f => f.Name)
                .NotNull()
                .NotEmpty()
                .Length(1, 200);

            this.RuleFor(f => f.ParentFolderId)
                .NotEqual(default(Guid));
        }
    }
}
