namespace Chatter.Domain.Interfaces;

public interface IDatabaseView
{
	string Script { get; }

	string DropScript { get; }
}