import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Movie } from '../interfaces/movie';

@Injectable({ providedIn: 'root' })
export class ManageMovieService {

    private apiurlShowMovies='https://localhost:7218/movies/show-movies';
    private apiurlRate='https://localhost:7218/rating/rate-movie';
  constructor(private http: HttpClient) {}

  getMovies() {
    const token = localStorage.getItem('jwt');
    const headers = { 'Authorization': `Bearer ${token}` };
    return this.http.get<Movie[]>(this.apiurlShowMovies, { headers });
  }
  RateGood(){
    const token = localStorage.getItem('jwt');
    const headers = { 'Authorization': `Bearer ${token}` };
  }
  RateNeutral(){
    const token = localStorage.getItem('jwt');
    const headers = { 'Authorization': `Bearer ${token}` };
  }
  RateBad(){
    const token = localStorage.getItem('jwt');
    const headers = { 'Authorization': `Bearer ${token}` };
  }
}
