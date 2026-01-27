import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { PlaylistAG } from '../interfaces/playlist';


@Injectable({ providedIn: 'root' })
export class PlaylistService {

    private apiurlShowPlaylist='https://localhost:7218/playlist/show-playlists';
    private apiurlCreatePlaylist='https://localhost:7218/playlist/create-playlist';


  constructor(private http: HttpClient) {}

  getPlaylists(){
    const token = localStorage.getItem('jwt');
    const headers = { Authorization: `Bearer ${token}` };

    return this.http.get<PlaylistAG[]>(this.apiurlShowPlaylist, { headers });
  }

//   addToPlaylists(movieId: number){
//     const token = localStorage.getItem('jwt');
//     const headers = { Authorization: `Bearer ${token}` };

//     return this.http.post(
//       `${this.apiurlAddToPlaylist}`,
//       { movieId },
//       { headers }
//     );
//   }

  createPlaylists(nameOfPlaylist: string){
    const token = localStorage.getItem('jwt');
    const headers = { Authorization: `Bearer ${token}` };

    return this.http.post(
      `${this.apiurlCreatePlaylist}`,
      { nameOfPlaylist },
      { headers }
    );
  }
}