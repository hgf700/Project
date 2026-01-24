import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class GenreSeedService {

  private apiUrl = 'https://localhost:7218/movies/seed_genre';

  constructor(private http: HttpClient) {}

    importSeedFromTmdb() {
    const token = localStorage.getItem('jwt');

    return this.http.post(
      `${this.apiUrl}`,
      {},
      {
        headers: {
          Authorization: `Bearer ${token}`
        }
      }
    );
  }
}
