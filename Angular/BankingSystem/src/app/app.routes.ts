import { Routes } from '@angular/router';
import { LandingPageComponent } from './component/landing-page/landing-page.component';
import { LoginComponent } from './component/login/login.component';
import { RegisterComponent } from './component/register/register.component';
import { PublicLayoutComponent } from './component/layouts/public-layout/public-layout.component';
import { OtpVerificationComponent } from './component/otp-verification/otp-verification.component';
import { CustomerLayoutComponent } from './component/layouts/customer-layout/customer-layout.component';
import { CustomerDashboardComponent } from './component/customer/customer-dashboard/customer-dashboard.component';
import { AccountManagementComponent } from './component/customer/account-management/account-management.component';
import { LoanApplicationComponent } from './component/customer/loan-application/loan-application.component';
import { CustomerSupportComponent } from './component/customer/customer-support/customer-support.component';
import { MakeTransactionComponent } from './component/customer/make-transaction/make-transaction.component';
import { ViewTransactionComponent } from './component/customer/view-transaction/view-transaction.component';
import { StaffDashboardComponent } from './component/staff/staff-dashboard/staff-dashboard.component';
import { TransactionApprovalComponent } from './component/staff/transaction-approval/transaction-approval.component';
import { AccountUpdateApprovalComponent } from './component/staff/account-update-approval/account-update-approval.component';
import { StafflayoutComponent } from './component/layouts/stafflayout/stafflayout.component';
import { LoanApprovalComponent } from './component/staff/loan-approval/loan-approval.component';
import { StaffCustomerSupportComponent } from './component/staff/staff-customer-support/staff-customer-support.component';
import { UnauthorizedComponent } from './component/un-authorized/un-authorized.component';
import { RoleGuard } from './component/guards/role.guard';
import { ManagerLayoutComponent } from './component/layouts/manager-layout/manager-layout.component';
import { ManagerDashboardComponent } from './component/Manager/dashboard/dashboard.component';
import { LoanSupportComponent } from './component/Manager/loan-support/loan-support.component';
import { UserActivityComponent } from './component/Manager/user-activity/user-activity.component';
import { CreateStaffComponent } from './component/Manager/create-staff/create-staff.component';
import { UpdateProfileComponent } from './component/customer/update-profile/update-profile.component';
import { Optional } from '@angular/core';
import { OTPVerficationRequiredComponent } from './component/otpverfication-required/otpverfication-required.component';

export const routes: Routes = [
    { path: 'unauthorized', component: UnauthorizedComponent },
    { path: 'OtpRequired', component: OTPVerficationRequiredComponent },
    
  {
    path: '',
    component: PublicLayoutComponent,
    children: [
      { path: '', component: LandingPageComponent },
      { path: 'login', component: LoginComponent },
      { path: 'register', component: RegisterComponent },
      { path: 'otpVerification', component: OtpVerificationComponent }
    ]
  },
  {
  path: 'customer',
  component: CustomerLayoutComponent,
  canActivate: [RoleGuard],
  data: { roles: ['customer'] },
  children: [
    { path: 'dashboard', component: CustomerDashboardComponent },
    { path: 'account', component: AccountManagementComponent },
    { path: 'loan', component: LoanApplicationComponent },
    { path: 'support', component: CustomerSupportComponent },
    { path: 'transaction', component: MakeTransactionComponent },  
    {path:'ViewTransaction',component:ViewTransactionComponent},
    {path:'UpdateProfile',component:UpdateProfileComponent},
    { path: '', redirectTo: 'dashboard', pathMatch: 'full' }
  ]
},
  {
  path: 'staff',
  component: StafflayoutComponent,
  canActivate: [RoleGuard],
  data: { roles: ['staff'] },
  children: [
    { path: 'dashboard', component: StaffDashboardComponent },
    { path: 'CustomerSupport', component: StaffCustomerSupportComponent },
    { path: 'LoanApproval', component:  LoanApprovalComponent},
    { path: 'TransactionApproval', component: TransactionApprovalComponent },
    { path: 'AccountUpdateTicket', component: AccountUpdateApprovalComponent },  
    { path: '', redirectTo: 'dashboard', pathMatch: 'full' }
  ]
},
  {
  path: 'manager',
  component: ManagerLayoutComponent,
  canActivate: [RoleGuard],
  data: { roles: ['Manager'] }, 
  children: [
    { path: 'dashboard', component: ManagerDashboardComponent },
    { path: 'LoanSupport', component: LoanSupportComponent },
    { path: 'UserActivity', component:  UserActivityComponent},
    { path: 'CreateStaff', component: CreateStaffComponent },
    { path: '', redirectTo: 'dashboard', pathMatch: 'full' }
  ]
}
];
