import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MovieAG } from '../interfaces/movie';
import { PlaylistAG } from '../interfaces/playlist';


@Injectable({ providedIn: 'root' })
export class ManageMovieService {

    private apiurlShowMovies='https://localhost:7218/movies/show-movies';
    private apiurlRate='https://localhost:7218/rating/rate-movie';
    private apiurlAddToPlaylist='https://localhost:7218/playlist/add-to-playlist';
    private apiurlShowPlaylist='https://localhost:7218/playlist/show-playlists';

  constructor(private http: HttpClient) {}

  getMovies() {
    const token = localStorage.getItem('jwt');
    const headers = { 'Authorization': `Bearer ${token}` };
    return this.http.get<MovieAG[]>(this.apiurlShowMovies, { headers });
  }

  rateMovie(movieId: number, rating: number) {
    const token = localStorage.getItem('jwt');
    const headers = { Authorization: `Bearer ${token}` };

    return this.http.post(
      `${this.apiurlRate}?movieId=${movieId}`,
      { rating },
      { headers }
    );
  }

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

    return this.http.get<PlaylistAG[]>(this.apiurlShowMovies, { headers });
  }

}