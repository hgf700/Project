import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-login-callback',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule
  ],
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
    var zoauth=1
    if (zoauth==1){
    // const token = this.route.snapshot.queryParamMap.get('token');
    var token = this.route.snapshot.queryParamMap.get('token');
    }
    else{
      token = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEyOTJkZTYxLTkwMDgtNDY5YS05NDVkLTc2M2M1ZWZlYzU4MCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6InBhd2VsbGFwaW5za2kyMkBnbWFpbC5jb20iLCJleHAiOjE3NjgyNDU4Mzd9.otBBhxws3WPXXTofkQgTfXZI1jRSIJk1AbmFfF8Fpa4"
    }


    if (!token) {
      console.error('Brak tokena w login-callback');
      // this.router.navigate(['/login']);
      return;
    }

    localStorage.setItem('jwt', token);

    console.log('JWT zapisany');
    console.log(token);

  }
}
