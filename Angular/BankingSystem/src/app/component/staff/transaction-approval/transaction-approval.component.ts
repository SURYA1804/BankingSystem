import { Component, OnInit } from '@angular/core';
import { CommonModule, DatePipe, NgIf, NgFor } from '@angular/common';
import { TransactionDTO, TransactionService } from '../../../services/TransactionService/transaction.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-transaction-approval',
  standalone: true,
  imports: [CommonModule, NgIf, NgFor, DatePipe],
  templateUrl: './transaction-approval.component.html',
  styleUrls: ['./transaction-approval.component.css']
})
export class TransactionApprovalComponent implements OnInit {
  loading = false;
  error: string | null = null;
  items: TransactionDTO[] = [];

  showDetail = false;
  selected: TransactionDTO | null = null;
  submitting = false;
  submitError: string | null = null;

  constructor(private svc: TransactionService) {}

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
    this.svc.getAllToApprove().subscribe({
      next: (res) => {
        this.items = res ?? [];
        this.loading = false;
      },
      error: (err) => {
        this.error = (typeof err?.error === 'string' && err.error) ? err.error : 'Failed to load transactions';
        this.loading = false;
        Swal.fire({
          icon: 'error',
          title: 'Load failed',
          text: this.error ?? '',
          confirmButtonColor: '#0d6efd'
        });
      }
    });
  }

  openDetail(tx: TransactionDTO) {
    this.selected = tx;
    this.submitError = null;
    this.showDetail = true;
  }

  closeDetail() {
    this.showDetail = false;
    this.selected = null;
    this.submitting = false;
    this.submitError = null;
  }

  approve() {
    this.confirmApprove();
  }

  reject() {
    this.promptRejectReason();
  }

  private async confirmApprove() {
    if (!this.selected) return;
    const staffId = this.staffId;
    if (!staffId) {
      this.submitError = 'Missing staff identity';
      await Swal.fire({
        icon: 'error',
        title: 'Missing identity',
        text: this.submitError ?? '',
        confirmButtonColor: '#0d6efd'
      });
      return;
    }

    const result = await Swal.fire({
      title: 'Approve transaction?',
      text: 'This will approve the selected transaction.',
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
          this.svc.approveTransaction(this.selected!.transactionId, staffId, true, 'approved').subscribe({
            next: () => resolve(),
            error: (e) => {
              const msg: string = (typeof e?.error === 'string' && e.error) ? e.error : 'Approval failed';
              reject(msg);
            }
          });
        }).catch((err) => {
          Swal.showValidationMessage(typeof err === 'string' ? err : 'Approval failed');
          return false;
        }).finally(() => {
          this.submitting = false;
        });
      }
    });

    if (result.isConfirmed) {
      const txId = this.selected!.transactionId;
      this.items = this.items.filter(x => x.transactionId !== txId);
      this.closeDetail();
      await Swal.fire({
        icon: 'success',
        title: 'Approved',
        text: 'Transaction approved successfully.',
        timer: 1400,
        showConfirmButton: false
      });
    }
  }

  private async promptRejectReason() {
    if (!this.selected) return;
    const staffId = this.staffId;
    if (!staffId) {
      this.submitError = 'Missing staff identity';
      await Swal.fire({
        icon: 'error',
        title: 'Missing identity',
        text: this.submitError ?? '',
        confirmButtonColor: '#0d6efd'
      });
      return;
    }

    const result = await Swal.fire({
      title: 'Reject transaction',
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
      confirmButtonText: 'Submit',
      cancelButtonText: 'Cancel',
      confirmButtonColor: '#0dcaf0',
      cancelButtonColor: '#6c757d',
      showLoaderOnConfirm: true,
      allowOutsideClick: () => !Swal.isLoading(),
      preConfirm: (value) => {
        const reason = (value ?? '').toString();
        return new Promise<void>((resolve, reject) => {
          this.submitting = true;
          this.svc.approveTransaction(this.selected!.transactionId, staffId, false, reason).subscribe({
            next: () => resolve(),
            error: (e) => {
              const msg: string = (typeof e?.error === 'string' && e.error) ? e.error : 'Rejection failed';
              reject(msg);
            }
          });
        }).catch((err) => {
          Swal.showValidationMessage(typeof err === 'string' ? err : 'Rejection failed');
          throw err;
        }).finally(() => {
          this.submitting = false;
        });
      }
    });

    if (result.isConfirmed) {
      const txId = this.selected!.transactionId;
      this.items = this.items.filter(x => x.transactionId !== txId);
      this.closeDetail();
      await Swal.fire({
        icon: 'success',
        title: 'Rejected',
        text: 'Transaction rejected.',
        timer: 1400,
        showConfirmButton: false
      });
    }
  }
}
