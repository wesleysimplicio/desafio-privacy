using FluentValidation;
using DeliveryApi.Application.DTOs;

namespace DeliveryApi.Application.Validators
{
    public class OrderDtoValidator : AbstractValidator<OrderDto>
    {
        public OrderDtoValidator()
        {
            RuleFor(x => x.CustomerName)
                .NotEmpty().WithMessage("O nome do cliente é obrigatório.")
                .MaximumLength(100).WithMessage("O nome do cliente não pode ter mais de 100 caracteres.");

            RuleFor(x => x.Status)
               .NotEmpty().WithMessage("O status é obrigatório.");

            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("Pelo menos um item deve ser fornecido.");

            RuleForEach(x => x.Items).SetValidator(new OrderItemDtoValidator());
        }
    }

}