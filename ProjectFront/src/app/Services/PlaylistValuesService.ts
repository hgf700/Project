import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { PlaylistResultAG } from '../interfaces/playlistResult';

@Injectable({ providedIn: 'root' })
export class PlaylistValuesService {
  private readonly baseUrl = 'https://localhost:7218/playlist';

  constructor(private http: HttpClient) {}

  addMovieToPlaylist(playlistId: number, tmdbId: number) {
    const token = localStorage.getItem('jwt');
    const headers = { Authorization: `Bearer ${token}` };
    return this.http.post(
      `${this.baseUrl}/${playlistId}/movies/${tmdbId}`,
      {},
      { headers },
    );
  }

  showResultFromPlaylist(playlistId: number) {
    const token = localStorage.getItem('jwt');
    const headers = { Authorization: `Bearer ${token}` };

    return this.http.get<PlaylistResultAG>(
      `${this.baseUrl}/show-playlist-values/${playlistId}`,
      { headers },
    );
  }

  deleteFromPlaylist(playlistId: number, tmdbId: number) {
    const token = localStorage.getItem('jwt');
    const headers = { Authorization: `Bearer ${token}` };
    return this.http.post<void>(
      `${this.baseUrl}/${playlistId}/delete-from-playlist/${tmdbId}`,
      {},
      { headers },
    );
  }
}
