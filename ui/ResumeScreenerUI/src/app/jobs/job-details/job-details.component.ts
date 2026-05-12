import {
  Component,
  OnInit
} from '@angular/core';

import { CommonModule }
from '@angular/common';

import {
  ActivatedRoute,RouterModule
} from '@angular/router';

import { JobService }
from '../../services/job.service';

import {
  MatDialog,
  MatDialogModule
} from '@angular/material/dialog';

import {
  UploadResumeDialogComponent
} from '../upload-resume-dialog/upload-resume-dialog.component';
import { ResumeService }
from '../../services/resume.service';

import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
@Component({
  selector: 'app-job-details',

  standalone: true,

  imports: [
    CommonModule,
    MatDialogModule,
    MatProgressSpinnerModule,
    MatDividerModule,
    MatIconModule
  ],

  templateUrl:
    './job-details.component.html',

  styleUrls: [
    './job-details.component.scss'
  ]
})
export class JobDetailsComponent
implements OnInit {

  job: any;
  candidates: any[] = [];

  isLoading = false;

  constructor(
    private route:
      ActivatedRoute,

    private jobService:
      JobService,

    private dialog:
      MatDialog,

    private resumeService:
      ResumeService
  )
  {
  }

  ngOnInit(): void
  {
    const id =
      Number(
        this.route.snapshot.paramMap.get('id')
      );

    this.loadJob(id);
    this.loadCandidates(id);
  }

  loadJob(id: number): void
  {
    this.isLoading = true;

    this.jobService
      .getJobById(id)
      .subscribe({

        next: (response) =>
        {
          this.job = response;

          this.isLoading = false;
        },

        error: (error) =>
        {
          console.error(error);

          this.isLoading = false;
        }
      });
  }
  openUploadDialog(): void
  {
    const dialogRef =
      this.dialog.open(
        UploadResumeDialogComponent,
        {
          width: '500px',

          data: {
            jobId: this.job.id
          }
        }
      );

    dialogRef.afterClosed()
      .subscribe(result =>
      {
        if (result)
        {
          this.loadCandidates(
            this.job.id
          );
        }
      });
  }
  loadCandidates(
    jobId: number
  ): void
  {
    this.jobService
      .getCandidates(jobId)
      .subscribe({

        next: (response) =>
        {
          this.candidates =
            response as any[];
        },

        error: (error) =>
        {
          console.error(error);
        }
      });
  }

  parseCandidate(
    resumeId: number
  ): void
  {
    this.resumeService
      .parseResume(resumeId)
      .subscribe({

        next: (response) =>
        {
          console.log(
            'Parsed:',
            response
          );

          this.loadCandidates(
            this.job.id
          );
        },

        error: (error) =>
        {
          console.error(error);
        }
      });
  }


  scoreCandidate(
    resumeId: number
  ): void
  {
    this.resumeService
      .scoreResume(resumeId)
      .subscribe({

        next: (response) =>
        {
          console.log(
            'Scored:',
            response
          );

          this.loadCandidates(
            this.job.id
          );
        },

        error: (error) =>
        {
          console.error(error);
        }
      });
  }
  viewResume(candidate: any): void {
    // option A — open the resume URL in a new tab
    if (candidate.resumeUrl) {
      window.open(candidate.resumeUrl, '_blank');
    }

    // option B — if you have a resume route
    // this.router.navigate(['/resumes', candidate.id]);
  }
}