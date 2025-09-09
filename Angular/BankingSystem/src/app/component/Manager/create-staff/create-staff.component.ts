import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import Swal from 'sweetalert2';
import { IUser } from '../../../../Interfaces/IUser';
import { ManagerService } from '../../../services/Manager/manager.service';
import { IRegister } from '../../../../Interfaces/IRegister';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-create-staff',
  standalone: true,
  imports: [FormsModule,ReactiveFormsModule,CommonModule],
  templateUrl: './create-staff.component.html',
  styleUrls: ['./create-staff.component.css']
})
export class CreateStaffComponent implements OnInit {
  staffForm: FormGroup;
  maxDate: string ='';
  staffs: IUser[] = [];
  loadingStaffs = false;
  creating = false;

  constructor(private managerService: ManagerService, private fb: FormBuilder) {
    this.staffForm = this.fb.group({
      name: ['',[
        Validators.required,
        Validators.minLength(3),
        Validators.pattern('^[a-zA-Z0-9 ]+$') 
      ]],
      userName: ['', [
        Validators.required,
        Validators.minLength(3),
        Validators.pattern('^[a-zA-Z0-9]+$') 
      ]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [
        Validators.required,
        Validators.minLength(8)
      ]],
      age: ['', [Validators.required, Validators.min(18)]],
      dob: ['', Validators.required],
      address: [''],
      phoneNumber: ['', [
        Validators.required,
        Validators.pattern('^[0-9]{10}$') 
      ]]
    });
  }
showCreateForm = false;

toggleCreateForm() {
  this.showCreateForm = !this.showCreateForm;
}

  ngOnInit(): void {
    const today = new Date();
  this.maxDate = today.toISOString().split('T')[0];
    this.fetchStaffs();
  }

  fetchStaffs() {
    this.loadingStaffs = true;
    this.managerService.getAllStaff().subscribe({
      next: data => { this.staffs = data; this.loadingStaffs = false; },
      error: () => { this.loadingStaffs = false; }
    });
  }

  submitCreate() {
    if (this.staffForm.invalid) return;
    const dto: IRegister = this.staffForm.value;
    dto.isEmployed = true;
    dto.customerType = 6;
    this.creating = true;
    this.managerService.createStaff(dto).subscribe({
      next: () => {
        this.creating = false;
        Swal.fire('Success', 'Staff created successfully.', 'success');
        this.staffForm.reset();
        this.fetchStaffs();
      },
      error: () => {
        this.creating = false;
        Swal.fire('Error', 'Error creating staff.', 'error');
      }
    });
  }

  

}
