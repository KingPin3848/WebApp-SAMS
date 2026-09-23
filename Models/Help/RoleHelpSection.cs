namespace SAMS.Models.Help
{
	public sealed class RoleHelpSection
	{
		public required string RoleName { get; init; }
		public required string Summary { get; init; }
		public required IReadOnlyList<string> AllowedBehaviors { get; init; }
		public required IReadOnlyList<string> NotAllowedBehaviors { get; init; }
	}
}
