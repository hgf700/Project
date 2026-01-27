import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { PlaylistAG } from '../interfaces/playlist';


@Injectable({ providedIn: 'root' })
export class PlaylistService {

    private apiurlAddToPlaylist='https://localhost:7218/playlist/add-to-playlist';
    private apiurlShowPlaylist='https://localhost:7218/playlist/show-playlists';

  constructor(private http: HttpClient) {}

  addToPlaylists(movieId: number){
    const token = localStorage.getItem('jwt');
    const headers = { Authorization: `Bearer ${token}` };

    return this.http.post(
      `${this.apiurlAddToPlaylist}`,
      { movieId },
      { headers }
    );
  }

  getPlaylists(){
    const token = localStorage.getItem('jwt');
    const headers = { Authorization: `Bearer ${token}` };

    return this.http.get<PlaylistAG[]>(this.apiurlShowPlaylist, { headers });
  }
}