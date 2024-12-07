using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Chatter.Persistence.Migrations;

[DbContext(typeof(DatabaseContext))]
[Migration("20241207200824_Views")]
public partial class Views : ViewsMigration
{
}