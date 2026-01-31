import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { getAuthHeaders } from '../helpers/GetAuthHeaders';
import { CurrentUserAG } from '../interfaces/currentUser';

@Injectable({ providedIn: 'root' })
export class CurrentUserService {
  private apiUrl = 'https://localhost:7218/auth';

  constructor(private http: HttpClient) {}

  getCurrentUser() {
  return this.http.get<CurrentUserAG>(
    `${this.apiUrl}/me`,
    { headers: getAuthHeaders() }
  );
}
}