import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map, shareReplay } from 'rxjs';
import { IAccountType } from '../../../Interfaces/IAccountType';
import { environment } from '../../../environments/environment';


@Injectable({ providedIn: 'root' })
export class ReferenceDataService {
  private http = inject(HttpClient);
  private BaseUrl = environment.apiUrl;
  private apiUrl = `${this.BaseUrl}/AccountType`;
  private accountTypes$?: Observable<IAccountType[]>;

  getAccountTypes(): Observable<IAccountType[]> {
    if (!this.accountTypes$) {
      this.accountTypes$ = this.http.get<IAccountType[]>(`${this.apiUrl}/GetAllAccountType`)
    }
    return this.accountTypes$;
  }
}
