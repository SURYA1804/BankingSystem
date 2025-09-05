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

export const routes: Routes = [
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
  children: [
    { path: 'dashboard', component: CustomerDashboardComponent },
    { path: 'account', component: AccountManagementComponent },
    { path: 'loan', component: LoanApplicationComponent },
    { path: 'support', component: CustomerSupportComponent },
    { path: 'transaction', component: MakeTransactionComponent },  
    {path:'ViewTransaction',component:ViewTransactionComponent},
    { path: '', redirectTo: 'dashboard', pathMatch: 'full' }
  ]
}
];
