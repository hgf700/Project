import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MovieAG } from '../interfaces/movie';

@Injectable({ providedIn: 'root' })
export class ManageMovieService {
  private apiurlMovies = 'https://localhost:7218/movies';
  private apiurlRate = 'https://localhost:7218/rating';

  constructor(private http: HttpClient) {}

  getMovies() {
    const token = localStorage.getItem('jwt');
    const headers = { Authorization: `Bearer ${token}` };
    return this.http.get<MovieAG[]>(`${this.apiurlMovies}/show-movies`, { headers });
  }

  rateMovie(movieId: number, rating: number) {
    const token = localStorage.getItem('jwt');
    const headers = { Authorization: `Bearer ${token}` };

    return this.http.post(
      `${this.apiurlRate}/rate-movie?movieId=${movieId}`,
      { rating },
      { headers },
    );
  }
}
