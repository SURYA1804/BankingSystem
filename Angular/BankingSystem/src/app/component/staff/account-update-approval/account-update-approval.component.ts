import { Component, OnInit } from '@angular/core';
import Swal from 'sweetalert2';
import { AccountService, AccountUpdateTicketDTO } from '../../../services/AccountService/account-service.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-account-update-approval',
  standalone: true, 
  templateUrl: './account-update-approval.component.html',
  styleUrls: ['./account-update-approval.component.css'],
  imports:[CommonModule]
})
export class AccountUpdateApprovalComponent implements OnInit {
  allTickets: AccountUpdateTicketDTO[] = [];
  pendingTickets: AccountUpdateTicketDTO[] = [];
  selectedTicket: AccountUpdateTicketDTO | null = null;

  loadingAll = false;
  loadingPending = false;
  error: string | null = null;

  constructor(private staff: AccountService) { }

  ngOnInit(): void {
    this.fetchAllTickets();
    this.fetchPendingTickets();
  }

allTicketsPage = 1;
allTicketsPageSize = 5;

setAllTicketsPage(page: number) {
  if (page < 1 || page > this.allTicketsTotalPages()) return;
  this.allTicketsPage = page;
}

allTicketsPages(): number[] {
  return Array.from({ length: this.allTicketsTotalPages() }, (_, i) => i + 1);
}

allTicketsTotalPages(): number {
  return Math.ceil(this.allTickets.length / this.allTicketsPageSize);
}

pagedAllTickets() {
  const start = (this.allTicketsPage - 1) * this.allTicketsPageSize;
  return this.allTickets.slice(start, start + this.allTicketsPageSize);
}

pendingTicketsPage = 1;
pendingTicketsPageSize = 5;

setPendingTicketsPage(page: number) {
  if (page < 1 || page > this.pendingTicketsTotalPages()) return;
  this.pendingTicketsPage = page;
}

pendingTicketsPages(): number[] {
  return Array.from({ length: this.pendingTicketsTotalPages() }, (_, i) => i + 1);
}

pendingTicketsTotalPages(): number {
  return Math.ceil(this.pendingTickets.length / this.pendingTicketsPageSize);
}

pagedPendingTickets() {
  const start = (this.pendingTicketsPage - 1) * this.pendingTicketsPageSize;
  return this.pendingTickets.slice(start, start + this.pendingTicketsPageSize);
}



  public get staffId(): number {
    const raw = localStorage.getItem('UserDetails');
    if (!raw) return 0;
    try {
      const user = JSON.parse(raw) as { userId?: number };
      return Number(user?.userId || 0);
    } catch {
      return 0;
    }
  }

  fetchAllTickets() {
    this.loadingAll = true;
    this.staff.getAllAccountUpdateTickets().subscribe({
      next: res => { this.allTickets = res ?? []; this.loadingAll = false; console.log(this.allTickets) },
      error: err => { this.error = 'Failed to load all tickets'; this.loadingAll = false; }
    });
  }

  fetchPendingTickets() {
    this.loadingPending = true;
    this.staff.getAllPendingAccountUpdateTickets().subscribe({
      next: res => { this.pendingTickets = res ?? []; this.loadingPending = false; },
      error: err => { this.error = 'Failed to load pending tickets'; this.loadingPending = false; }
    });
  }

  review(ticket: AccountUpdateTicketDTO) {
        if (ticket.isProcessed) {
    Swal.fire('Info', 'This ticket is already processed and cannot be modified.', 'info');
    return; 
  }
    Swal.fire({
      title: `Review Ticket #${ticket.ticketId}`,
      html: `
        <div><b>Requested By:</b> ${ticket.requestedBy}</div>
        <div><b>Account Number:</b> ${ticket.accountNumber}</div>
        <div><b>Requested Change:</b> ${ticket.requestedChange}</div>
        <div><b>Requested At:</b> ${new Date(ticket.requestedAt).toLocaleString()}</div>
        <div><b>Is Approved:</b> ${ticket.isApproved ? 'Yes' : 'No'}</div>
        <div><b>Is Processed:</b> ${ticket.isProcessed ? 'Yes' : 'No'}</div>
        ${ticket.approvedBy ? `<div><b>Approved By:</b> ${ticket.approvedBy}</div>` : ''}
        ${ticket.approvedAt ? `<div><b>Approved At:</b> ${new Date(ticket.approvedAt).toLocaleString()}</div>` : ''}
        ${ticket.rejectionReaosn ? `<div><b>Rejection Reason:</b> ${ticket.rejectionReaosn}</div>` : ''}
      `,
      showCancelButton: true,
      showDenyButton: true,
      confirmButtonText: 'Approve',
      denyButtonText: 'Reject',
      cancelButtonText: 'Cancel'
    }).then(result => {
      if (result.isConfirmed) {
        this.sendReview(ticket, 1, '');
      } else if (result.isDenied) {
        Swal.fire({
          title: 'Reason for rejection',
          input: 'text',
          inputPlaceholder: 'Enter reason',
          showCancelButton: true,
          confirmButtonText: 'Reject'
        }).then(reject => {
          if (reject.isConfirmed) {
            this.sendReview(ticket, 2, reject.value ?? '');
          }
        });
      }
    });
  }

  sendReview(ticket: AccountUpdateTicketDTO, action: number, reason: string) {
    const staffId = this.staffId;
    if (staffId === 0) {
      Swal.fire('Error', 'Staff ID not found. Please login again.', 'error');
      return;
    }


    Swal.fire({ title: 'Processing...', didOpen: () => Swal.showLoading(), allowOutsideClick: false });

    this.staff.reviewAccountTypeChange(ticket.ticketId, staffId, action, reason).subscribe({
      next: res => {
        Swal.close();
        if (action === 1) {
          Swal.fire('Approved!', res, 'success');
        } else {
          Swal.fire('Rejected!', res, 'info');
        }
        this.fetchAllTickets();
        this.fetchPendingTickets();
      },
      error: err => {
        Swal.close();
        Swal.fire('Error', typeof err?.error === 'string' ? err.error : 'Failed', 'error');
      }
    });
  }
}
