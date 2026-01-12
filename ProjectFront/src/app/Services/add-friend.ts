import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class FriendService {

  private apiUrl = 'https://localhost:7218/friend/add-friend';

  constructor(private http: HttpClient) {}

  addFriend(email: string) {
    return this.http.post(`${this.apiUrl}`, { email });
  }
}
