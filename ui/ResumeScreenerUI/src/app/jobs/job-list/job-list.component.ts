import { Component, OnInit }from '@angular/core';
import { CommonModule }from '@angular/common';
import { JobService }from '../../services/job.service';

@Component({
  selector: 'app-job-list',

  standalone: true,

  imports: [CommonModule],

  templateUrl: './job-list.component.html',

  styleUrls: ['./job-list.component.scss']
})
export class JobListComponent
implements OnInit {

  jobs: any[] = [];

  isLoading = false;

  constructor(private jobService: JobService)
  {
  }

  ngOnInit(): void
  {
    this.loadJobs();
  }

  loadJobs(): void
  {
    this.isLoading = true;

    this.jobService.getJobs()
      .subscribe({

        next: (response) =>
        {
          this.jobs = response;

          this.isLoading = false;
        },

        error: (error) =>
        {
          console.error(error);

          this.isLoading = false;
        }
      });
  }
}