import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IAccountSummary } from '../../../Interfaces/IAccountSummary';

export interface ApiMessage { message: string; }


@Injectable({ providedIn: 'root' })
export class AccountService {
  private http = inject(HttpClient);
  private base = '/api/v1/Account';

  createAccount(body: { userId: number; accountType: string; initialDeposit: number; nickname?: string }): Observable<ApiMessage> {
    return this.http.post<ApiMessage>(`${this.base}/CreateAccount`, body);
  }

  requestTypeChange(accountNumber: number, newAccountType: string, userId: number): Observable<string> {
    const params = new HttpParams()
      .set('accountNumber', accountNumber.toString())
      .set('newAccountType', newAccountType)
      .set('userId', userId.toString());
    return this.http.post(`${this.base}/request-change`, null, { responseType: 'text', params });
  }

  closeAccount(accountNumber: number): Observable<ApiMessage> {
    const params = new HttpParams().set('accountNumber', accountNumber.toString());
    return this.http.post<ApiMessage>(`${this.base}/CloseAccount`, null, { params });
  }

  getAllAccounts(): Observable<IAccountSummary[]> {
    return this.http.get<IAccountSummary[]>(`${this.base}/AllAccount`);
  }

  getAccountsByUser(userId: number): Observable<IAccountSummary[]> {
    const params = new HttpParams().set('userId', userId.toString());
    return this.http.get<IAccountSummary[]>(`${this.base}/AccountByUser`, { params });
  }
}
