namespace SAMS.Models.Help
{
	public sealed class HelpPageViewModel
	{
		public required string DisplayName { get; init; }
		public required DateTime GeneratedAtUtc { get; init; }
		public required IReadOnlyList<RoleHelpSection> Sections { get; init; }
	}
}
