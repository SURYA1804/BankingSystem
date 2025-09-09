import { Component, OnInit } from '@angular/core';
import { CommonModule, DatePipe, NgIf, NgFor } from '@angular/common';
import Swal from 'sweetalert2';
import { LoanDTO, LoanService } from '../../../services/LoanService/loan.service';

@Component({
  selector: 'app-loan-approval',
  standalone: true,
  imports: [CommonModule, NgIf, NgFor, DatePipe],
  templateUrl: './loan-approval.component.html',
  styleUrls: ['./loan-approval.component.css']
})
export class LoanApprovalComponent implements OnInit {
  loading = false;
  error: string | null = null;
  items: LoanDTO[] = [];

  showDetail = false;
  selected: LoanDTO | null = null;
  submitting = false;
  submitError: string | null = null;

  constructor(private svc: LoanService) {}

  private get staffId(): number {
    const raw = localStorage.getItem('UserDetails');
    if (!raw) return 0;
    try {
      const user = JSON.parse(raw) as { userId?: number };
      return Number(user?.userId || 0);
    } catch {
      return 0;
    }
  }

  ngOnInit(): void {
    this.fetch();
  }

  fetch() {
    this.loading = true;
    this.error = null;
    this.svc.getPendingApprovals().subscribe({
      next: (res) => { this.items = res ?? []; this.loading = false; },
      error: (err) => {
        this.error = (typeof err?.error === 'string' && err.error) ? err.error : 'Failed to load loans';
        this.loading = false;
        Swal.fire({ icon: 'error', title: 'Load failed', text: this.error ?? '', confirmButtonColor: '#0d6efd' });
      }
    });
  }

  openDetail(loan: LoanDTO) {
    this.selected = loan;
    this.submitError = null;
    this.showDetail = true;
  }

  closeDetail() {
    this.showDetail = false;
    this.selected = null;
    this.submitting = false;
    this.submitError = null;
  }

  approve() { this.confirmApprove(true); }
  reject()  { this.confirmApprove(false); }



  async confirmApprove(isApproved: boolean) {
  if (!this.selected) return;
  const staffId = this.staffId;
  if (!staffId) {
    await Swal.fire({ icon: 'error', title: 'Missing identity', text: 'Staff identity not found.', confirmButtonColor: '#0d6efd' });
    return;
  }

  if (isApproved) {
    const result = await Swal.fire({
      title: 'Approve loan?',
      text: 'This will approve the selected loan request.',
      icon: 'question',
      showCancelButton: true,
      confirmButtonText: 'Approve',
      cancelButtonText: 'Cancel',
      confirmButtonColor: '#0d6efd',
      cancelButtonColor: '#6c757d',
      showLoaderOnConfirm: true,
      allowOutsideClick: () => !Swal.isLoading(),
      preConfirm: () => {
        return new Promise<void>((resolve, reject) => {
          this.submitting = true;
          this.svc.approveLoan(this.selected!.id, staffId, true, 'approved').subscribe({
            next: () => resolve(),
            error: (e) => reject(typeof e?.error === 'string' ? e.error : 'Approval failed'),
            complete: () => { this.submitting = false; }
          });
        }).catch((msg) => {
          Swal.showValidationMessage(typeof msg === 'string' ? msg : 'Approval failed');
          return undefined;
        });
      }
    });

    if (result.isConfirmed) {
      const id = this.selected!.id;
      this.items = this.items.filter(x => x.id !== id);
      this.closeDetail();
      await Swal.fire({ icon: 'success', title: 'Approved', text: 'Loan approved successfully.', timer: 1400, showConfirmButton: false });
    }
    return;
  }

  const reasonAsk = await Swal.fire({
    title: 'Reject loan',
    input: 'textarea',
    inputLabel: 'Reason for rejection',
    inputPlaceholder: 'Enter the reason...',
    inputAttributes: { 'aria-label': 'Reason for rejection' },
    inputAutoTrim: true,
    inputValidator: (value) => {
      const v = (value ?? '').trim();
      if (!v) return 'Reason is required';
      if (v.length < 3) return 'Reason must be at least 3 characters';
      return undefined;
    },
    showCancelButton: true,
    confirmButtonText: 'Continue',
    cancelButtonText: 'Cancel',
    confirmButtonColor: '#0dcaf0',
    cancelButtonColor: '#6c757d'
  });

  if (!reasonAsk.isConfirmed) return;

  const reason = (reasonAsk.value ?? '').toString().trim();

  const result = await Swal.fire({
    title: 'Confirm rejection',
    text: 'This will reject the selected loan request.',
    icon: 'warning',
    showCancelButton: true,
    confirmButtonText: 'Reject',
    cancelButtonText: 'Back',
    confirmButtonColor: '#0dcaf0',
    cancelButtonColor: '#6c757d',
    showLoaderOnConfirm: true,
    allowOutsideClick: () => !Swal.isLoading(),
    preConfirm: () => {
      return new Promise<void>((resolve, reject) => {
        this.submitting = true;
        this.svc.approveLoan(this.selected!.id, staffId, false, reason).subscribe({
          next: () => resolve(),
          error: (e) => reject(typeof e?.error === 'string' ? e.error : 'Rejection failed'),
          complete: () => { this.submitting = false; }
        });
      }).catch((msg) => {
        Swal.showValidationMessage(typeof msg === 'string' ? msg : 'Rejection failed');
        return undefined;
      });
    }
  });

  if (result.isConfirmed) {
    const id = this.selected!.id;
    this.items = this.items.filter(x => x.id !== id);
    this.closeDetail();
    await Swal.fire({ icon: 'success', title: 'Rejected', text: 'Loan rejected.', timer: 1400, showConfirmButton: false });
  }
}
  }

