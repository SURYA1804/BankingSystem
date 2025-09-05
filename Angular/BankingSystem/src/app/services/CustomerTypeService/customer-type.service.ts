import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ICustomerType } from '../../../Interfaces/ICustomerType';



@Injectable({
  providedIn: 'root'
})
export class CustomerService {
  private apiUrl = 'http://localhost:5139/api/v1/CustomerType'; 

  constructor(private http: HttpClient) {}

  getCustomerTypes(): Observable<ICustomerType[]> {
    return this.http.get<ICustomerType[]>(this.apiUrl);
  }
}
