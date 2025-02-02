import { Injectable } from '@angular/core';
import { HttpParams, HttpClient } from '@angular/common/http';
import { SettingsService } from '../services/settings.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

export namespace api.Controller {
	@Injectable() export class BaseController
	{
		constructor (protected httpClient: HttpClient, protected settingsService: SettingsService) { } 
	}
	@Injectable() export class AuthController extends api.Controller.BaseController
	{
		public Login(data: api.LoginDto) : Observable<api.TokenDto>
		{
			const body = <any>data;
			return this.httpClient.post<api.TokenDto>(
			this.settingsService.createApiUrl('Auth/Login'),
			body,
			{
				responseType: 'json',
				observe: 'response',
				withCredentials: true
			})
			.pipe(map(response => response.body!));
			
		}
		public Signup(data: api.SignupDto) : Observable<api.TokenDto>
		{
			const body = <any>data;
			return this.httpClient.post<api.TokenDto>(
			this.settingsService.createApiUrl('Auth/Signup'),
			body,
			{
				responseType: 'json',
				observe: 'response',
				withCredentials: true
			})
			.pipe(map(response => response.body!));
			
		}
		constructor (httpClient: HttpClient, settingsService: SettingsService)
		{
			super(httpClient, settingsService);
		}
	}
	@Injectable() export class FollowController extends api.Controller.BaseController
	{
		public IsFollowing(data: api.FollowDto) : Observable<boolean>
		{
			const body = <any>data;
			return this.httpClient.post<boolean>(
			this.settingsService.createApiUrl('Follow/IsFollowing'),
			body,
			{
				responseType: 'json',
				observe: 'response',
				withCredentials: true
			})
			.pipe(map(response => response.body!));
			
		}
		public Follow(data: api.FollowDto) : Observable<any>
		{
			const body = <any>data;
			return this.httpClient.post<any>(
			this.settingsService.createApiUrl('Follow/Follow'),
			body,
			{
				responseType: 'json',
				observe: 'response',
				withCredentials: true
			})
			.pipe(map(response => response.body!));
			
		}
		public Unfollow(data: api.FollowDto) : Observable<any>
		{
			const body = <any>data;
			return this.httpClient.post<any>(
			this.settingsService.createApiUrl('Follow/Unfollow'),
			body,
			{
				responseType: 'json',
				observe: 'response',
				withCredentials: true
			})
			.pipe(map(response => response.body!));
			
		}
		constructor (httpClient: HttpClient, settingsService: SettingsService)
		{
			super(httpClient, settingsService);
		}
	}
	@Injectable() export class GroupController extends api.Controller.BaseController
	{
		public Save(model: api.GroupEditDto) : Observable<any>
		{
			const body = <any>model;
			return this.httpClient.post<any>(
			this.settingsService.createApiUrl('Group/Save'),
			body,
			{
				responseType: 'json',
				observe: 'response',
				withCredentials: true
			})
			.pipe(map(response => response.body!));
			
		}
		public GetSingle(chatId: string) : Observable<api.GroupDto>
		{
			const body = <any>{'chatId': chatId};
			return this.httpClient.get<api.GroupDto>(
			this.settingsService.createApiUrl('Group/GetSingle'),
			{
				params: new HttpParams({ fromObject: body }),
				responseType: 'json',
				observe: 'response',
				withCredentials: true
			})
			.pipe(map(response => response.body!));
			
		}
		constructor (httpClient: HttpClient, settingsService: SettingsService)
		{
			super(httpClient, settingsService);
		}
	}
	@Injectable() export class InboxController extends api.Controller.BaseController
	{
		public GetInbox(options: api.InboxSearchOptions) : Observable<api.PagingResultDto<api.ChatDto>>
		{
			const body = <any>options;
			return this.httpClient.post<api.PagingResultDto<api.ChatDto>>(
			this.settingsService.createApiUrl('Inbox/GetInbox'),
			body,
			{
				responseType: 'json',
				observe: 'response',
				withCredentials: true
			})
			.pipe(map(response => response.body!));
			
		}
		public GetChat(options: api.MessageSearchOptions) : Observable<api.ChatLightDto>
		{
			const body = <any>options;
			return this.httpClient.post<api.ChatLightDto>(
			this.settingsService.createApiUrl('Inbox/GetChat'),
			body,
			{
				responseType: 'json',
				observe: 'response',
				withCredentials: true
			})
			.pipe(map(response => response.body!));
			
		}
		constructor (httpClient: HttpClient, settingsService: SettingsService)
		{
			super(httpClient, settingsService);
		}
	}
	@Injectable() export class MessageController extends api.Controller.BaseController
	{
		public GetMessageList(options: api.MessageSearchOptions) : Observable<api.PagingResultDto<api.MessageDto>>
		{
			const body = <any>options;
			return this.httpClient.post<api.PagingResultDto<api.MessageDto>>(
			this.settingsService.createApiUrl('Message/GetMessageList'),
			body,
			{
				responseType: 'json',
				observe: 'response',
				withCredentials: true
			})
			.pipe(map(response => response.body!));
			
		}
		public CreateMessage(data: api.MessageCreateDto) : Observable<string>
		{
			const body = <any>data;
			return this.httpClient.post(
			this.settingsService.createApiUrl('Message/CreateMessage'),
			body,
			{
				responseType: 'text',
				observe: 'response',
				withCredentials: true
			})
			.pipe(map(response => response.body!));
			
		}
		public EditMessage(data: api.MessageDto) : Observable<any>
		{
			const body = <any>data;
			return this.httpClient.post<any>(
			this.settingsService.createApiUrl('Message/EditMessage'),
			body,
			{
				responseType: 'json',
				observe: 'response',
				withCredentials: true
			})
			.pipe(map(response => response.body!));
			
		}
		public DeleteMessage(id: string) : Observable<any>
		{
			const body = <any>id;
			return this.httpClient.post<any>(
			this.settingsService.createApiUrl('Message/DeleteMessage'),
			body,
			{
				responseType: 'json',
				observe: 'response',
				withCredentials: true
			})
			.pipe(map(response => response.body!));
			
		}
		constructor (httpClient: HttpClient, settingsService: SettingsService)
		{
			super(httpClient, settingsService);
		}
	}
	@Injectable() export class UserController extends api.Controller.BaseController
	{
		public GetCurrentUser() : Observable<api.UserDto>
		{
			return this.httpClient.get<api.UserDto>(
			this.settingsService.createApiUrl('User/GetCurrentUser'),
			{
				responseType: 'json',
				observe: 'response',
				withCredentials: true
			})
			.pipe(map(response => response.body!));
			
		}
		public GetProfile(userId: string) : Observable<api.UserDto>
		{
			return this.httpClient.get<api.UserDto>(
			this.settingsService.createApiUrl('User/GetProfile') + '/' + userId,
			{
				responseType: 'json',
				observe: 'response',
				withCredentials: true
			})
			.pipe(map(response => response.body!));
			
		}
		public GetUserList(options: api.UserSearchOptions) : Observable<api.PagingResultDto<api.UserLightDto>>
		{
			const body = <any>options;
			return this.httpClient.post<api.PagingResultDto<api.UserLightDto>>(
			this.settingsService.createApiUrl('User/GetUserList'),
			body,
			{
				responseType: 'json',
				observe: 'response',
				withCredentials: true
			})
			.pipe(map(response => response.body!));
			
		}
		constructor (httpClient: HttpClient, settingsService: SettingsService)
		{
			super(httpClient, settingsService);
		}
	}
}
export namespace api {
	export class AttachmentDto
	{
		Id: string;
		MessageId: string;
		BlobId: string;
		Order: number;
		Blob: api.BlobDto;
	}
	export class BlobDto
	{
		Id: string;
		TypeId: number;
		Type: api.LookupValueDto;
		MimeType: string;
		Url: string;
		Size: number;
		Duration: number;
	}
	export class ChatDto
	{
		Id: string;
		Name: string;
		IsGroup: boolean;
		IsMuted: boolean;
		ImageUrl: string;
		LastMessageOn: Date;
		LastMessage: string;
		LastMessageStatusId: api.eMessageStatus;
		IsLastMessageMine: boolean;
		UserGenderId: api.eGender;
	}
	export class ChatLightDto
	{
		Id: string;
		UserId: string;
		Name: string;
		IsGroup: boolean;
		IsMuted: boolean;
		ImageUrl: string;
		UserGenderId: api.eGender;
		GroupChatRoleId: api.eChatRole;
		Messages: api.PagingResultDto<api.MessageDto>;
	}
	export class EnumProvider
	{
		Id: number;
		Name: string;
		Description: string;
		BgColor: string;
		CssClass: string;
	}
	export class FileInformationDto
	{
		FileName: string;
		Type: string;
		Size: number;
		Buffer: number[];
	}
	export class FollowDto
	{
		FollowerId: string;
		FollowingId: string;
	}
	export class GroupDto
	{
		ChatId: string;
		GroupName: string;
		GroupPhotoUrl: string;
		BlobId: string;
		Members: api.UserLightDto[];
	}
	export class GroupEditDto
	{
		Id: string;
		Name: string;
		Members: api.UserLightDto[];
	}
	export class InboxSearchOptions
	{
		Skip: number;
		Take: number;
		Filter: string;
		TotalCount: boolean;
	}
	export class LoginDto
	{
		Username: string;
		Password: string;
	}
	export class LookupValueDto
	{
		Id: number;
		Name: string;
		Description: string;
	}
	export class MessageCreateDto
	{
		ChatId: string;
		SenderId: string;
		RecipientId: string;
		Content: string;
		TypeId: api.eMessageType;
		StatusId: api.eMessageStatus;
		Attachments: api.AttachmentDto[];
	}
	export class MessageDto
	{
		Id: string;
		ChatId: string;
		UserId: string;
		Content: string;
		TypeId: api.eMessageType;
		StatusId: api.eMessageStatus;
		IsEditable: boolean;
		IsMine: boolean;
		CreatedOn: Date;
		User: api.UserLightDto;
		Attachments: api.AttachmentDto[];
	}
	export class MessageSearchOptions
	{
		ChatId: string;
		RecipientId: string;
		Skip: number;
		Take: number;
		Filter: string;
		TotalCount: boolean;
	}
	export class PagingResultDto<TData>
	{
		Data: TData[];
		Total: number;
	}
	export class SignupDto
	{
		FirstName: string;
		LastName: string;
		Username: string;
		Email: string;
		Password: string;
		ConfirmPassword: string;
		GenderId: api.eGender;
		IsPrivate: boolean;
	}
	export class TokenDto
	{
		Token: string;
	}
	export class UserDto
	{
		Id: string;
		Username: string;
		FullName: string;
		FirstName: string;
		LastName: string;
		ProfilePhoto: string;
		Thumbnail: string;
		GenderId: api.eGender;
		Gender: api.LookupValueDto;
		IsPrivate: boolean;
		HasAccess: boolean;
		Followers: number;
		Following: number;
		ChatId: string;
	}
	export class UserLightDto
	{
		Id: string;
		Username: string;
		ProfilePhoto: string;
		GenderId: api.eGender;
		IsFollowed: boolean;
		ChatRoleId: api.eChatRole;
	}
	export class UserSearchOptions
	{
		IsFollowed: boolean;
		IsNotSpokenTo: boolean;
		IsNotPartOfGroup: boolean;
		GroupId: string;
		Skip: number;
		Take: number;
		Filter: string;
		TotalCount: boolean;
	}
	@Injectable() export class Providers
	{
		getSystemRoles() : api.EnumProvider[]
		{
			return [
			    { Id: 0, Name: 'NotSet', Description: '', BgColor: '', CssClass: '' },
			    { Id: 1, Name: 'SystemAdministrator', Description: 'System Administrator', BgColor: '', CssClass: '' },
			    { Id: 2, Name: 'Member', Description: 'Member', BgColor: '', CssClass: '' },
			    { Id: 3, Name: 'UserAdmin', Description: 'User Admin', BgColor: '', CssClass: '' },
			    { Id: 4, Name: 'Moderator', Description: 'Moderator', BgColor: '', CssClass: '' },
			    { Id: 5, Name: 'LegalDepartment', Description: 'Legal Department', BgColor: '', CssClass: '' }
			];
		}
		getGenders() : api.EnumProvider[]
		{
			return [
			    { Id: 0, Name: 'NotSet', Description: '', BgColor: '', CssClass: '' },
			    { Id: 1, Name: 'Male', Description: 'Male', BgColor: '', CssClass: '' },
			    { Id: 2, Name: 'Female', Description: 'Female', BgColor: '', CssClass: '' },
			    { Id: 3, Name: 'Other', Description: 'Other', BgColor: '', CssClass: '' }
			];
		}
		getMessageStatuses() : api.EnumProvider[]
		{
			return [
			    { Id: 0, Name: 'NotSet', Description: '', BgColor: '', CssClass: '' },
			    { Id: 1, Name: 'Draft', Description: 'Draft', BgColor: '', CssClass: '' },
			    { Id: 2, Name: 'Sent', Description: 'Sent', BgColor: '', CssClass: 'fa-solid fa-check gray' },
			    { Id: 3, Name: 'Delivered', Description: 'Delivered', BgColor: '', CssClass: 'fa-solid fa-check primary-red' },
			    { Id: 4, Name: 'Seen', Description: 'Seen', BgColor: '', CssClass: 'double-check' },
			    { Id: 5, Name: 'Forwarded', Description: 'Forwarded', BgColor: '', CssClass: 'fa-solid fa-share small' }
			];
		}
		getChatRoles() : api.EnumProvider[]
		{
			return [
			    { Id: 0, Name: 'NotSet', Description: '', BgColor: '', CssClass: '' },
			    { Id: 10, Name: 'Member', Description: 'Member', BgColor: '', CssClass: '' },
			    { Id: 20, Name: 'Admin', Description: 'Admin', BgColor: '', CssClass: '' },
			    { Id: 30, Name: 'Moderator', Description: 'Moderator', BgColor: '', CssClass: '' }
			];
		}
	}
	export enum eSystemRole {
		NotSet = 0,
		SystemAdministrator = 1,
		Member = 2,
		UserAdmin = 3,
		Moderator = 4,
		LegalDepartment = 5
	}
	export enum eGender {
		NotSet = 0,
		Male = 1,
		Female = 2,
		Other = 3
	}
	export enum eMessageStatus {
		NotSet = 0,
		Draft = 1,
		Sent = 2,
		Delivered = 3,
		Seen = 4,
		Forwarded = 5
	}
	export enum eMessageType {
		NotSet = 0,
		Text = 1,
		Image = 100,
		Video = 200,
		Document = 300,
		Voice = 400
	}
	export enum eChatRole {
		NotSet = 0,
		Member = 10,
		Admin = 20,
		Moderator = 30
	}
}
