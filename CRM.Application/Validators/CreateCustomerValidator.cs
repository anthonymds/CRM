using CRM.Application.DTOs;
using FluentValidation;

namespace CRM.Application.Validators;

public class CreateCustomerValidator : AbstractValidator<CreateCustomerRequest>
{
    public CreateCustomerValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(200)
            .WithMessage("Name must not exceed 200 characters.");


        RuleFor(x => x.Email)
            .NotEmpty()
            .MaximumLength(200)
            .WithMessage("Email address is required")
            .EmailAddress()
            .WithMessage("Email address is invalid");


        RuleFor(x => x.Document)
            .NotEmpty()
            .WithMessage("Document is required.")
            .MaximumLength(20)
            .WithMessage("Document must not exceed 20 characters.");


        RuleFor(x => x.Phone)
            .MaximumLength(20)
            .WithMessage("Phone must not exceed 20 characters.");

        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Type invalid.");
    }
}