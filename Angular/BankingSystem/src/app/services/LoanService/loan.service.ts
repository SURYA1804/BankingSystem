import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

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
  private base = 'http://localhost:5139/api/v1/Loan';

  create(dto: CreateLoanDTO): Observable<string> {
    return this.http.post(`${this.base}/create`, dto, { responseType: 'text' });
  }
  getMyLoans(userId: number) {
  const params = new HttpParams().set('UserId', String(userId));
  return this.http.get<LoanDTO[]>(`${this.base}/GetMyLoans`, { params });
}
  getPendingApprovals(): Observable<LoanDTO[]> {
    return this.http.get<LoanDTO[]>(`${this.base}/pending-approvals`);
  }
    approveLoan(loanId: number, staffId: number, isApproved: boolean,reason:string): Observable<string> {
    const params = new HttpParams()
      .set('loanId', String(loanId))
      .set('staffId', String(staffId))
      .set('reason', String(reason))
      .set('isApproved', String(isApproved));
    return this.http.post(`${this.base}/approve`, null, { responseType: 'text', params });
  }
    getAllLoans(): Observable<LoanDTO[]> {
    return this.http.get<LoanDTO[]>(`${this.base}/GetAllLoans`);
  }
}
