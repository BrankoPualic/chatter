using Chatter.Application.Dtos.Auth;
using Chatter.Application.Identity.Interfaces;
using Chatter.Common.Extensions;
using Chatter.Common.Resources;
using Chatter.Domain.Models.Application.Users;
using Humanizer;
using Microsoft.EntityFrameworkCore;

namespace Chatter.Application.UseCases.Auth;

public class LoginCommand(LoginDto data) : BaseCommand<TokenDto>
{
	public LoginDto Data { get; set; } = data;
}

internal class LoginCommandHandler(IDatabaseContext db, ITokenService tokenService, IUserManager userManager) : BaseCommandHandler<LoginCommand, TokenDto>(db)
{
	public override async Task<ResponseWrapper<TokenDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
	{
		var model = await db.Users.Where(_ => _.Username == request.Data.Username).FirstOrDefaultAsync(cancellationToken);
		if (model == null)
			return new(new Error(nameof(User), ResourcesValidation.Wrong_Credentials));

		bool passwordMatch = userManager.VerifyPassword(request.Data.Password, model.Password);
		if (!passwordMatch)
			return new(new Error(nameof(User), ResourcesValidation.Wrong_Credentials));

		// Login log
		UserLoginLog log = new(model.Id);
		db.Logins.Add(log);
		await db.SaveChangesAsync(false, cancellationToken);

		return new(new TokenDto { Token = tokenService.GenerateJwtToken(model) });
	}
}

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
	public LoginCommandValidator()
	{
		RuleFor(_ => _.Data.Username)
			.NotEmpty().WithMessage(ResourcesValidation.Required.FormatWith(nameof(User.Username)));
		RuleFor(_ => _.Data.Username)
			.MaximumLength(20).WithMessage(ResourcesValidation.MaximumLength.FormatWith(nameof(User.Username), 20))
			.When(_ => _.Data.Username.IsNotNullOrWhiteSpace());

		RuleFor(_ => _.Data.Password)
			.NotEmpty().WithMessage(ResourcesValidation.Required.FormatWith(nameof(User.Password)));
		RuleFor(_ => _.Data.Password)
			.MinimumLength(8).WithMessage(ResourcesValidation.MinimumLength.FormatWith(nameof(User.Password), 8))
			.MaximumLength(50).WithMessage(ResourcesValidation.MaximumLength.FormatWith(nameof(User.Password), 50))
			.When(_ => _.Data.Password.IsNotNullOrWhiteSpace());
	}
}