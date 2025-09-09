import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/AuthService/auth.service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { IRegister } from '../../../Interfaces/IRegister';
import Swal from 'sweetalert2';
import { ICustomerType } from '../../../Interfaces/ICustomerType';
import { CustomerService } from '../../services/CustomerTypeService/customer-type.service';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    FormsModule,
    CommonModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule
  ],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  Register: IRegister;
  customerTypes: ICustomerType[] = [];
  maxDate: string ='';

  constructor(
    private authService: AuthService,
    private router: Router,
    private customerService: CustomerService
  ) {
    this.Register = {
      name: '',
      userName: '',
      email: '',
      password: '',
      customerType: 0,
      age: 0,
      dob: '',
      isEmployed: false,
      address: '',
      phoneNumber: ''
    };
  }

  ngOnInit() {
  const today = new Date();
  this.maxDate = today.toISOString().split('T')[0];
    this.customerService.getCustomerTypes().subscribe({
      next: (res) => this.customerTypes = res,
      error: (err) => console.error('Failed to fetch customer types', err)
    });
  }

  onRegister() {
    if (!this.Register.name || this.Register.name.trim().length < 3) {
    Swal.fire({
      icon: 'error',
      title: 'Invalid Name',
      text: 'Full Name must be at least 3 characters long',
    });
    return;
  }

  if (!this.Register.userName || this.Register.userName.trim().length < 3) {
    Swal.fire({
      icon: 'error',
      title: 'Invalid Username',
      text: 'Username must be at least 3 characters long',
    });
    return;
  }

  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  if (!this.Register.email || !emailRegex.test(this.Register.email)) {
    Swal.fire({
      icon: 'error',
      title: 'Invalid Email',
      text: 'Please enter a valid email address',
    });
    return;
  }

  if (!this.Register.password || this.Register.password.length < 8) {
    Swal.fire({
      icon: 'error',
      title: 'Weak Password',
      text: 'Password must be at least 6 characters long',
    });
    return;
  }

  const phoneRegex = /^[0-9]{10}$/;
  if (!this.Register.phoneNumber || !phoneRegex.test(this.Register.phoneNumber)) {
    Swal.fire({
      icon: 'error',
      title: 'Invalid Phone Number',
      text: 'Phone number must be exactly 10 digits',
    });
    return;
  }

  if (!this.Register.customerType) {
    Swal.fire({
      icon: 'error',
      title: 'Select Customer Type',
      text: 'Please choose a customer type',
    });
    return;
  }

  if (!this.Register.age || this.Register.age < 5) {
    Swal.fire({
      icon: 'error',
      title: 'Invalid Age',
      text: 'You must be at least 5 years old to register',
    });
    return;
  }

  if (!this.Register.dob) {
    Swal.fire({
      icon: 'error',
      title: 'Date of Birth Required',
      text: 'Please select your date of birth',
    });
    return;
  }
    this.authService.register(this.Register).subscribe({
      next: () => {
        Swal.fire({
          icon: 'success',
          title: 'Registration Successful',
          text: 'Please login to continue',
          timer: 2000,
          timerProgressBar: true,
          showConfirmButton: false
        }).then(() => {
          this.router.navigate(['/login']);
        });
      },
      error: err => {
        Swal.fire({
          icon: 'error',
          title: 'Registration Failed',
          text: err.error.message || 'Username Already Taken!',
        });
      }
    });
  }
}
