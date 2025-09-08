import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-otpverfication-required',
  imports: [],
  templateUrl: './otpverfication-required.component.html',
  styleUrl: './otpverfication-required.component.css'
})
export class OTPVerficationRequiredComponent {
  constructor(private router: Router) {}

  back() {
    this.router.navigate(['/otpVerification']);
  }

  login() {
    this.router.navigate(['/login']);
  }
}
