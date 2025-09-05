import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map, shareReplay } from 'rxjs';
import { IAccountType } from '../../../Interfaces/IAccountType';


@Injectable({ providedIn: 'root' })
export class ReferenceDataService {
  private http = inject(HttpClient);
  private base = 'http://localhost:5139/api/v1/AccountType';
  private accountTypes$?: Observable<IAccountType[]>;

  getAccountTypes(): Observable<IAccountType[]> {
    if (!this.accountTypes$) {
      this.accountTypes$ = this.http.get<IAccountType[]>(`${this.base}/GetAllAccountType`)
    }
    return this.accountTypes$;
  }
}
