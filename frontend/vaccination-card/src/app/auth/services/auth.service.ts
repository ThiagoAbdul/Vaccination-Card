import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { LoginResponse } from '../models/login-response';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private http = inject(HttpClient)
  private authServiceBaseUrl = environment.authServiceUrl + "/Auth"
  private router = inject(Router)

  private readonly AT_STORAGE_KEY = "vaccination_carda:at"
  private readonly RT_STORAGE_KEY = "vaccination_carda:rt"

  login(email: string, password: string){
    const endpoint = this.authServiceBaseUrl + "/login"
    return this.http.post<LoginResponse>(endpoint, { email, password })
  }

  storeTokens(accessToken: string, refreshToken: string){
    localStorage.setItem(this.AT_STORAGE_KEY, accessToken)
    localStorage.setItem(this.RT_STORAGE_KEY, refreshToken)
  }

  get accessToken(){
    return localStorage.getItem(this.AT_STORAGE_KEY)
  }
  get refreshToken(){
    return localStorage.getItem(this.RT_STORAGE_KEY)
  }

  logout(){
    localStorage.clear()
    this.router.navigate(["login"])
  }

  refreshTokens(rt: string){
    const endpoint = this.authServiceBaseUrl + "/refresh-token"

    return this.http.post<LoginResponse>(endpoint, rt)
  }
}
