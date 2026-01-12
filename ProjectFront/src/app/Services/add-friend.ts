import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class FriendService {

  private apiUrl = 'https://localhost:7218/friend/add-friend';

  constructor(private http: HttpClient) {}

  // W FriendService
  addFriend(email: string) {
    const token = localStorage.getItem('jwt');
    const headers = { 'Authorization': `Bearer ${token}` };
    return this.http.post(this.apiUrl, { email }, { headers });
  }
}
