export class Constants {
  static DEFAULT_PHOTO_MALE = 'default-avatar-profile-picture-male-icon.png';
  static DEFAULT_PHOTO_FEMALE = 'default-avatar-profile-picture-female-icon.png';
  static DEFAULT_PHOTO_GROUP = 'default-group-image.jpg';

  static GUID_EMPTY = '00000000-0000-0000-0000-000000000000';

  static TOKEN = 'token.1';

  // Search options
  static TAKE = 25;

  // Textarea
  static MAX_ROWS = 4;

  // Providers
  static Providers = {
    Genders: 'Genders',
    SystemRoles: 'SystemRoles',
    MessageStatuses: 'MessageStatuses'
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
  static ROUTE_EXPLORE = 'explore';
  static ROUTE_EXPLORE_TOP = 'explore/top';
  static ROUTE_EXPLORE_USERS = 'explore/users';
  static ROUTE_EXPLORE_POSTS = 'explore/posts';
  static ROUTE_TOP = 'top';
  static ROUTE_USERS = 'users';
  static ROUTE_POSTS = 'posts';
  static ROUTE_INBOX = 'inbox';
  static ROUTE_CHAT = 'chat';

  // Route Parameters
  static PARAM_ID = ':id';
}
