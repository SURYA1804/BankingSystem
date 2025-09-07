import {
  Component,
  OnInit,
  ViewChild,
  ElementRef,
  ChangeDetectorRef,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import Swal from 'sweetalert2';
import {
  CustomerSupportService,
  CustomerQueryDTO,
  QueryCommentsDTO,
} from '../../../services/CustomerSupportService/customer-support.service';

@Component({
  selector: 'app-staff-customer-support',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './staff-customer-support.component.html',
  styleUrls: ['./staff-customer-support.component.css'],
})
export class StaffCustomerSupportComponent implements OnInit {
  @ViewChild('chatBody') private chatBody!: ElementRef<HTMLDivElement>;

  tickets: CustomerQueryDTO[] = [];
  selected: CustomerQueryDTO | null = null;
  newMessage = '';
  loading = false;
  error: string | null = null;

  constructor(
    private svc: CustomerSupportService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.fetch();
  }

  get isComposerDisabled(): boolean {
    return !!this.selected?.isSolved;
  }

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

  fetch(): void {
    this.loading = true;
    this.error = null;
    this.svc.getAllPendingQueries().subscribe({
      next: (res) => {
        console.log('Fetched Tickets:', res);
        this.tickets = res ?? [];
        this.loading = false;
        if (this.tickets.length > 0 && !this.selected) {
          this.openTicket(this.tickets[0]); // ✅ FIXED
        }
      },
      error: (err) => {
        this.loading = false;
        this.error =
          typeof err?.error === 'string' && err.error
            ? err.error
            : 'Failed to load tickets';
        Swal.fire({
          icon: 'info',
          title: 'No pending tickets',
          text: this.error ?? '',
          confirmButtonColor: '#0d6efd',
        });
      },
    });
  }

  openTicket(ticket: CustomerQueryDTO): void {
    if (this.selected?.customerQueryId === ticket.customerQueryId) {
      this.selected = null;
      return;
    }

    this.selected = JSON.parse(JSON.stringify(ticket)) as CustomerQueryDTO;

    this.selected.comments = this.selected.comments ?? [];

    this.cdr.detectChanges(); 
    this.scrollToBottom();
  }

  sendMessage(): void {
    if (!this.newMessage?.trim() || !this.selected) return;

    const text = this.newMessage.trim();

    const optimistic: QueryCommentsDTO & { _temp?: boolean } = {
      comment: text,
      createdAt: new Date().toISOString(),
      isStaffComment: true,
      isUserComment: false,
      _temp: true,
    };

    this.selected.comments.push(optimistic);
    this.scrollToBottom();
    this.newMessage = '';

    this.svc
      .addComment({
        queryId: this.selected.customerQueryId,
        comments: text,
        isStaff: true,
      })
      .subscribe({
        next: (updated) => {
          if (updated && typeof updated === 'object') {
            this.selected = updated;
            this.tickets = this.tickets.map((t) =>
              t.customerQueryId === updated.customerQueryId ? updated : t
            );
          } else {
            this.fetch();
          }

          this.cdr.detectChanges(); 
          Swal.fire({
            icon: 'success',
            title: 'Message Sent',
            showConfirmButton: false,
            timer: 1200,
          }).then(() => {
            setTimeout(() => {
              window.location.reload();
            }, 10); 
          });
          this.scrollToBottom();

        },
        error: (e) => {
          if (this.selected) {
            this.selected.comments = this.selected.comments.filter(
              (c: any) => !c._temp
            );
          }
          Swal.fire({
            icon: 'error',
            title: 'Send failed',
            text:
              e?.error && typeof e.error === 'string'
                ? e.error
                : 'Failed to send message. Try again.',
            confirmButtonColor: '#d33',
          });
        },
      });
  }

  markClosed(): void {
    if (!this.selected) return;
    if (!this.staffId) {
      Swal.fire({
        icon: 'error',
        title: 'Missing Staff ID',
        text: 'Please login as staff.',
      });
      return;
    }

    this.svc
      .markQueryClosed(this.selected.customerQueryId, this.staffId)
      .subscribe({
        next: () => {
          Swal.fire({
            icon: 'success',
            title: 'Ticket Closed',
            text: `Ticket #${this.selected?.customerQueryId} has been closed.`,
            confirmButtonColor: '#0d6efd',
          });
          this.fetch();
          this.selected = null;
        },
        error: () => {
          Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'Failed to close ticket. Please try again.',
            confirmButtonColor: '#d33',
          });
        },
      });
  }

  private scrollToBottom(): void {
    setTimeout(() => {
      const el = this.chatBody?.nativeElement;
      if (el) {
        el.scrollTop = el.scrollHeight;
      }
    }, 100);
  }
}
