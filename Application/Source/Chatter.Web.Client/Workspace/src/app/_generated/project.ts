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
}
export namespace api {
	export class EnumProvider
	{
		Id: number;
		Name: string;
		Description: string;
		BgColor: string;
	}
	export class LookupValueDto
	{
		Id: number;
		Name: string;
		Description: string;
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
	}
	export enum eSystemRole {
		SystemAdministrator = 1,
		Member = 2,
		UserAdmin = 3,
		Moderator = 4,
		LegalDepartment = 5
	}
}
