import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IRegister } from '../../../Interfaces/IRegister';
import { IUser } from '../../../Interfaces/IUser';

export interface AccountActivityDto {
  lastTransactionAt: string | null; 
  accountNumber: number;
  balance: number;
  accountTypeName: string;
}

export interface UserActivityDto {
  userId: number;
  userName: string;
  email: string;
  customerType: string;
  lastLoginAt: string; 
  isVerified:boolean;
  accountsList: AccountActivityDto[];
}


@Injectable({ providedIn: 'root' })
export class ManagerService {
  private http = inject(HttpClient);
  private base = 'http://localhost:5139/api/v1/Manager';

  createStaff(registerDTO: IRegister): Observable<string> {
    return this.http.post(`${this.base}/create-staff`, registerDTO, { responseType: 'text' });
  }

  getAllStaff(): Observable<IUser[]> {
    return this.http.get<IUser[]>(`${this.base}/GetAllStaff`); 
  }
    getAllUsersActivity(): Observable<UserActivityDto[]> {
    return this.http.get<UserActivityDto[]>(`${this.base}/GetAllUserActivity`);
  }
}
