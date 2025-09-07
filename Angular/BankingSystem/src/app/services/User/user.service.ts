import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = 'http://localhost:5139/api/v1/Users/UpdateUserProfile';

  constructor(private http: HttpClient) { }

updateUserPatch(userId: number, patchDoc: any[]): Observable<void> {
  const headers = new HttpHeaders({ 'Content-Type': 'application/json-patch+json' });
  const params = new HttpParams().set('userId', userId.toString());
  return this.http.patch<void>(`${this.apiUrl}`, patchDoc, { headers, params });
}
checkPassword(userId: number, password: string): Observable<boolean> {
  const params = new HttpParams()
    .set('userId', userId.toString())
    .set('password', password);

  return this.http.get<boolean>(`http://localhost:5139/api/v1/Users/CheckPassword`, { params });
}

}
