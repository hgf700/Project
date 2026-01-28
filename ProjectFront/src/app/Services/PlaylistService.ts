import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { PlaylistAG } from '../interfaces/playlist';

@Injectable({ providedIn: 'root' })
export class PlaylistService {
  private readonly baseUrl = 'https://localhost:7218/playlist';

  constructor(private http: HttpClient) {}

  getPlaylists() {
    const token = localStorage.getItem('jwt');
    const headers = { Authorization: `Bearer ${token}` };
    return this.http.get<PlaylistAG[]>(
      `${this.baseUrl}/show-playlists`,
      { headers }
    );
  }

  createPlaylist(name: string) {
    const token = localStorage.getItem('jwt');
    const headers = { Authorization: `Bearer ${token}` };
    return this.http.post(
      `${this.baseUrl}/create-playlist`,
      { name },
      { headers } 
    );
  }

  addMovieToPlaylist(playlistId: number, tmdbId: number) {
    const token = localStorage.getItem('jwt');
    const headers = { Authorization: `Bearer ${token}` };
    return this.http.post(
      `${this.baseUrl}/${playlistId}/movies/${tmdbId}`,
      {},
      { headers } 
    );
  }
}
