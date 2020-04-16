using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModel.System.Users
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("User name is required");
            RuleFor(x => x.Phone).NotEmpty().WithMessage("Phone is required");
            RuleFor(x => x.Passwork).NotEmpty().WithMessage("Passwork is required");
            RuleFor(x => x.Passwork).NotEmpty().WithMessage("Passwork is required");
            RuleFor(x => x.Dob).NotEmpty().GreaterThan(DateTime.Now.AddYears(-100)).WithMessage("Birth day is incorrect");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required").Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")
                .WithMessage("Email format is match");
            RuleFor(x => x).Custom((request, context) => {
                if (request.Passwork != request.ConfirmPasswork)
                {
                    context.AddFailure("ConfirmPassword is not match ");
                }
            });
        }
    }
}
