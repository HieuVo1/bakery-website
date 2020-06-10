using FluentValidation;

namespace eShopSolution.ViewModel.System.Users
{
	public class LoginRequestValidator: AbstractValidator<LoginRequest>
    {
		public LoginRequestValidator()
		{
			RuleFor(x => x.UserName).NotEmpty().WithMessage("User name is required");
			RuleFor(x => x.Passwork).NotEmpty().WithMessage("Passwork is required");
		}
	}
}
