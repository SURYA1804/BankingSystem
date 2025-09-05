import { ChangeDetectorRef, Component, OnInit, PLATFORM_ID, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Observable, finalize } from 'rxjs';
import Swal from 'sweetalert2';
import { ReferenceDataService } from '../../../services/referencedata/reference-data.service';
import { AccountDTO, AccountService } from '../../../services/AccountService/account-service.service';
import { IAccountType } from '../../../../Interfaces/IAccountType';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatExpansionModule } from '@angular/material/expansion';
import { IUser } from '../../../../Interfaces/IUser';

@Component({
  selector: 'app-account-management',
  standalone: true,
  templateUrl: './account-management.component.html',
  styleUrls: ['./account-management.component.css'],
  imports:[CommonModule, FormsModule, ReactiveFormsModule,MatFormFieldModule,MatSelectModule]
})
export class AccountManagementComponent implements OnInit {
  private fb = inject(FormBuilder);
  private ref = inject(ReferenceDataService);
  private api = inject(AccountService);
  private cdr = inject(ChangeDetectorRef);
  private platformId = inject(PLATFORM_ID);

  user: IUser | null = null;
  types$!: Observable<IAccountType[]>;
  AccountTypes: IAccountType[] = [];
  accounts: AccountDTO[] = [];
  accountsLoading = false;
  accountsError: string | null = null;
  creating = false;
  closing = false;
  changing = false;

  createForm = this.fb.group({
    accountType: ['', Validators.required],
    initialDeposit: [0, [Validators.required, Validators.min(0)]]
  });

  closeForm = this.fb.group({
    accountNumber: ['', Validators.required],
    reason: ['']
  });

  changeTypeForm = this.fb.group({
    accountNumber: ['', Validators.required],
    newAccountType: ['', Validators.required]
  });


  ngOnInit(): void {
    this.setUserFromStorage();
    this.loadUserAccounts();
    this.types$ = this.ref.getAccountTypes();
    this.types$.subscribe(types => {
      this.AccountTypes = types;
      this.cdr.detectChanges();
      console.log(this.AccountTypes)
    });
  }

iconFor(type?: string): string {
  const t = (type || '').toLowerCase();
  if (t.includes('saving')) return 'bi-piggy-bank';
  if (t.includes('current')) return 'bi-cash-coin';
  if (t.includes('salary')) return 'bi-briefcase';
  if (t.includes('joint')) return 'bi-people';
  return 'bi-credit-card';
}

  private setUserFromStorage() {
    const raw = localStorage.getItem('UserDetails');
    if (!raw) { this.user = null; return; }
    try {
      const parsed = JSON.parse(raw);
      this.user = parsed && typeof parsed.userId === 'number' ? parsed : null;
    } catch {
      this.user = null;
    }
  }


  private loadUserAccounts() {
    if (!this.user?.userId) return;
    this.accountsLoading = true;
    this.accountsError = null;

    this.api.getAccountsByUserId(this.user.userId)
      .pipe(finalize(() => this.accountsLoading = false))
      .subscribe({
        next: (list) => this.accounts = Array.isArray(list) ? list : [],
        error: (err) => {
          this.accounts = [];
          this.accountsError = err?.error?.message || err?.message || 'Failed to load accounts';
        }
      });
  }

  private success(msg: string) {
    Swal.fire({ icon: 'success', title: 'Success', text: msg, timer: 1800, showConfirmButton: false });
  }

  private error(msg: string) {
    Swal.fire({ icon: 'error', title: 'Error', text: msg });
  }

  submitCreate() {
    debugger
    if (this.createForm.invalid || !this.user) { this.error('Missing or invalid data'); return; }
    this.creating = true;
    const payload = {
      userId: this.user.userId,
      accountType: this.createForm.value.accountType!,
      OpeningBalance: Number(this.createForm.value.initialDeposit || 99999)
    };
    this.api.createAccount(payload).pipe(finalize(() => this.creating = false))
      .subscribe({
        next: res => { this.success(res?.message || 'Account created'); this.createForm.reset({ accountType: '', initialDeposit: 0 }); },
        error: err => this.error(err?.error?.message || 'Failed to create account')
      });
  }

  submitClose() {
    if (this.closeForm.invalid) { this.error('Provide a valid account number'); return; }
    this.closing = true;
    const accountNumber = Number(this.closeForm.value.accountNumber);
    this.api.closeAccount(accountNumber).pipe(finalize(() => this.closing = false))
      .subscribe({
        next: res => { this.success(res?.message || 'Account closed'); this.closeForm.reset({ accountNumber: '', reason: '' }); },
        error: err => this.error(err?.error?.message || 'Failed to close account')
      });
  }

  submitChangeType() {
    if (this.changeTypeForm.invalid || !this.user) { this.error('Missing or invalid data'); return; }
    this.changing = true;
    const v = this.changeTypeForm.value;
    this.api.requestTypeChange(Number(v.accountNumber), String(v.newAccountType), this.user.userId)
      .pipe(finalize(() => this.changing = false))
      .subscribe({
        next: res => { this.success(typeof res === 'string' ? res : 'Account type change submitted'); this.changeTypeForm.reset({ accountNumber: '', newAccountType: '' }); },
        error: err => this.error(err?.error?.message || 'Failed to change type')
      });
  }
}
