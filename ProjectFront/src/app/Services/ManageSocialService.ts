import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { getAuthHeaders } from '../helpers/GetAuthHeaders';

@Injectable({ providedIn: 'root' })
export class ManageSocialService {
  private apiurlSocial = 'https://localhost:7218/social';

  constructor(private http: HttpClient) {}

  sharePlaylistWithFriends(playlistId: number, friendId: string){
    return this.http.post(
      `${this.apiurlSocial}/share-playlist/${playlistId}/members`,
      { friendId },
      {  headers: getAuthHeaders() },
    );
  }
}
