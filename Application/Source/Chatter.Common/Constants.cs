namespace Chatter.Common;

public static class Constants
{
	public const string SOLUTION_NAME = "Chatter";

	public const string SYSTEM_USER = "00000000-0000-0000-0000-000000000001";

	public const string CLAIM_ID = "ID";
	public const string CLAIM_USERNAME = "USERNAME";
	public const string CLAIM_EMAIL = "EMAIL";
	public const string CLAIM_ROLES = "ROLES";
	public const int TOKEN_EXPIRATION_TIME = 7;

	public const string ERROR_NOT_FOUND = "Not Found!";
	public const string ERROR_INVALID_OPERATION = "Invalid operation!";
	public const string ERROR_UNAUTHORIZED = "Unauthorized!";
	public const string ERROR_INTERNAL_ERROR = "Internal server error!";

	public const int MESSAGE_EDIT_TIME_LIMIT_SECONDS = 600;

	public static readonly DateTime MIN_VALID_DATETIME = new(2025, 1, 1);

	public const string CLOUDINARY_STORAGE = "chatter-social-media-app";
	public const string CLOUDINARY_FILES_STORAGE = $"{CLOUDINARY_STORAGE}/files";
	public const string CLOUDINARY_VIDEO_STORAGE = $"{CLOUDINARY_STORAGE}/videos";

	public static readonly string[] ALLOWED_IMAGE_FORMATS = ["jpg", "jpeg", "png", "gif", "bmp", "webp"];
	public static readonly string[] ALLOWED_VIDEO_FORMATS = ["mp4", "avi", "mov", "mkv", "flv"];
}