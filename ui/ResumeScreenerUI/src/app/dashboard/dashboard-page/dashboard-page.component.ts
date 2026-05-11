import { Component } from '@angular/core';
import { CommonModule }from '@angular/common';
import { MatCardModule} from '@angular/material/card';

@Component({
  selector: 'app-dashboard-page',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule
  ],

  templateUrl:
    './dashboard-page.component.html',

  styleUrls: [
    './dashboard-page.component.scss'
  ]
})
export class DashboardPageComponent {

  totalJobs = 12;

  totalResumes = 34;

  totalInterviews = 8;

  topScore = 92;
}