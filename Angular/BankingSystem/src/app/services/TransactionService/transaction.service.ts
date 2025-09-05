import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

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
}
