import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { getAuthHeaders } from '../helpers/GetAuthHeaders';
import { PlaylistFriendsDto } from '../Dto/playlistFriendsDto';

@Injectable({ providedIn: 'root' })
export class SharePlaylistToFriendsService {
  private apiurlSocial = 'https://localhost:7218/share-playlist-to-friend';

  playlistFriendsDto: PlaylistFriendsDto[]=[];

  constructor(private http: HttpClient) {}

  sharePlaylistWithFriends(playlistId: number, friendId: string) {
    return this.http.post(
      `${this.apiurlSocial}/share-playlist/${playlistId}/members`,
      { friendId },
      { headers: getAuthHeaders() },
    );
  }

  stopSharePlaylistWithFriends(playlistId: number, friendId: string) {
    return this.http.post(
      `${this.apiurlSocial}/stop-share-playlist/${playlistId}/members`,
      { friendId },
      { headers: getAuthHeaders() },
    );
  }

  getPlaylistWithFriends(playlistId: number){
    return this.http.get<PlaylistFriendsDto[]>(
      `${this.apiurlSocial}/show-playlist-friends/${playlistId}`,
      { headers: getAuthHeaders() },
    );
  }

}
