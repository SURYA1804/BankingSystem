import { Component, OnInit } from '@angular/core';
import Swal from 'sweetalert2';
import { CommonModule } from '@angular/common';
import { LoanDTO, LoanService } from '../../../services/LoanService/loan.service';

@Component({
  selector: 'app-loan-support',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './loan-support.component.html',
  styleUrls: ['./loan-support.component.css']
})
export class LoanSupportComponent implements OnInit {
  loans: LoanDTO[] = [];
  loading = false;
  error: string | null = null;

  currentPage = 1;
  pageSize = 5;

  constructor(private loanService: LoanService) {}

  ngOnInit() {
    this.fetchLoans();
  }

  fetchLoans() {
    this.loading = true;
    this.loanService.getAllLoans().subscribe({
      next: (res) => {
        this.loans = res ?? [];
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load loans';
        this.loading = false;
      }
    });
  }

  pagedLoans() {
    const start = (this.currentPage - 1) * this.pageSize;
    return this.loans.slice(start, start + this.pageSize);
  }

  totalPages() {
    return Math.ceil(this.loans.length / this.pageSize);
  }

  setPage(page: number) {
    if (page < 1 || page > this.totalPages()) return;
    this.currentPage = page;
  }

  openLoanDetails(loan: LoanDTO) {
    Swal.fire({
      title: `Loan #${loan.id} Details`,
      html: `
        <p><strong>User:</strong> ${loan.userName || 'N/A'}</p>
        <p><strong>Loan Type:</strong> ${loan.loanTypeName || 'N/A'}</p>
        <p><strong>Loan Amount:</strong> ${loan.loanAmount}</p>
        <p><strong>Requested At:</strong> ${new Date(loan.createdAt).toLocaleString()}</p>
        <p><strong>Approved:</strong> ${loan.isApproved ? 'Yes' : 'No'}</p>
        <p><strong>Approved At:</strong> ${loan.approvedAt ? new Date(loan.approvedAt).toLocaleString() : 'N/A'}</p>
        <p><strong>Approved By (ID):</strong> ${loan.approvedBy}</p>
        <p><strong>Current Salary (LPA):</strong> ${loan.currentSalaryInLPA}</p>
        <p><strong>Customer Type:</strong> ${loan.customerType || 'N/A'}</p>
        <p><strong>Employment Status:</strong> ${loan.isEmployed ? 'Employed' : 'Unemployed'}</p>
        <p><strong>Reason:</strong> ${loan.rejectionReason}</p>
        <p><strong>Processed:</strong> ${loan.isProcessed ? 'Yes' : 'No'}</p>
      `,
      confirmButtonText: 'Close'
    });
  }
}
