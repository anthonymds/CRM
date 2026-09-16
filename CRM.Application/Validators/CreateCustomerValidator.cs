using CRM.Application.DTOs;
using FluentValidation;

namespace CRM.Application.Validators;

public class CreateCustomerValidator
    : AbstractValidator<CreateCustomerRequest>
{
    public CreateCustomerValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Nome é obrigatório.")
            .MaximumLength(200)
            .WithMessage("Nome deve possuir no máximo 200 caracteres.");


        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email é obrigatório.")
            .EmailAddress()
            .WithMessage("Email informado é inválido.");


        RuleFor(x => x.Document)
            .NotEmpty()
            .WithMessage("Documento é obrigatório.")
            .MaximumLength(20)
            .WithMessage("Documento deve possuir no máximo 20 caracteres.");


        RuleFor(x => x.Phone)
            .MaximumLength(20)
            .WithMessage("Telefone deve possuir no máximo 20 caracteres.");


        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Tipo de cliente inválido.");
    }
}