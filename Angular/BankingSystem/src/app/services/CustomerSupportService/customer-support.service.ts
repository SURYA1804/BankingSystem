import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

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
  private base = 'http://localhost:5139/api/v1/CustomerSupport';

  createQuery(dto: RaiseTicketDTO): Observable<string> {
    return this.http.post(`${this.base}/CreateQuery`, dto, { responseType: 'text' });
  }

  getAllQueriesByUser(userId: number): Observable<CustomerQueryDTO[]> {
    const params = new HttpParams().set('UserId', String(userId));
    return this.http.get<CustomerQueryDTO[]>(`${this.base}/GetAllQueriesByUser`, { params });
  }
  addComment(dto: { queryId: number; comments: string; isStaff: boolean }) {
  return this.http.post<CustomerQueryDTO>(`${this.base}/AddComment`, dto);
}
  getAllPendingQueries() {
    return this.http.get<CustomerQueryDTO[]>(`${this.base}/GetAllPendingQueries`);
  }
    markQueryClosed(queryId: number, staffId: number) {
    const params = new HttpParams()
      .set('queryId', String(queryId))
      .set('staffId', String(staffId));
    return this.http.post(`${this.base}/MarkQueryClosed`, null, { responseType: 'text', params });
  }

}
