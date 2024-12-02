using Chatter.Common.Extensions;
using Chatter.Domain.Interfaces;

namespace Chatter.Domain;

public class DatabaseIndex(string indexName, string tableName) : IDatabaseIndex
{
	public List<string> Columns { get; set; } = [];

	public List<string> Include { get; set; } = [];

	public bool IsUnique { get; set; }

	public string Where { get; set; }

	public string Sql => $@"CREATE{(IsUnique ? " UNIQUE" : "")} NONCLUSTERED INDEX [{indexName}] ON [{tableName.Split('.').Join("].[")}]
(
{Columns
		.Select(_ => $"\t[{_.Split(" ").Take(1).Join(" ")}]{_.Split(" ").Skip(1).Join(" ")}")
		.Join(",\n")}
) {(Include?.Count > 0 ? $"\nINCLUDE (\n{Include
						 .Select(_ => $"\t[{_}]")
						 .Join(",\n")})" : "\n")}
{Where.IfNotNullOrEmpty("WHERE {0}")}
WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO";

	public string DropSql => $@"DROP INDEX IF EXISTS [{indexName}] ON [{tableName.Split('.').Join("].[")}]";

	public override string ToString() => $"{DropSql}\n{Sql}";
}