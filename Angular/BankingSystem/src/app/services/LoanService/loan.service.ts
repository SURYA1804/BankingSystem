import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface CreateLoanDTO {
  userId: number;
  loanTypeId: number;
  loanAmount: number;
  currentSalaryInLPA: number;
}

export interface LoanDTO {
  id: number;
  userId: number;
  userName?: string | null;
  loanTypeId: number;
  loanAmount:number;
  loanTypeName?: string | null;
  createdAt: string;
  isApproved: boolean;
  approvedAt?: string | null;
  approvedBy: number;
  currentSalaryInLPA: number;
  customerType?: string | null;
  isEmployed: boolean;
  rejectionReason:string;
  isProcessed:boolean;
}


@Injectable({ providedIn: 'root' })
export class LoanService {
  private http = inject(HttpClient);
  private BaseUrl = environment.apiUrl;
  
  private apiUrl = `${this.BaseUrl}/Loan`;

  create(dto: CreateLoanDTO): Observable<string> {
    return this.http.post(`${this.apiUrl}/create`, dto, { responseType: 'text' });
  }
  getMyLoans(userId: number) {
  const params = new HttpParams().set('UserId', String(userId));
  return this.http.get<LoanDTO[]>(`${this.apiUrl}/GetMyLoans`, { params });
}
  getPendingApprovals(): Observable<LoanDTO[]> {
    return this.http.get<LoanDTO[]>(`${this.apiUrl}/pending-approvals`);
  }
    approveLoan(loanId: number, staffId: number, isApproved: boolean,reason:string): Observable<string> {
    const params = new HttpParams()
      .set('loanId', String(loanId))
      .set('staffId', String(staffId))
      .set('reason', String(reason))
      .set('isApproved', String(isApproved));
    return this.http.post(`${this.apiUrl}/approve`, null, { responseType: 'text', params });
  }
    getAllLoans(): Observable<LoanDTO[]> {
    return this.http.get<LoanDTO[]>(`${this.apiUrl}/GetAllLoans`);
  }
}
