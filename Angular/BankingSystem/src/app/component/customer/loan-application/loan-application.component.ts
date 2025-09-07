import { Component, OnInit, inject, PLATFORM_ID } from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { finalize } from 'rxjs';
import Swal from 'sweetalert2';
import { LoanDTO, LoanService } from '../../../services/LoanService/loan.service';

@Component({
  selector: 'app-loan-application',
  standalone: true,
  templateUrl: './loan-application.component.html',
  styleUrls: ['./loan-application.component.css'],
  imports: [CommonModule, ReactiveFormsModule]
})
export class LoanApplicationComponent implements OnInit {
  private fb = inject(FormBuilder);
  private api = inject(LoanService);
  private platformId = inject(PLATFORM_ID);

  submitting = false;
  userId: number | null = null;
  loans: LoanDTO[] = [];
  loansLoading = false;
  loansError: string | null = null;

  loanTypes = [
    { id: 1, name: 'Personal Loan' },
    { id: 2, name: 'Home Loan' },
    { id: 3, name: 'Car Loan' },
    { id: 4, name: 'Education Loan' }
  ];

  form = this.fb.group({
    loanTypeId: [null as number | null, Validators.required],
    loanAmount: [null as number | null, [Validators.required, Validators.min(1)]],
    currentSalaryInLPA: [null as number | null, [Validators.required, Validators.min(0)]]
  });

ngOnInit(): void {
  if (isPlatformBrowser(this.platformId)) {
    const raw = localStorage.getItem('UserDetails');
    if (raw) {
      try {
        const user = JSON.parse(raw);
        if (user && typeof user.userId === 'number') {
          this.userId = user.userId;
        }
      } catch {
        
      }
    }
    this.loadLoansForUser();
  }
}


private loadLoansForUser() {
  if (this.userId == null) return;
  this.loansLoading = true;
  this.loansError = null;

  this.api.getMyLoans(this.userId)
    .pipe(finalize(() => this.loansLoading = false))
    .subscribe({
      next: list => this.loans = Array.isArray(list) ? list : [],
      error: err => {
        this.loans = [];
        this.loansError = err?.error || err?.message || 'Failed to load loans';
      }
    });
}
 submit() {
  if (this.form.invalid || this.userId == null) {
    Swal.fire({ icon: 'error', title: 'Error', text: 'Fill all fields; user not found' });
    return;
  }
  this.submitting = true;

  const dto = {
    userId: this.userId,
    loanTypeId: Number(this.form.value.loanTypeId),
    loanAmount: Number(this.form.value.loanAmount),
    currentSalaryInLPA: Number(this.form.value.currentSalaryInLPA)
  };

  this.api
    .create(dto)
    .pipe(finalize(() => (this.submitting = false)))
    .subscribe({
      next: (res) => {
        const msg = typeof res === 'string' ? res : 'Loan request submitted';
        Swal.fire({ icon: 'success', title: 'Success', text: msg, timer: 1800, showConfirmButton: false });
        this.form.reset();
        this.loadLoansForUser(); 
      },
      error: (err) => {
        const msg = err?.error || err?.message || 'Failed to submit loan request';
        Swal.fire({ icon: 'error', title: 'Error', text: msg });
      }
    });
}
}
