namespace Chatter.Domain.Models;

public class DatabaseMigrationResult
{
	public string Result { get; set; }

	public List<string> Migrations { get; set; }
}