using Chatter.Application.Dtos.Follows;

[TestFixture]
public partial class SandboxUT : BaseUT
{
	[Test, Explicit]
	public async Task SandboxBPR()
	{
		var response = await TestHttpClient.TestAsync(new()
		{
			Url = "https://localhost:7039/api/Follow/Unfollow",
			Method = HttpMethod.Post,
			Payload = new FollowDto
			{
				FollowerId = Guid.Empty,
				FollowingId = Guid.Empty,
			}
		});
	}
}