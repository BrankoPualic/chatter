using Chatter.Application.Dtos.Auth;
using Chatter.Domain.Models.Application.Users;

namespace Chatter.Application.UseCases.Auth;

public class SignupCommand(SignupDto data) : BaseCommand<TokenDto>
{
	public SignupDto Data { get; set; } = data;
}

internal class SignupCommandHandler(IDatabaseContext db, ITokenService tokenService, IUserManager userManager) : BaseCommandHandler<SignupCommand, TokenDto>(db)
{
	public override async Task<ResponseWrapper<TokenDto>> Handle(SignupCommand request, CancellationToken cancellationToken)
	{
		var existingEmail = await _db.Users.FirstOrDefaultAsync(_ => _.Email == request.Data.Email, cancellationToken);
		if (existingEmail.IsNotNullOrEmpty())
			return new(new Error(nameof(User.Email), ResourcesValidation.Already_Exist.FormatWith(nameof(User.Email))));

		var existingUsername = await _db.Users.FirstOrDefaultAsync(_ => _.Username == request.Data.Username, cancellationToken);
		if (existingUsername.IsNotNullOrEmpty())
			return new(new Error(nameof(User.Username), ResourcesValidation.Already_Exist.FormatWith(nameof(User.Username))));

		User model = new();
		request.Data.ToModel(model);
		model.Password = userManager.HashPassword(request.Data.Password);
		_db.Users.Add(model);

		// Login log
		UserLoginLog log = new(model.Id);
		_db.Logins.Add(log);

		await _db.SaveChangesAsync(true, cancellationToken);

		return new(new TokenDto { Token = tokenService.GenerateJwtToken(model) });
	}
}

public class SignupCommandValidator : AbstractValidator<SignupCommand>
{
	public SignupCommandValidator()
	{
		RuleFor(_ => _.Data.FirstName)
			.NotEmpty().WithMessage(ResourcesValidation.Required.FormatWith(nameof(User.FirstName)));
		RuleFor(_ => _.Data.FirstName)
			.MinimumLength(3).WithMessage(ResourcesValidation.MinimumLength.FormatWith(nameof(User.FirstName), 3))
			.MaximumLength(20).WithMessage(ResourcesValidation.MaximumLength.FormatWith(nameof(User.FirstName), 20))
			.When(_ => _.Data.FirstName.IsNotNullOrWhiteSpace());

		RuleFor(_ => _.Data.LastName)
			.NotEmpty().WithMessage(ResourcesValidation.Required.FormatWith(nameof(User.LastName)));
		RuleFor(_ => _.Data.LastName)
			.MinimumLength(3).WithMessage(ResourcesValidation.MinimumLength.FormatWith(nameof(User.LastName), 3))
			.MaximumLength(20).WithMessage(ResourcesValidation.MaximumLength.FormatWith(nameof(User.LastName), 30))
			.When(_ => _.Data.LastName.IsNotNullOrWhiteSpace());

		RuleFor(_ => _.Data.Username)
			.NotEmpty().WithMessage(ResourcesValidation.Required.FormatWith(nameof(User.Username)));
		RuleFor(_ => _.Data.Username)
			.MinimumLength(3).WithMessage(ResourcesValidation.MinimumLength.FormatWith(nameof(User.Username), 5))
			.MaximumLength(20).WithMessage(ResourcesValidation.MaximumLength.FormatWith(nameof(User.Username), 20))
			.When(_ => _.Data.Username.IsNotNullOrWhiteSpace());

		RuleFor(_ => _.Data.Email)
			.NotEmpty().WithMessage(ResourcesValidation.Required.FormatWith(nameof(User.Email)));
		RuleFor(_ => _.Data.Email)
			.EmailAddress().WithMessage(ResourcesValidation.Wrong_Format.FormatWith(nameof(User.Email)))
			.MaximumLength(80).WithMessage(ResourcesValidation.MaximumLength.FormatWith(nameof(User.Email), 80))
			.When(_ => _.Data.Email.IsNotNullOrWhiteSpace());

		RuleFor(_ => _.Data.Password)
			.NotEmpty().WithMessage(ResourcesValidation.Required.FormatWith(nameof(User.Password)));
		RuleFor(_ => _.Data.Password)
			.MinimumLength(8).WithMessage(ResourcesValidation.MinimumLength.FormatWith(nameof(User.Password), 8))
			.MaximumLength(50).WithMessage(ResourcesValidation.MaximumLength.FormatWith(nameof(User.Password), 50))
			.Matches(@"(?=.*[a-z])").WithMessage("Passwrod lacks 1 lowercase letter.")
			.Matches(@"(?=.*[A-Z])").WithMessage("Passwrod lacks 1 uppercase letter.")
			.Matches(@"(?=.*\d)").WithMessage("Passwrod lacks 1 digit.")
			.Matches(@"(?=.*[@$!%*?&])").WithMessage("Passwrod lacks 1 special character.")
			.When(_ => _.Data.Password.IsNotNullOrWhiteSpace());

		RuleFor(_ => _.Data.ConfirmPassword)
			.NotEmpty().WithMessage(ResourcesValidation.Required.FormatWith("ConfirmPassword"));
		RuleFor(_ => _.Data.ConfirmPassword)
			.Equal(_ => _.Data.Password).WithMessage(ResourcesValidation.Dont_Match.FormatWith(nameof(User.Password), "ConfirmPassword"))
			.When(_ => _.Data.ConfirmPassword.IsNotNullOrWhiteSpace());

		RuleFor(_ => _.Data.GenderId)
			.NotEmpty().WithMessage(ResourcesValidation.Required.FormatWith("Gender"));
	}
}