import { inject } from '@angular/core';
import { ResolveFn } from '@angular/router';
import { api } from '../_generated/project';

export const inboxResolver: ResolveFn<api.PagingResultDto<api.ChatDto>> = (route, state): Promise<api.PagingResultDto<api.ChatDto>> => {
  const inboxClr = inject(api.Controller.InboxController);
  return inboxClr.GetInbox(new api.InboxSearchOptions()).toPromise();
};
