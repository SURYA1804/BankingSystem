import { Component, OnInit, inject, PLATFORM_ID } from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormsModule } from '@angular/forms';
import { finalize } from 'rxjs';
import { AccountDTO, AccountService } from '../../../services/AccountService/account-service.service';
import { TransactionDTO, TransactionService } from '../../../services/TransactionService/transaction.service';

@Component({
  selector: 'app-view-transaction',
  standalone: true,
  templateUrl: './view-transaction.component.html',
  styleUrls: ['./view-transaction.component.css'],
  imports: [CommonModule, ReactiveFormsModule,FormsModule ]
})
export class ViewTransactionComponent implements OnInit {
  private fb = inject(FormBuilder);
  private accountsApi = inject(AccountService);
  private txApi = inject(TransactionService);
  private platformId = inject(PLATFORM_ID);

  accounts: AccountDTO[] = [];
  accountsLoading = false;
  accountsError: string | null = null;

  transactions: TransactionDTO[] = [];
  txLoading = false;
  txError: string | null = null;

  userId: number | null = null;

  form = this.fb.group({
    accountNumber: [null as number | null]
  });
  page = 1;
  pageSize = 10;

get totalPages(): number {
  return Math.max(1, Math.ceil(this.transactions.length / this.pageSize));
}

setPage(p: number) {
  if (p < 1 || p > this.totalPages) return;
  this.page = p;
}

get pagedTransactions() {
  const start = (this.page - 1) * this.pageSize;
  return this.transactions.slice(start, start + this.pageSize);
}



  ngOnInit(): void {
    if (isPlatformBrowser(this.platformId)) {
      const raw = localStorage.getItem('UserDetails');
      if (raw) {
        try {
          const u = JSON.parse(raw);
          if (u && typeof u.userId === 'number') this.userId = u.userId;
        } catch {}
      }
      this.loadAccounts();
    }

    this.form.get('accountNumber')?.valueChanges.subscribe(val => {
      const acc = Number(val);
      if (!isNaN(acc) && acc > 0) {
        this.loadTransactions(acc);
      } else {
        this.transactions = [];
      }
    });
  }

  private loadAccounts() {
    if (this.userId == null) return;
    this.accountsLoading = true;
    this.accountsError = null;

    this.accountsApi.getAccountsByUserId(this.userId)
      .pipe(finalize(() => this.accountsLoading = false))
      .subscribe({
        next: list => {
          this.accounts = Array.isArray(list) ? list : [];
          if (this.accounts.length) {
            const first = this.accounts[0].accountNumber;
            this.form.patchValue({ accountNumber: first }, { emitEvent: true });
          }
        },
        error: err => {
          this.accounts = [];
          this.accountsError = err?.error?.message || err?.message || 'Failed to load accounts';
        }
      });
  }

  private loadTransactions(accountNumber: number) {
    this.txLoading = true;
    this.txError = null;

    this.txApi.getTransactionsByAccount(accountNumber)
      .pipe(finalize(() => this.txLoading = false))
      .subscribe({
        next: list => this.transactions = Array.isArray(list) ? list : [],
        error: err => {
          this.transactions = [];
          this.txError = err?.error?.message || err?.message || 'Failed to load transactions';
        }
      });
  }

isCredit(row: TransactionDTO, selectedAcc: number): boolean {
  return row.toAccount === Number(selectedAcc);
}

isDebit(row: TransactionDTO, selectedAcc: number): boolean {
  return row.fromAccount === Number(selectedAcc);
}
}
