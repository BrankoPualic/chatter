namespace Chatter.Domain.Interfaces;

public interface IDatabaseIndex
{
	string Sql { get; }

	string DropSql { get; }
}