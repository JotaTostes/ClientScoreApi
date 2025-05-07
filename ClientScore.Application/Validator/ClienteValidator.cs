using ClientScore.Application.DTOs;
using FluentValidation;

namespace ClientScore.Application.Validator
{
    public class ClienteValidator : AbstractValidator<ClienteRequestDto>
    {
        public ClienteValidator()
        {
            RuleFor(c => c.Nome)
                .NotEmpty().WithMessage("Nome é obrigatório.");

            RuleFor(c => c.DataNascimento)
                .NotEmpty().WithMessage("Data de nascimento é obrigatória.")
                .LessThan(DateTime.Today).WithMessage("Data de nascimento deve ser no passado.");

            RuleFor(c => c.CPF)
                .NotEmpty().WithMessage("CPF é obrigatório.")
                .Must(ValidaCpf).WithMessage("CPF inválido.");

            RuleFor(c => c.Email)
                .NotEmpty().WithMessage("Email é obrigatório.")
                .EmailAddress().WithMessage("Email inválido.");

            RuleFor(c => c.RendimentoAnual)
                .GreaterThan(0).WithMessage("Rendimento Anual deve ser maior que zero.");

            RuleFor(c => c.Estado)
                .NotEmpty().WithMessage("Estado é obrigatório.");

            RuleFor(c => c.Telefone)
                .Must(TelefoneValidator.IsValid)
                .WithMessage("Telefone inválido. Ex: 1191234-5678")
                .NotEmpty().WithMessage("Telefone é obrigatório.")
                .Matches(@"^\d{10,11}$").WithMessage("Telefone deve conter DDD e número.");
        }

        private bool ValidaCpf(string cpf)
        {
            return cpf != null && cpf.Length == 11 && cpf.All(char.IsDigit);
        }
    }
}
