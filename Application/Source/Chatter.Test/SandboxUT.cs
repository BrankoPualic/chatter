using AutoMapper;
using Chatter.Application.Dtos.Users;
using Chatter.Application.Extensions;
using Microsoft.EntityFrameworkCore;

[TestFixture]
public partial class SandboxUT : BaseUT
{
	[Test, Explicit]
	public async Task SandboxBPR()
	{
		var userid = Guid.Parse("67aa6c84-71d4-4462-be6b-08dd17cb6506");

		var result = await db.Users
			//.Include(_ => _.Blobs.Where(_ => (_.IsProfilePhoto == true || _.IsThumbnail == true) && _.Blob.IsActive == true))
			.Where(_ => _.Id == userid)
			.FirstOrDefaultAsync();

		await db.UserBlobs
			.Where(_ => _.UserId == userid)
			.Where(_ => _.IsProfilePhoto == true || _.IsThumbnail == true)
			.Where(_ => _.Blob.IsActive == true)
			.Include(_ => _.Blob)
		.ToListAsync();

		var follows = await db.Follows
		.Where(_ => _.FollowerId == userid || _.FollowingId == userid)
		.GroupBy(_ => _.FollowingId == userid)
			.Select(_ => new
			{
				Following = _.Count(_ => _.FollowerId == userid),
				Followers = _.Count(_ => _.FollowingId == userid)
			})
			.FirstOrDefaultAsync();

		var data = Get<IMapper>().To<UserDto>(result);

		data.ProfilePhoto = result.Blobs.Where(_ => _.IsProfilePhoto == true).Select(_ => _.Blob).FirstOrDefault()?.Url;
		data.Thumbnail = result.Blobs.Where(_ => _.IsThumbnail == true).Select(_ => _.Blob).FirstOrDefault()?.Url;

		data.Following = follows?.Following ?? 0;
		data.Followers = follows?.Followers ?? 0;
		data.HasAccess = CurrentUser.Id == userid
		|| !data.IsPrivate
			|| (await db.Follows.FirstOrDefaultAsync(_ => _.FollowerId == CurrentUser.Id && _.FollowingId == data.Id) != null);
	}
}