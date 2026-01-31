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

    this.router.navigate(['/developing-view']);
  }
}
