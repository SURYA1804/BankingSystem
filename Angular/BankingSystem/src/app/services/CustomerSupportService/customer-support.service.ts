import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface RaiseTicketDTO {
  userId: number;
  ticketTypeId: number;
  message: string;
}

export interface QueryCommentsDTO {
  CommentId:number;
  comment: string;
  createdAt: string;
  isStaffComment: boolean;
  isUserComment: boolean;
}

export interface CustomerQueryDTO {
  customerQueryId: number;
  userName: string;
  userId: number;
  queryType: string;
  status: string;
  priority: string;
  isSolved: boolean;
  createdAt: string;
  solvedAt?: string | null;
  comments: QueryCommentsDTO[];
}

@Injectable({ providedIn: 'root' })
export class CustomerSupportService {
  private http = inject(HttpClient);
  private BaseUrl = environment.apiUrl;

  private apiUrl = `${this.BaseUrl}/CustomerSupport`;

  createQuery(dto: RaiseTicketDTO): Observable<string> {
    return this.http.post(`${this.apiUrl}/CreateQuery`, dto, { responseType: 'text' });
  }

  getAllQueriesByUser(userId: number): Observable<CustomerQueryDTO[]> {
    const params = new HttpParams().set('UserId', String(userId));
    return this.http.get<CustomerQueryDTO[]>(`${this.apiUrl}/GetAllQueriesByUser`, { params });
  }
  addComment(dto: { queryId: number; comments: string; isStaff: boolean }) {
  return this.http.post<CustomerQueryDTO>(`${this.apiUrl}/AddComment`, dto);
}
  getAllPendingQueries() {
    return this.http.get<CustomerQueryDTO[]>(`${this.apiUrl}/GetAllPendingQueries`);
  }
    markQueryClosed(queryId: number, staffId: number) {
    const params = new HttpParams()
      .set('queryId', String(queryId))
      .set('staffId', String(staffId));
    return this.http.post(`${this.apiUrl}/MarkQueryClosed`, null, { responseType: 'text', params });
  }

}
