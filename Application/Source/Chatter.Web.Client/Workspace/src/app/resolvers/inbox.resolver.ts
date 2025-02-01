import { inject } from '@angular/core';
import { ResolveFn } from '@angular/router';
import { api } from '../_generated/project';
import { PageLoaderService } from '../services/page-loader.service';

export const inboxResolver: ResolveFn<api.PagingResultDto<api.ChatDto>> = (route, state): Promise<api.PagingResultDto<api.ChatDto>> => {
  const inboxClr = inject(api.Controller.InboxController);
  const loader = inject(PageLoaderService);

  loader.show();
  return inboxClr.GetInbox(new api.InboxSearchOptions()).toPromise();
};
