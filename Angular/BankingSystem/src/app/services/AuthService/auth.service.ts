import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IRegister } from '../../../Interfaces/IRegister';
import { IUser } from '../../../Interfaces/IUser';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

 user: IUser | null = null;
 private apiUrl = 'http://localhost:5139/api/v1/Users'

  constructor(private http: HttpClient) {this.SetUser()}

  login(username: string, password: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/Login`, { userName:username,password: password })
  }

SetUser() {
    const userJson = localStorage.getItem('UserDetails');
    if (!userJson) {
      this.user = null;
      return;
    }

    const userObj = JSON.parse(userJson);

    this.user = userObj as IUser;
  }


  register(payload: IRegister): Observable<any> {
    return this.http.post(`${this.apiUrl}/Register`, payload);
  }

  verifyOtp(otp: number): Observable<string> {
    if (!this.user?.email) {
      console.log('User email is not set');
    }
    const url = `${this.apiUrl}/verify-otp?email=${encodeURIComponent(this.user!.email)}&otp=${otp}`;
    return this.http.get(url,{responseType:`text`});
  }


resendOtp(): Observable<string> {
  return this.http.get(`${this.apiUrl}/Get-OTP?email=${this.user?.email}`, {
    responseType: 'text'
  });
}

}
