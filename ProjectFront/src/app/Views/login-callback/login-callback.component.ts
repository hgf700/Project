import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login-callback',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './login-callback.component.html',
  styleUrl: './login-callback.component.css'
})
export class LoginCallbackComponent implements OnInit {

  constructor(
    private route: ActivatedRoute,
    private router: Router
  ) {}

  addFriend() {
    this.router.navigate(['/add-friends']);
  }

  ngOnInit(): void {
    const token = this.route.snapshot.queryParamMap.get('token');


    if (!token) {
      console.error('Brak tokena w login-callback');
      this.router.navigate(['/']);
      return;
    }

    localStorage.setItem('jwt', token);
    console.log('JWT zapisany');
  }
}
