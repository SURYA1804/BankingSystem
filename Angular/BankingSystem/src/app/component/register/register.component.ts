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
    this.customerService.getCustomerTypes().subscribe({
      next: (res) => this.customerTypes = res,
      error: (err) => console.error('Failed to fetch customer types', err)
    });
  }

  onRegister() {
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
          text: err.error.message || 'Something went wrong!',
        });
      }
    });
  }
}
