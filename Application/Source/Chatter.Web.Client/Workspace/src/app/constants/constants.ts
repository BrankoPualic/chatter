export class Constants {
  static DEFAULT_PHOTO_MALE = 'default-avatar-profile-picture-male-icon.png';
  static DEFAULT_PHOTO_FEMALE = 'default-avatar-profile-picture-female-icon.png';
  static DEFAULT_PHOTO_GROUP = 'default-group-image.jpg';

  static GUID_EMPTY = '00000000-0000-0000-0000-000000000000';

  static TOKEN = 'token.1';

  // File Validation
  static VIDEO_DURATION_60s = 60;
  static VIDEO_SIZE_100MB = 100 * 1024 * 1024;
  static IMAGE_SIZE_10MB = 10 * 1024 * 1024;

  // Search options
  static TAKE = 25;

  // Textarea
  static MAX_ROWS = 4;

  // Providers
  static Providers = {
    Genders: 'Genders',
    SystemRoles: 'SystemRoles',
    MessageStatuses: 'MessageStatuses',
    ChatRoles: 'ChatRoles'
  }

  // Route Titles
  static TITLE = 'Chatter';

  // Routes
  static ROUTE_HOME = '';
  static ROUTE_NOT_FOUND = 'not-found';
  static ROUTE_UNAUTHORIZED = 'unauthorized';
  static ROUTE_LOGIN = 'login';
  static ROUTE_SIGNUP = 'signup';
  static ROUTE_PROFILE = 'profile';
  static ROUTE_PROFILE_EDIT = 'edit_profile';
  static ROUTE_EXPLORE = 'explore';
  static ROUTE_EXPLORE_TOP = 'explore/top';
  static ROUTE_EXPLORE_USERS = 'explore/users';
  static ROUTE_EXPLORE_POSTS = 'explore/posts';
  static ROUTE_TOP = 'top';
  static ROUTE_USERS = 'users';
  static ROUTE_POSTS = 'posts';
  static ROUTE_INBOX = 'inbox';
  static ROUTE_START_NEW_CHAT = 'start_new';
  static ROUTE_EDIT_GROUP_CHAT = 'edit_group_chat';
  static ROUTE_CHAT = 'chat';
  static ROUTE_CREATE_POST = 'create_post';

  // Route Parameters
  static PARAM_ID = ':id';
}
