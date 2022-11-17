import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Params } from '@angular/router';
import { LoginResponse, User } from './models/user.model';

@Injectable({ providedIn: 'any' })
export class LoginService {
    constructor(private _http: HttpClient) { }

    login(params: Params, user: User) {
        return this._http.post<LoginResponse>(`${environment.baseUrl}/Account/Login`, user, { params })
    }

    authorize(params: Params) {
        return this._http.get(`${environment.baseUrl}/connect/authorize`, { params })
    }

    register(user: User) {
        return this._http.post<void>(`${environment.baseUrl}/Account/Register`, user)
    }
}