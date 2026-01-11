import { Component } from '@angular/core';

@Component({
  selector: 'app-login',
  imports: [],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  loginWithGoogle() {
  window.location.href = 'https://localhost:7218/auth/signin-google';
  }
}
