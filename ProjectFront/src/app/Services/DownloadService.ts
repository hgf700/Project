import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class DownloadService {
  private apiUrl = 'https://localhost:7218/movies';

  constructor(private http: HttpClient) {}

  importAllTmdb() {
    const token = localStorage.getItem('jwt');
    const headers = { Authorization: `Bearer ${token}` };

    return this.http.post(
      `${this.apiUrl}/import-from-tmdb`,
      {},
      { headers }
    )
  }
}
