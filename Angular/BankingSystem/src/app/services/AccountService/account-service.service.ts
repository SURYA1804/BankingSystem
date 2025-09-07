import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IAccountSummary } from '../../../Interfaces/IAccountSummary';

export interface ApiMessage { message: string; }
export interface AccountDTO {
  accountNumber: number;
  balance: number;
  createdAt: string;
  userId: number;
  userName: string;
  accountTypeName: string;
  accountTypeId: number;
  lastTransactionAt?: string | null;
  currency: string;
  isActive: boolean;
  isAccountClosed: boolean;
}
export interface AccountUpdateTicketDTO {
  ticketId: number;
  accountNumber: number;
  requestedChange: string;
  requestedBy: string;
  requestedAt: string;        
  isApproved: boolean;
  isProcessed: boolean;
  approvedBy?: string | null;
  approvedAt?: string | null;
  rejectionReaosn?: string | null;
}


@Injectable({ providedIn: 'root' })
export class AccountService {
  private http = inject(HttpClient);
  private base = 'http://localhost:5139/api/v1/Account';
  private basestaffurl = 'http://localhost:5139/api/v1/staff';
  createAccount(body: { userId: number; accountType: string; OpeningBalance: number;  }): Observable<ApiMessage> {
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

  getAccountsByUserId(userId: number): Observable<AccountDTO[]> {
    const params = new HttpParams().set('userId', String(userId));
    return this.http.get<AccountDTO[]>(`${this.base}/AccountByUser`, { params });
  }
  reviewAccountTypeChange(ticketId: number, staffId: number, action: number, rejectionReason: string): Observable<string> {
    const params = new HttpParams()
      .set('ticketId', ticketId.toString())
      .set('staffId', staffId.toString())
      .set('action', action.toString())
      .set('RejectionReaosn', rejectionReason ?? '');

    return this.http.post(`${this.basestaffurl}/ReviewAccountTypeChange`, null, { responseType: 'text', params });
  }

  getAllAccountUpdateTickets(): Observable<any[]> {
    return this.http.get<any[]>(`${this.basestaffurl}/GetAllAccountUpdateTickets`);
  }

  getAllPendingAccountUpdateTickets(): Observable<any[]> {
    return this.http.get<any[]>(`${this.basestaffurl}/GetAllPendingAccountUpdateTickes`);
  }
}

