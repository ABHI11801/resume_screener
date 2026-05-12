import { Component, OnInit} from '@angular/core';
import { CommonModule }from '@angular/common';
import {  MatCardModule} from '@angular/material/card';
import { DashboardService }from '../../services/dashboard.service';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';

@Component({
  selector: 'app-dashboard-page',

  standalone: true,

  imports: [
    CommonModule,
    MatCardModule,
    MatIconModule,
    MatDividerModule,
  ],

  templateUrl:
    './dashboard-page.component.html',

  styleUrls: [
    './dashboard-page.component.scss'
  ]
})
export class DashboardPageComponent
implements OnInit {

  totalJobs = 0;
  totalResumes = 0;
  totalInterviews = 0;
  totalScreenedCandidates = 0;
  upcomingInterviews: any[] = [];

  constructor(
    private dashboardService:
      DashboardService
  )
  {
  }

  ngOnInit(): void
  {
    this.loadStats();
  }

  loadStats(): void
  {
    this.dashboardService
      .getStats()
      .subscribe({

        next: (response) =>
        {
          this.totalJobs =
            response.totalJobs;

          this.totalResumes =
            response.totalResumes;

          this.totalInterviews =
            response.totalInterviews;

          this.totalScreenedCandidates =
            response.totalScreenedCandidates;

          this.upcomingInterviews =
            response.upcomingInterviews;
        },

        error: (error) =>
        {
          console.error(error);
        }
      });
  }
}