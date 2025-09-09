import { Component, Inject, OnInit, PLATFORM_ID } from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { UserService } from '../../../services/User/user.service';
import Swal from 'sweetalert2';
import { MatSelectChange, MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';

@Component({
  selector: 'app-update-profile',
  templateUrl: './update-profile.component.html',
  styleUrls: ['./update-profile.component.css'],
    imports: [CommonModule, ReactiveFormsModule ,MatSelectModule, MatFormFieldModule],
})
export class UpdateProfileComponent implements OnInit {
  updateForm!: FormGroup;
  userId!: number;
  selectedField: string | null = null;
  maxDate: string = '';

  constructor(
    @Inject(PLATFORM_ID) private platformId: Object,
    private fb: FormBuilder,
    private userService: UserService
  ) 
  {

  }

  ngOnInit(): void {
  const today = new Date();
  this.maxDate = today.toISOString().split('T')[0];
    if (isPlatformBrowser(this.platformId)) {
      const raw = localStorage.getItem('UserDetails');
      if (raw) {
        try {
          const user = JSON.parse(raw);
          if (user && typeof user.userId === 'number') {
            this.userId = user.userId;
          }
        } catch {
        }
      }
    }
    this.initForm();
  }

  private initForm() {
  this.updateForm = this.fb.group({
    name: [''],
    userName: [''],
    dob: [''],
    age: ['', [Validators.min(5)]],
    oldPassword: [''], 
     password: ['', [Validators.minLength(8)]],
    email: [''],
  phoneNumber: ['', [Validators.pattern('^[0-9]{10}$')]]
  });
  }
formatFieldName(field: string): string {
  switch (field) {
    case 'userName':
      return 'Username';
    case 'dob':
      return 'Date of Birth';
    case 'phoneNumber':
      return 'Phone Number';
    case 'oldPassword':
      return 'old Password';
    default:
      return field.charAt(0).toUpperCase() + field.slice(1);
  }
}

onFieldChange(event: MatSelectChange) {
  this.selectedField = event.value;

  Object.keys(this.updateForm.controls).forEach(field => {
    const control = this.updateForm.get(field);
    control?.clearValidators();
    control?.setValue('');
    control?.updateValueAndValidity();
  });

  if (!this.selectedField) return;

  const control = this.updateForm.get(this.selectedField);
  if (!control) return;

  const validators = [];

  if (['name', 'userName', 'dob', 'age', 'email', 'phoneNumber'].includes(this.selectedField)) {
    validators.push(Validators.required);
  }
  if (this.selectedField === 'email') {
    validators.push(Validators.email);
  }
  if(this.selectedField === 'phoneNumber')
  {
    validators.push(Validators.pattern('^[0-9]{10}$'));

  }
  if(this.selectedField === 'age')
  {
    validators.push(Validators.min(5));

  }
  if(this.selectedField === 'password')
  {
    validators.push(Validators.minLength(8));

  }

  control.setValidators(validators);
  control.updateValueAndValidity();
}



onSubmit() {
  if (!this.selectedField) {
    Swal.fire('Select a field', 'Please select which field you want to update.', 'warning');
    return;
  }

const control = this.updateForm.get(this.selectedField);
  if (!control || control.invalid) {
    let errorMessage = 'Invalid input. Please correct the error.';

    if (control?.errors) {
      if (control.errors['required']) {
        errorMessage = `${this.formatFieldName(this.selectedField)} is required.`;
      } else if (control.errors['email']) {
        errorMessage = 'Please enter a valid email address.';
      } else if (control.errors['minlength']) {
        const requiredLength = control.errors['minlength'].requiredLength;
        errorMessage = `Password must be at least ${requiredLength} characters long.`;
      } else if (control.errors['pattern']) {
        if (this.selectedField === 'phoneNumber') {
          errorMessage = 'Phone number must be exactly 10 digits.';
        } else {
          errorMessage = 'Invalid format.';
        }
      } else if (control.errors['min']) {
        const min = control.errors['min'].min;
        errorMessage = `Age must be at least ${min} years old.`;
      }
    }

    Swal.fire('Validation Error', errorMessage, 'error');
    return;
  }

  if (this.selectedField === 'password') {
    const oldPasswordControl = this.updateForm.get('oldPassword');
    if (!oldPasswordControl || !oldPasswordControl.value) {
      Swal.fire('Old Password Required', 'Please enter your old password to update.', 'warning');
      return;
    }

    this.userService.checkPassword(this.userId, oldPasswordControl.value).subscribe({
      next: (res) => {
        this.executePatchUpdate(control!.value);
      },
      error: (err) => {
        Swal.fire('Incorrect Password', 'The old password you entered is incorrect.', 'error');
      }
    });
  } else {
    this.executePatchUpdate(control.value);
  }
}

private executePatchUpdate(newValue: any) {
  const patchDoc = [
    {
      op: 'replace',
      path: this.toPatchPath(this.selectedField!),
      value: newValue
    }
  ];

  this.userService.updateUserPatch(this.userId, patchDoc).subscribe({
    next: () => Swal.fire('Updated', 'Profile updated successfully!', 'success'),
    error: err => Swal.fire('Error', err.message, 'error')
  });
}

  private toPatchPath(fieldName: string): string {
    switch (fieldName) {
      case 'userName': return '/UserName';
      case 'phoneNumber': return '/PhoneNumber';
      default: return '/' + this.capitalizeFirstLetter(fieldName);
    }
  }

  private capitalizeFirstLetter(text: string): string {
    return text.charAt(0).toUpperCase() + text.slice(1);
  }
}
