using CRM.Application.DTOs;
using FluentValidation;

namespace CRM.Application.Validators;

public class CreateLeadValidator : AbstractValidator<CreateLeadRequest>
{
    public CreateLeadValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(200)
            .WithMessage("Name must not exceed 200 characters.");

        RuleFor(x => x.Email)
                .EmailAddress()
                .MaximumLength(200)
                .When(x => !string.IsNullOrWhiteSpace(x.Email))
                .WithMessage("Email address is invalid.");

        RuleFor(x => x.Phone)
                .MaximumLength(20)
                .When(x => !string.IsNullOrWhiteSpace(x.Phone))
                .WithMessage("Phone number must not exceed 20 characters.");

        RuleFor(x => x.Document)
                .MaximumLength(20)
                .When(x => !string.IsNullOrWhiteSpace(x.Document))
                .WithMessage("Document must not exceed 20 characters.");

        RuleFor(x => x.Source)
                .IsInEnum()
                .WithMessage("Lead source is invalid.");
    }
}
