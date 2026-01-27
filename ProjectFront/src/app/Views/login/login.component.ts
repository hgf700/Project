import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  imports: [CommonModule, RouterModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {
  constructor(
    private route: ActivatedRoute,
    private router: Router,
  ) {}

  loginWithGoogle() {
    window.location.href = 'https://localhost:7218/auth/signin-google';
  }

  NormalLogin() {
    const token =
      'eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEyOTJkZTYxLTkwMDgtNDY5YS05NDVkLTc2M2M1ZWZlYzU4MCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6InBhd2VsbGFwaW5za2kyMkBnbWFpbC5jb20iLCJleHAiOjE3NjgyNDU4Mzd9.otBBhxws3WPXXTofkQgTfXZI1jRSIJk1AbmFfF8Fpa4';

    localStorage.setItem('jwt', token);

    if (!token) {
      console.error('Brak tokena w login-callback');
      // this.router.navigate(['/login']);
      return;
    }

    this.router.navigate(['/login-callback']);
  }
}
