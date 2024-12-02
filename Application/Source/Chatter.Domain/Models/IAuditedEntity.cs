namespace Chatter.Domain.Models;

public interface IAuditedEntity
{
	public Guid CreatedBy { get; set; }

	public DateTime CreatedOn { get; set; }

	public Guid LastChangedBy { get; set; }

	public DateTime LastChangedOn { get; set; }
}
