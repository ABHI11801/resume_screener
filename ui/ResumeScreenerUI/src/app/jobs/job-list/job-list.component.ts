import { Component, OnInit }from '@angular/core';
import { CommonModule }from '@angular/common';
import { JobService }from '../../services/job.service';
import {MatDialog,MatDialogModule} from '@angular/material/dialog';
import { AddJobDialogComponent} from '../add-job-dialog/add-job-dialog.component';

@Component({
  selector: 'app-job-list',

  standalone: true,

  imports: [CommonModule,MatDialogModule],

  templateUrl: './job-list.component.html',

  styleUrls: ['./job-list.component.scss']
})
export class JobListComponent
implements OnInit {

  jobs: any[] = [];

  isLoading = false;

  constructor(private jobService: JobService,private dialog:MatDialog)
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
  openAddJobDialog(): void
  {
    const dialogRef =
      this.dialog.open(
        AddJobDialogComponent,
        {
          width: '500px'
        }
      );

    dialogRef.afterClosed()
      .subscribe(result =>
      {
        if (result)
        {
          this.loadJobs();
        }
      });
  }
}