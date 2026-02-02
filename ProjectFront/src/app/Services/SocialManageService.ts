import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { getAuthHeaders } from '../helpers/GetAuthHeaders';
import { profileMessageDto } from '../Dto/profileMessageDto';


@Injectable({ providedIn: 'root' })
export class SocialManageService {
  private apiurlSocial = 'https://localhost:7218/social';

  profileMessage: profileMessageDto[]=[];

  constructor(private http: HttpClient) {}

  sharePlaylistWithFriends(playlistId: number, friendId: string) {
    return this.http.post(
      `${this.apiurlSocial}/share-playlist/${playlistId}/members`,
      { friendId },
      { headers: getAuthHeaders() },
    );
  }

  sharePlaylistPublically(playlistId: number) {
    return this.http.put(
      `${this.apiurlSocial}/change-to-public/${playlistId}`,
      {},
      { headers: getAuthHeaders() },
    );
  }

  stopSharePlaylsitPublically(playlistId: number) {
    return this.http.put(
      `${this.apiurlSocial}/change-to-private/${playlistId}`,
      {},
      { headers: getAuthHeaders() },
    );
  }

  writeProfileMessage(targetUserId: string,text: string) {
    return this.http.post(
      `${this.apiurlSocial}/${targetUserId}/write-profile-message`,
      {text},
      { headers: getAuthHeaders() },
    );
  }

  getProfileMessage(targetUserId: string) {
    return this.http.get<profileMessageDto[]>(
      `${this.apiurlSocial}/users/${targetUserId}/view-profile-message`,
      { headers: getAuthHeaders() }
    );
  }

}
