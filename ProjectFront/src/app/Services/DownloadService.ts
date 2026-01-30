import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class DownloadService {
  private apiUrl = 'https://localhost:7218/movies';

  constructor(private http: HttpClient) {}

  importSeedFromTmdb() {
    const token = localStorage.getItem('jwt');
    const headers = { Authorization: `Bearer ${token}` };

    return this.http.post(`${this.apiUrl}/seed-genre`, {}, { headers });
  }

  importFromTmdb(page: number = 1) {
    const token = localStorage.getItem('jwt');
    const headers = { Authorization: `Bearer ${token}` };

    return this.http.post(`${this.apiUrl}/add-from-tmdb?page=${page}`, {}, { headers });
  }
}
