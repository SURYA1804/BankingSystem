import { Component, OnInit, inject, PLATFORM_ID } from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { finalize } from 'rxjs';
import Swal from 'sweetalert2';
import { TransactionService } from '../../../services/TransactionService/transaction.service';

@Component({
  selector: 'app-make-transaction',
  standalone: true,
  templateUrl: './make-transaction.component.html',
  styleUrls: ['./make-transaction.component.css'],
  imports: [CommonModule, ReactiveFormsModule]
})
export class MakeTransactionComponent implements OnInit {
  private fb = inject(FormBuilder);
  private api = inject(TransactionService);
  private platformId = inject(PLATFORM_ID);

  submitting = false;

  defaultFromAccount: number | null = null;

  transactionTypes = [
    { id: 1, name: 'Deposit' },
    { id: 2, name: 'Withdrawal' },
    { id: 3, name: 'Transfer' }
  ];

  form = this.fb.group({
    fromAccount: [null as number | null, [Validators.required]],
    toAccount: [null as number | null, [Validators.required]],
    amount: [null as number | null, [Validators.required, Validators.min(0.01)]],
    transactionTypeId: [null as number | null, [Validators.required]]
  });

  ngOnInit(): void {
    if (isPlatformBrowser(this.platformId)) {
      const raw = localStorage.getItem('DefaultFromAccount');
      const parsed = raw ? Number(raw) : NaN;
      if (!isNaN(parsed)) {
        this.defaultFromAccount = parsed;
        this.form.patchValue({ fromAccount: parsed });
      }
    }
  }

  submit() {
    if (this.form.invalid) {
      Swal.fire({ icon: 'error', title: 'Error', text: 'Please fill all fields correctly' });
      return;
    }
    this.submitting = true;

    const dto = {
      fromAccount: Number(this.form.value.fromAccount),
      toAccount: Number(this.form.value.toAccount),
      amount: Number(this.form.value.amount),
      transactionTypeId: Number(this.form.value.transactionTypeId)
    };

    this.api.makeTransaction(dto)
      .pipe(finalize(() => this.submitting = false))
      .subscribe({
        next: (msg) => {
          const text = typeof msg === 'string' ? msg : 'Transaction successful.';
          Swal.fire({ icon: 'success', title: 'Success', text, timer: 1800, showConfirmButton: false });
          this.form.reset();
        },
        error: (err) => {
          const text = err?.error || err?.message || 'Transaction failed.';
          Swal.fire({ icon: 'error', title: 'Error', text });
        }
      });
  }
}
