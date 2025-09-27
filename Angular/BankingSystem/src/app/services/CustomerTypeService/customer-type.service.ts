import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ICustomerType } from '../../../Interfaces/ICustomerType';
import { environment } from '../../../environments/environment';



@Injectable({
  providedIn: 'root'
})
export class CustomerService {
  

  private BaseUrl = environment.apiUrl;

  private apiUrl = `${this.BaseUrl}/CustomerType`; 

  constructor(private http: HttpClient) {}

  getCustomerTypes(): Observable<ICustomerType[]> {
    return this.http.get<ICustomerType[]>(this.apiUrl);
  }
}
