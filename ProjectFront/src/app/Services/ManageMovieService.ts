import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Movie } from '../interfaces/movie';

@Injectable({ providedIn: 'root' })
export class ManageMovieService {

    private apiurl='https://localhost:7218/movies/show-movies';

  constructor(private http: HttpClient) {}

  getMovies() {
    const token = localStorage.getItem('jwt');
    const headers = { 'Authorization': `Bearer ${token}` };
    return this.http.get<Movie[]>(this.apiurl, { headers });
  }
}
