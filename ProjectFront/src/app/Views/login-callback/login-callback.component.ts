import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { CurrentUserService } from '../../Services/CurrentUserService';
import { CurrentUserAG } from '../../interfaces/currentUser';

@Component({
  selector: 'app-login-callback',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './login-callback.component.html',
  styleUrl: './login-callback.component.css',
})
export class LoginCallbackComponent implements OnInit {
  currentUser?: CurrentUserAG;
  
  constructor(
    private currentUserService:CurrentUserService,
    private route: ActivatedRoute,
    private router: Router,
  ) {}

  manageFriend() {
    this.router.navigate(['/manage-friends']);
  }

  showPhoto() {
    this.router.navigate(['/show-movie-photo']);
  }

  manageMedia() {
    this.router.navigate(['/manage-movie']);
  }

  download() {
    this.router.navigate(['/download']);
  }

  playlistWIndow() {
    this.router.navigate(['/playlist-window']);
  }

  loadCurrentUser() {
  this.currentUserService.getCurrentUser().subscribe({
    next: (user) => {
      console.log('RESPONSE /me:', user);
      this.currentUser = user;
    },
    error: (err) => {
      console.error('ERROR /me:', err);
    }
  });
}


  ngOnInit(): void {
  const tokenFromUrl = this.route.snapshot.queryParamMap.get('token');
  const tokenFromStorage = localStorage.getItem('jwt');

  if (tokenFromUrl) {
    localStorage.setItem('jwt', tokenFromUrl);
    console.log('JWT zapisany z URL');
  }

  if (!tokenFromUrl && !tokenFromStorage) {
    console.error('Brak tokena – użytkownik niezalogowany');
    return;
  }

  this.loadCurrentUser();
}

}
