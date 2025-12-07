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

  login(email: string, password: string){
    const endpoint = this.authServiceBaseUrl + "/login"
    return this.http.post<LoginResponse>(endpoint, { email, password })
  }

  storeTokens(accessToken: string, refreshToken: string){
    localStorage.setItem("vaccination_carda:at", accessToken)
    localStorage.setItem("vaccination_carda:rt", refreshToken)
  }

  get accessToken(){
    return localStorage.getItem("vaccination_carda:at")
  }

  logout(){
    localStorage.clear()
    this.router.navigate(["login"])
  }
}
