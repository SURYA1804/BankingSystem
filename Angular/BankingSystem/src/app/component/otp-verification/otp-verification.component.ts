import { Component, ViewChildren, QueryList, ElementRef, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { AuthService } from '../../services/AuthService/auth.service';
import Swal from 'sweetalert2';

interface IUser {
  name: string;
  email: string;
  roleName: string; // Make sure backend returns this
  [key: string]: any;
}

@Component({
  selector: 'app-otp-verification',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './otp-verification.component.html',
  styleUrls: ['./otp-verification.component.css']
})
export class OtpVerificationComponent implements OnInit {
  otpDigits: string[] = ['', '', '', '', '', ''];
  @ViewChildren('otpInput') otpInputs!: QueryList<ElementRef>;

  user!: IUser;

  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit() {
    const userData = localStorage.getItem('UserDetails');
    if (!userData) {
      Swal.fire({
        icon: 'error',
        title: 'Session Expired',
        text: 'Please login again.',
        confirmButtonColor: '#dc3545'
      }).then(() => this.router.navigate(['/login']));
      return;
    }
    this.user = JSON.parse(userData);
  }

  handleInput(index: number, event: Event) {
    const input = event.target as HTMLInputElement;
    const value = input.value;

    if (!/^[0-9]$/.test(value)) {
      input.value = '';
      this.otpDigits[index] = '';
      return;
    }

    this.otpDigits[index] = value;

    if (index < this.otpDigits.length - 1) {
      this.otpInputs.toArray()[index + 1].nativeElement.focus();
    } else {
      this.onVerify();
    }
  }

  handleBackspace(index: number, event: KeyboardEvent) {
    if (event.key === 'Backspace' && !this.otpDigits[index] && index > 0) {
      this.otpInputs.toArray()[index - 1].nativeElement.focus();
    }
  }

  handlePaste(event: ClipboardEvent) {
    event.preventDefault();
    const pastedData = event.clipboardData?.getData('text') ?? '';
    if (/^\d{6}$/.test(pastedData)) {
      this.otpDigits = pastedData.split('');
      this.otpInputs.toArray().forEach((el, i) => el.nativeElement.value = this.otpDigits[i]);
      this.onVerify();
    } else {
      Swal.fire({
        icon: 'error',
        title: 'Invalid Paste',
        text: 'Please paste a 6-digit numeric OTP only.',
        confirmButtonColor: '#0d6efd'
      });
    }
  }

  onVerify() {
    const otp = this.otpDigits.join('');
    if (otp.length !== 6) {
      Swal.fire({
        icon: 'error',
        title: 'Invalid OTP',
        text: 'OTP must be 6 digits.',
        confirmButtonColor: '#0d6efd'
      });
      return;
    }

    this.authService.verifyOtp(Number(otp)).subscribe({
      next: res => {
        Swal.fire({
          icon: 'success',
          title: 'Verified',
          text: res.message,
          confirmButtonColor: '#0d6efd'
        }).then(() => {
          switch (this.user.roleName) {
            case 'admin':
              this.router.navigate(['/admin-dashboard']);
              break;
            case 'customer':
              this.router.navigate(['/customer-dashboard']);
              break;
            case 'staff':
              this.router.navigate(['/staff-dashboard']);
              break;
            default:
              this.router.navigate(['/dashboard']);
          }
        });
      },
      error: err => {
        Swal.fire({
          icon: 'error',
          title: 'Verification Failed',
          text: err.error?.message || 'Something went wrong',
          confirmButtonColor: '#dc3545'
        });
      }
    });
  }

  onResendOtp() {
    this.authService.resendOtp().subscribe({
      next: res => {
        Swal.fire({
          icon: 'success',
          title: 'OTP Sent',
          text: res,
          confirmButtonColor: '#0d6efd'
        });
      },
      error: err => {
        Swal.fire({
          icon: 'error',
          title: 'Error',
          text: err.error?.message || 'Something went wrong',
          confirmButtonColor: '#dc3545'
        });
      }
    });
  }
}
