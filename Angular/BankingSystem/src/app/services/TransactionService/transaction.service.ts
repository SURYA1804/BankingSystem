import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface TransactionDTO {
  transactionId: number;
  referenceNumber?: string | null;
  transactionDate: string;
  isVerificationRequired: boolean;
  isSuccess: boolean;
  errorMessage?: string | null;
  transactionType: string;
  fromAccount: number;
  fromUser: string;
  fromEmail: string;
  toAccount: number;
  toUser: string;
  toEmail: string;
  amount:number;
}

export interface MakeTransactionDTO {
  fromAccount: number;
  toAccount: number;
  amount: number;
  transactionTypeId: number;
}

@Injectable({ providedIn: 'root' })
export class TransactionService {
  private http = inject(HttpClient);
  private base = 'http://localhost:5139/api/v1/Transcation';

  makeTransaction(dto: MakeTransactionDTO): Observable<string> {
    return this.http.post(`${this.base}/MakeTransaction`, dto, { responseType: 'text' });
  }

  getTransactionsByAccount(accountNumber: number): Observable<TransactionDTO[]> {
    const params = new HttpParams().set('AccountNumber', String(accountNumber));
    return this.http.get<TransactionDTO[]>(`${this.base}/GetAllTransactionsByAccount`, { params });
  }
    getAllToApprove(): Observable<TransactionDTO[]> {
    return this.http.get<TransactionDTO[]>(
      `${this.base}/GetAllTransactionsToApprove`
    );
  }
   approveTransaction(transactionId: number, staffId: number, isApproved: boolean,reason:string): Observable<string> {
    const params = new HttpParams()
      .set('transactionId', transactionId.toString())
      .set('staffId', staffId.toString())
      .set('isApproved', String(isApproved))
      .set('reason', reason ?? 'approved');
    return this.http.post(`${this.base}/ApproveTransaction`, null, { responseType: 'text', params });
  }

}
