import { Injectable } from '@angular/core';
import { HttpClient }from '@angular/common/http';
import { Observable }from 'rxjs';
import { environment }from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class JobService {
  private apiUrl =
    `${environment.apiUrl}/jobs`;

  constructor(private http: HttpClient)
  {
  }

  getJobs(): Observable<any>
  {
    return this.http.get(
      this.apiUrl
    );
  }

  createJob(data: any): Observable<any>
  {
    return this.http.post(
      this.apiUrl,
      data
    );
  }
}