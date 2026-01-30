import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FriendAG } from '../interfaces/friend';

@Injectable({ providedIn: 'root' })
export class FriendService {
  private apiUrl = 'https://localhost:7218/friend';

  constructor(private http: HttpClient) {}

  addFriend(email: string) {
    const token = localStorage.getItem('jwt');
    const headers = { Authorization: `Bearer ${token}` };
    return this.http.post(`${this.apiUrl}/add-friend`, { email }, { headers });
  }

  getFriends() {
    const token = localStorage.getItem('jwt');
    const headers = { Authorization: `Bearer ${token}` };
    return this.http.get<FriendAG[]>(
      `${this.apiUrl}/show-friends`,
      { headers },
    );
  }

  deleteFriend(friendId: string) {
    const token = localStorage.getItem('jwt');
    const headers = { Authorization: `Bearer ${token}` };
    return this.http.post(`${this.apiUrl}/delete-friend`, { friendId }, { headers });
  }
}
