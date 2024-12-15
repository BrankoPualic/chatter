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
		constructor (httpClient: HttpClient, settingsService: SettingsService)
		{
			super(httpClient, settingsService);
		}
	}
}
export namespace api {
	export class EnumProvider
	{
		Id: number;
		Name: string;
		Description: string;
		BgColor: string;
	}
	export class FollowDto
	{
		FollowerId: string;
		FollowingId: string;
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
	}
	@Injectable() export class Providers
	{
		getSystemRoles() : api.EnumProvider[]
		{
			return [
			    { Id: 1, Name: 'SystemAdministrator', Description: 'System Administrator', BgColor: '' },
			    { Id: 2, Name: 'Member', Description: 'Member', BgColor: '' },
			    { Id: 3, Name: 'UserAdmin', Description: 'User Admin', BgColor: '' },
			    { Id: 4, Name: 'Moderator', Description: 'Moderator', BgColor: '' },
			    { Id: 5, Name: 'LegalDepartment', Description: 'Legal Department', BgColor: '' }
			];
		}
		getGenders() : api.EnumProvider[]
		{
			return [
			    { Id: 0, Name: 'NotSet', Description: '', BgColor: '' },
			    { Id: 1, Name: 'Male', Description: 'Male', BgColor: '' },
			    { Id: 2, Name: 'Female', Description: 'Female', BgColor: '' },
			    { Id: 3, Name: 'Other', Description: 'Other', BgColor: '' }
			];
		}
	}
	export enum eSystemRole {
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
}
