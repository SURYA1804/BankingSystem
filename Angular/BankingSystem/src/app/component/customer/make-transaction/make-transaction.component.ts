import { Component, OnInit, inject, PLATFORM_ID } from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { finalize } from 'rxjs';
import Swal from 'sweetalert2';
import { TransactionService } from '../../../services/TransactionService/transaction.service';
import { AccountDTO, AccountService } from '../../../services/AccountService/account-service.service';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'app-make-transaction',
  standalone: true,
  templateUrl: './make-transaction.component.html',
  styleUrls: ['./make-transaction.component.css'],
  imports: [CommonModule, ReactiveFormsModule,MatFormFieldModule,MatSelectModule,MatInputModule]
})
export class MakeTransactionComponent implements OnInit {
  private fb = inject(FormBuilder);
  private api = inject(TransactionService);
  private AccountApi = inject(AccountService)
  private platformId = inject(PLATFORM_ID);
  fromAccounts: AccountDTO[] = [];

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
    const rawUser = isPlatformBrowser(this.platformId) ? localStorage.getItem('UserDetails') : null;
    const userId = rawUser ? Number(JSON.parse(rawUser)?.userId) : null;

    if (userId != null && !Number.isNaN(userId)) {
      this.AccountApi.getAccountsByUserId(userId).subscribe({
        next: (list) => {
          this.fromAccounts = Array.isArray(list) ? list.filter(m=>m.isActive == true) : [];
        },
        error: () => { this.fromAccounts = []; }
      });
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
