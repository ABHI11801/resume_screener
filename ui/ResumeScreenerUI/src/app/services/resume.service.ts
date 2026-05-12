import { Injectable }
from '@angular/core';

import { HttpClient }
from '@angular/common/http';

import { Observable }
from 'rxjs';

import { environment }
from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ResumeService {

  private apiUrl =
    `${environment.apiUrl}/resumes`;

  constructor(
    private http: HttpClient
  )
  {
  }

  uploadResume(
    formData: FormData
  ): Observable<any>
  {
    return this.http.post(
      `${this.apiUrl}/upload`,
      formData
    );
  }

  getResumes(): Observable<any>
  {
    return this.http.get(
      this.apiUrl
    );
  }
  parseResume(
    resumeId: number
  )
  {
    return this.http.post(
      `${this.apiUrl}/${resumeId}/parse`,
      {}
    );
  }
  scoreResume(
  resumeId: number
  )
  {
    return this.http.post(
      `${this.apiUrl}/${resumeId}/score`,
      {}
    );
  }
}