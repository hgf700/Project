import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { getAuthHeaders } from '../helpers/GetAuthHeaders';

@Injectable({ providedIn: 'root' })
export class SharePlaylistPublically {
  private apiurlSocial = 'https://localhost:7218/share-playlist-publically';

  constructor(private http: HttpClient) {}

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
}
