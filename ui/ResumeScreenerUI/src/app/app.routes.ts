import { Routes } from '@angular/router';
import { LoginComponent } from './auth/login/login.component';
import { HomeComponent } from './dashboard/home/home.component';
import { authGuard } from './guards/auth.guard';
import { JobListComponent }from './jobs/job-list/job-list.component';
import { DashboardPageComponent } from './dashboard/dashboard-page/dashboard-page.component';
import { JobDetailsComponent }from './jobs/job-details/job-details.component';

export const routes: Routes = [

  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },

  {
    path: 'login',
    component: LoginComponent
  },

  {
    path: '',
    component: HomeComponent,

    canActivate: [authGuard],

    children: [

      {
        path: 'dashboard',
        component: DashboardPageComponent
      },

      {
        path: 'jobs',
        component: JobListComponent
      },
      {
        path: 'jobs/:id',
        component: JobDetailsComponent
      },

    ]
  },

  {
    path: '**',
    redirectTo: 'login'
  }
];