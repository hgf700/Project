import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class MovieService {

  private apiUrl = 'https://localhost:7218/movies/add-from-tmdb';

  constructor(private http: HttpClient) {}

    importFromTmdb(page: number = 1) {
    const token = localStorage.getItem('jwt');

    return this.http.post(
      `${this.apiUrl}?page=${page}`,
      {},
      {
        headers: {
          Authorization: `Bearer ${token}`
        }
      }
    );
  }
}
