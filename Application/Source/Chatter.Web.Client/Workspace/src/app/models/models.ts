import { api } from "../_generated/project";

export interface IModelError {
    key: string;
    errors: string[];
    [key: string]: any;
}

export interface ICurrentUser {
    id?: number;
    username?: string;
    email?: string;
    roles?: api.eSystemRole[];
    token?: string;
    tokenExpirationDate?: Date;
}