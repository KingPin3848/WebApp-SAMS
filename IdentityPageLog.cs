using Microsoft.Extensions.Logging;

namespace SAMS.IdentityLogging
{
	internal static partial class IdentityPageLog
	{
		[LoggerMessage(LogLevel.Information, "User created a new account WITHOUT password.")]
		internal static partial void AccountCreatedWithoutPassword(ILogger logger);

		[LoggerMessage(LogLevel.Critical, "Error message: \n {ErrorMessage}")]
		internal static partial void ActivationModelError(ILogger logger, string errorMessage);

		[LoggerMessage(LogLevel.Information, "The email address from the Google Account was not found with the one in the database.")]
		internal static partial void GoogleEmailMismatch(ILogger logger);

		[LoggerMessage(LogLevel.Information, "Couldn't find the user from the Google Account email address in our database.")]
		internal static partial void GoogleUserNotFound(ILogger logger);

		[LoggerMessage(LogLevel.Information, "THE ACCOUNT IS NOT LOCKED OUT.")]
		internal static partial void ExternalAccountNotLockedOut(ILogger logger);

		[LoggerMessage(LogLevel.Information, "{Name} logged in with {LoginProvider} provider.")]
		internal static partial void ExternalUserLoggedIn(ILogger logger, string? name, string loginProvider);

		[LoggerMessage(LogLevel.Information, "User logged in.")]
		internal static partial void UserLoggedIn(ILogger logger);

		[LoggerMessage(LogLevel.Warning, "User account locked out.")]
		internal static partial void UserAccountLockedOut(ILogger logger);

		[LoggerMessage(LogLevel.Information, "User with ID '{UserId}' logged in with 2fa.")]
		internal static partial void UserLoggedInWith2fa(ILogger logger, string userId);

		[LoggerMessage(LogLevel.Warning, "User with ID '{UserId}' account locked out.")]
		internal static partial void UserAccountLockedOutWith2fa(ILogger logger, string userId);

		[LoggerMessage(LogLevel.Warning, "Invalid authenticator code entered for user with ID '{UserId}'.")]
		internal static partial void InvalidAuthenticatorCode(ILogger logger, string userId);

		[LoggerMessage(LogLevel.Information, "User with ID '{UserId}' logged in with a recovery code.")]
		internal static partial void UserLoggedInWithRecoveryCode(ILogger logger, string userId);

		[LoggerMessage(LogLevel.Warning, "Invalid recovery code entered for user with ID '{UserId}' ")]
		internal static partial void InvalidRecoveryCode(ILogger logger, string userId);

		[LoggerMessage(LogLevel.Information, "User logged out.")]
		internal static partial void UserLoggedOut(ILogger logger);

		[LoggerMessage(LogLevel.Information, "User created a new account with password.")]
		internal static partial void AccountCreatedWithPassword(ILogger logger);

		[LoggerMessage(LogLevel.Information, "User changed their password successfully.")]
		internal static partial void PasswordChanged(ILogger logger);

		[LoggerMessage(LogLevel.Information, "User with ID '{UserId}' deleted themselves.")]
		internal static partial void PersonalDataDeleted(ILogger logger, string userId);

		[LoggerMessage(LogLevel.Information, "User with ID '{UserId}' has disabled 2fa.")]
		internal static partial void TwoFactorDisabled(ILogger logger, string userId);

		[LoggerMessage(LogLevel.Information, "User with ID '{UserId}' asked for their personal data.")]
		internal static partial void PersonalDataRequested(ILogger logger, string userId);

		[LoggerMessage(LogLevel.Information, "User with ID '{UserId}' has enabled 2FA with an authenticator app.")]
		internal static partial void AuthenticatorEnabled(ILogger logger, string userId);

		[LoggerMessage(LogLevel.Information, "User with ID '{UserId}' has generated new 2FA recovery codes.")]
		internal static partial void RecoveryCodesGenerated(ILogger logger, string userId);

		[LoggerMessage(LogLevel.Information, "User with ID '{UserId}' has reset their authentication app key.")]
		internal static partial void AuthenticatorReset(ILogger logger, string userId);
	}
}
