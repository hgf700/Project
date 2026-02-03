import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { getAuthHeaders } from '../helpers/GetAuthHeaders';
import { viewLikedPlaylistDto } from '../Dto/viewLikedPlaylistDto';

@Injectable({ providedIn: 'root' })
export class LikePlaylistService {
  private apiurlSocial = 'https://localhost:7218/like-playlist';

  viewLikedPlaylist: viewLikedPlaylistDto[] = [];

  constructor(private http: HttpClient) {}

  likePlaylist(playlistId: number) {
    return this.http.post(
      `${this.apiurlSocial}/like-playlist/${playlistId}`,
      {},
      { headers: getAuthHeaders() },
    );
  }

  stopLikePlaylist(playlistId: number) {
    return this.http.delete(
      `${this.apiurlSocial}/stop-like-playlist/${playlistId}`,
      { headers: getAuthHeaders() },
    );
  }

  getLikedPlaylist() {
    return this.http.get<viewLikedPlaylistDto[]>(
      `${this.apiurlSocial}/view-liked-playlist`,
      { headers: getAuthHeaders() },
    );
  }
}
