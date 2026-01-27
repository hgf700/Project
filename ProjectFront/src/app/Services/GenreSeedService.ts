import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class GenreSeedService {
  private apiUrl = 'https://localhost:7218/movies/seed-genre';

  constructor(private http: HttpClient) {}

  importSeedFromTmdb() {
    const token = localStorage.getItem('jwt');
    const headers = { Authorization: `Bearer ${token}` };

    return this.http.post(`${this.apiUrl}`, {}, { headers });
  }
}
