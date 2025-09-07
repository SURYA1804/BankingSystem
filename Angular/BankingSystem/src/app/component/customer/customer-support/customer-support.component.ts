import { Component, OnInit, inject, PLATFORM_ID, ChangeDetectorRef, NgZone } from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { finalize } from 'rxjs';
import Swal from 'sweetalert2';
import { CustomerQueryDTO, CustomerSupportService } from '../../../services/CustomerSupportService/customer-support.service';
import { PRIORITIES, QUERY_TYPES } from '../../../../Interfaces/Lookup';
import { TicketFilterPipe } from '../../../pipe/filter-ticket.pipe';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';

@Component({
  selector: 'app-customer-support',
  standalone: true,
  templateUrl: './customer-support.component.html',
  styleUrls: ['./customer-support.component.css'],
  imports: [CommonModule, ReactiveFormsModule,FormsModule ,TicketFilterPipe,MatFormFieldModule,MatSelectModule]
})
export class CustomerSupportComponent implements OnInit {
  private fb = inject(FormBuilder);
  private api = inject(CustomerSupportService);
  private platformId = inject(PLATFORM_ID);
  private cdr = inject(ChangeDetectorRef);
  private zone = inject(NgZone);

  userId: number | null = null;

  tickets: CustomerQueryDTO[] = [];
  loading = false;
  error: string | null = null;

  selecting = false;
  selected: CustomerQueryDTO | null = null;

  creating = false;

  ticketTypes = QUERY_TYPES;
  priorities = PRIORITIES;

  selectedTypePriorityName: string | null = null;

  ticketQuery = '';
  draftMessage = '';

  createForm = this.fb.group({
    ticketTypeId: [null as number | null, Validators.required],
    message: ['', [Validators.required, Validators.minLength(5)]]
  });

  ngOnInit(): void {
    if (isPlatformBrowser(this.platformId)) {
      const raw = localStorage.getItem('UserDetails');
      if (raw) {
        try {
          const u = JSON.parse(raw);
          if (u && typeof u.userId === 'number') this.userId = u.userId;
        } catch {}
      }
    }
    this.createForm.get('ticketTypeId')?.valueChanges.subscribe(val => {
      const t = this.ticketTypes.find(x => x.id === Number(val));
      this.selectedTypePriorityName = t ? (this.priorities.find(p => p.id === t.priorityId)?.name || null) : null;
    });
    this.loadTickets();
  }

  loadTickets() {
    if (this.userId == null) return;
    this.loading = true;
    this.error = null;
    this.api.getAllQueriesByUser(this.userId)
      .pipe(finalize(() => this.loading = false))
      .subscribe({
        next: list => {
          this.tickets = Array.isArray(list) ? list : [];
          if (this.selected) {
            const match = this.tickets.find(t => t.customerQueryId === this.selected!.customerQueryId);
            this.selected = match || null;
          }
        },
        error: err => {
          this.tickets = [];
          this.error = err?.error || err?.message || 'Failed to load tickets';
        }
      });
  }

  selectTicket(t: CustomerQueryDTO) {
    this.selecting = true;
    this.selected = t;
    setTimeout(() => this.selecting = false);
  }

  submitCreate() {
    if (this.createForm.invalid || this.userId == null) {
      Swal.fire({ icon: 'error', title: 'Error', text: 'Fill all fields; user not found' });
      return;
    }
    this.creating = true;
    const dto = {
      userId: this.userId,
      ticketTypeId: Number(this.createForm.value.ticketTypeId),
      message: String(this.createForm.value.message).trim()
    };
    this.api.createQuery(dto)
      .pipe(finalize(() => this.creating = false))
      .subscribe({
        next: res => {
          const text = typeof res === 'string' ? res : 'Ticket Created';
          Swal.fire({ icon: 'success', title: 'Success', text, timer: 1600, showConfirmButton: false });
          this.createForm.reset();
          this.selectedTypePriorityName = null;
          this.loadTickets();
        },
        error: err => {
          const text = err?.error || err?.message || 'Not Created';
          Swal.fire({ icon: 'error', title: 'Error', text });
        }
      });
  }

  statusBadgeClasses(t: CustomerQueryDTO) {
    if (t.isSolved) return 'bg-success';
    if ((t.status || '').toLowerCase() === 'pending') return 'bg-warning text-dark';
    return 'bg-secondary';
  }

  priorityBadgeClassesFromName(name: string) {
    const n = (name || '').toLowerCase();
    if (n === 'urgent') return 'bg-danger';
    if (n === 'high') return 'bg-danger';
    if (n === 'medium') return 'bg-warning text-dark';
    return 'bg-secondary';
  }

async openChat(t: CustomerQueryDTO) {
  this.selectTicket(t);
  if (typeof window === 'undefined') return;
  const Offcanvas = (await import('bootstrap/js/dist/offcanvas')).default as any;
  const el = document.getElementById('chatOffcanvas');
  if (!el) return;
  const oc = Offcanvas.getInstance?.(el) || new Offcanvas(el, { scroll: true, backdrop: true });
  oc.show();
  setTimeout(() => {
    const focusTarget = el.querySelector<HTMLElement>('button, [href], input, [tabindex]:not([tabindex="-1"])');
    focusTarget?.focus();
  }, 0);
}


async closeChat() {
  if (typeof window === 'undefined') return;
  const Offcanvas = (await import('bootstrap/js/dist/offcanvas')).default as any;
  const el = document.getElementById('chatOffcanvas');
  if (!el) return;
  const oc = Offcanvas.getInstance?.(el) || new Offcanvas(el);
  oc.hide();
  if (document.activeElement instanceof HTMLElement) document.activeElement.blur();
}




sendDraft() {
  const msg = this.draftMessage?.trim();
  if (!msg || !this.selected || this.selected.isSolved) return;

  const dto = { queryId: this.selected.customerQueryId, comments: msg, isStaff: false };
  const id = this.selected.customerQueryId;
  this.draftMessage = '';

  this.api.addComment(dto).subscribe({
    next: updated => {
      if (!updated) return;

      Swal.fire({
        icon: 'success',
        title: 'Comment sent',
        text: 'Your reply has been added.',
        timer: 1400,
        showConfirmButton: false
      }).then(() => {
        this.loadTickets();
        this.reloadRoute();
      });

    },
    error: err => {
      const text = err?.error || err?.message || 'Failed to add comment';
      Swal.fire({ icon: 'error', title: 'Error', text });
    }
  });
}
reloadRoute() {
window.location.reload();
}

}
