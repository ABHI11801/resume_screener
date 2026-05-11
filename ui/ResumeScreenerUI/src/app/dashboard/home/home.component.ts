import { Component } from '@angular/core';
import { CommonModule }from '@angular/common';
import { RouterOutlet, Router, RouterLink }from '@angular/router';
import { MatToolbarModule} from '@angular/material/toolbar';
import { MatSidenavModule} from '@angular/material/sidenav';
import { MatListModule} from '@angular/material/list';
import {  MatButtonModule} from '@angular/material/button';
import {  MatIconModule} from '@angular/material/icon';
import { AuthService }from '../../services/auth.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    CommonModule,
    RouterOutlet,
    RouterLink,
    MatToolbarModule,
    MatSidenavModule,
    MatListModule,
    MatButtonModule,
    MatIconModule
  ],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {

  constructor(private authService: AuthService,private router: Router)
  {
  }

  logout(): void
  {
    this.authService.logout();

    this.router.navigate([
      '/login'
    ]);
  }
}