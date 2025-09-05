import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface CreateLoanDTO {
  userId: number;
  loanTypeId: number;
  loanAmount: number;
  currentSalaryInLPA: number;
}

@Injectable({ providedIn: 'root' })
export class LoanService {
  private http = inject(HttpClient);
  private base = 'http://localhost:5139/api/v1/Loan';

  create(dto: CreateLoanDTO): Observable<string> {
    return this.http.post(`${this.base}/create`, dto, { responseType: 'text' });
  }
}
