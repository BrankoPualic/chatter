import { ResolveFn } from '@angular/router';
import { api } from '../_generated/project';
import { inject } from '@angular/core';
import { PageLoaderService } from '../services/page-loader.service';

export const editProfileResolver: ResolveFn<api.UserDto> = (route, state): Promise<api.UserDto> => {
  const userClr = inject(api.Controller.UserController);
  const loader = inject(PageLoaderService);

  loader.show();
  return userClr.GetProfile(route.paramMap.get('id')!).toPromise();
};
