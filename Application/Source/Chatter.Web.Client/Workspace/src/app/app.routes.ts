import { Routes } from '@angular/router';
import { Constants } from './constants/constants';
import { authGuard } from './guards/auth.guard';
import { inboxResolver } from './resolvers/inbox.resolver';
import { editProfileResolver } from './resolvers/edit-profile.resolver';

export const routes: Routes = [
  {
    path: Constants.ROUTE_HOME,
    title: 'Home | ' + Constants.TITLE,
    canActivate: [authGuard],
    loadComponent: () => import('./pages/home/home.component').then(_ => _.HomeComponent)
  },

  // Log in and Sign up
  {
    path: Constants.ROUTE_LOGIN,
    title: 'Log in | ' + Constants.TITLE,
    canActivate: [authGuard],
    loadComponent: () => import('./pages/auth/login/login.component').then(_ => _.LoginComponent)
  },
  {
    path: Constants.ROUTE_SIGNUP,
    title: 'Sign up | ' + Constants.TITLE,
    canActivate: [authGuard],
    loadComponent: () => import('./pages/auth/signup/signup.component').then(_ => _.SignupComponent)
  },

  {
    path: Constants.ROUTE_PROFILE + '/' + Constants.PARAM_ID,
    canActivate: [authGuard],
    children: [
      {
        path: '',
        title: 'Profile | ' + Constants.TITLE,
        pathMatch: 'full',
        loadComponent: () => import('./pages/profile/profile.component').then(_ => _.ProfileComponent)
      },
      {
        path: Constants.ROUTE_PROFILE_EDIT,
        title: 'Edit Profile | ' + Constants.TITLE,
        resolve: { profile: editProfileResolver },
        loadComponent: () => import('./pages/profile/edit-profile/edit-profile.component').then(_ => _.EditProfileComponent)
      }
    ]
  },

  {
    path: Constants.ROUTE_EXPLORE,
    canActivate: [authGuard],
    loadComponent: () => import('./pages/explore/explore.component').then(_ => _.ExploreComponent),
    children: [
      {
        path: '',
        redirectTo: Constants.ROUTE_TOP,
        pathMatch: 'full'
      },
      {
        path: Constants.ROUTE_TOP,
        title: 'Explore Top | ' + Constants.TITLE,
        loadComponent: () => import('./pages/explore/top/top.component').then(_ => _.TopComponent)
      },
      {
        path: Constants.ROUTE_USERS,
        title: 'Explore Users | ' + Constants.TITLE,
        loadComponent: () => import('./pages/explore/users/users.component').then(_ => _.UsersComponent)
      },
      {
        path: Constants.ROUTE_POSTS,
        title: 'Explore Posts | ' + Constants.TITLE,
        loadComponent: () => import('./pages/explore/posts/posts.component').then(_ => _.PostsComponent)
      },
    ],
  },

  {
    path: Constants.ROUTE_INBOX,
    canActivate: [authGuard],
    children: [
      {
        path: '',
        resolve: { inbox: inboxResolver },
        title: 'Inbox | ' + Constants.TITLE,
        loadComponent: () => import('./pages/inbox/inbox.component').then(_ => _.InboxComponent),
        pathMatch: 'full'
      },
      {
        path: Constants.ROUTE_CHAT + '/' + Constants.PARAM_ID,
        title: 'Chat | ' + Constants.TITLE,
        loadComponent: () => import('./pages/inbox/chat/chat.component').then(_ => _.ChatComponent)
      },
      {
        path: Constants.ROUTE_START_NEW_CHAT,
        title: 'Start Chatting | ' + Constants.TITLE,
        loadComponent: () => import('./pages/inbox/start-new-chat/start-new-chat.component').then(_ => _.StartNewChatComponent)
      },
      {
        path: Constants.ROUTE_EDIT_GROUP_CHAT + '/' + Constants.PARAM_ID,
        loadComponent: () => import('./pages/inbox/edit-group-chat/edit-group-chat.component').then(_ => _.EditGroupChatComponent)
      }
    ]
  }
];
