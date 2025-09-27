import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private baseUrl = environment.apiUrl;
  private apiUrl = `${this.baseUrl}/Users`;

  constructor(private http: HttpClient) { }

updateUserPatch(userId: number, patchDoc: any[]): Observable<void> {
  const headers = new HttpHeaders({ 'Content-Type': 'application/json-patch+json' });
  const params = new HttpParams().set('userId', userId.toString());
  return this.http.patch<void>(`${this.apiUrl}/UpdateUserProfile`, patchDoc, { headers, params });
}
checkPassword(userId: number, password: string): Observable<boolean> {
  const params = new HttpParams()
    .set('userId', userId.toString())
    .set('password', password);

  return this.http.get<boolean>(`${this.apiUrl}/CheckPassword`, { params });
}

}
