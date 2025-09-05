import { Component } from '@angular/core';
import { AuthService } from '../../services/AuthService/auth.service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-login',
  imports: [FormsModule,RouterLink],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  username = ''
  password = ''

  constructor(private authService: AuthService, private router: Router) {}

onLogin() {
  debugger
  this.authService.login(this.username, this.password).subscribe({
    next: (res) => {
      localStorage.setItem('token', res.token)
      localStorage.setItem('UserDetails', JSON.stringify(res.user))
      this.authService.SetUser()
      Swal.fire({
        icon: 'success',
        title: 'Login Successful',
        text: 'Proceed with OTP verification',
        confirmButtonColor: '#0d6efd'
      }).then(() => {
        this.router.navigate(['/otpVerification'])
      })
    },
    error: (err) => {
      Swal.fire({
        icon: 'error',
        title: 'Login Failed',
        text: err.error?.message || 'Invalid username or password',
        confirmButtonColor: '#dc3545'
      })
    }
  })
}
}
