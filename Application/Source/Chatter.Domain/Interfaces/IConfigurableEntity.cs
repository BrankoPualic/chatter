using Microsoft.EntityFrameworkCore;

namespace Chatter.Domain.Interfaces;

public interface IConfigurableEntity
{
	void Configure(ModelBuilder builder);
}