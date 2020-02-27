import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Runner } from 'protractor';
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  jwtHelper = new JwtHelperService();
  baseURL = 'http://localhost:5000/api/auth';

  decodedToken: any;
  constructor(private http: HttpClient, ) { }

  login(model: any) {
    return this.http.post(`${this.baseURL}/login`, model)
      .pipe(map((response: any) => {
        const user = response;
        if (user) {
          this.setToken(user.data.accessToken);
          this.decodeToken();
        }
      }));
  }
  register(model: any) {
    return this.http.post(`${this.baseURL}/register`, model);
  }
  isLoggedIn() {
    const token = this.getToken();
    if (token && token !== 'undefined') {
      return !this.jwtHelper.isTokenExpired(token);
    }
    return false;
  }
  LogOut() {
    localStorage.clear();
    this.decodedToken = undefined;
  }
  getToken() {
    return localStorage.getItem('token');
  }
  setToken(token: string) {
    localStorage.setItem('token', token);
  }
  decodeToken() {
    const token = this.getToken();
    if (token && token !== 'undefined') {
      this.decodedToken = this.jwtHelper.decodeToken(token);
    }
  }
}
