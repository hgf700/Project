import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class PhotoService {

  private apiUrl = 'https://localhost:7218/movies/show-images';

  constructor(private http: HttpClient) {}

  getImages() {
    const token = localStorage.getItem('jwt');

    return this.http.get<
      { posterPath: string | null; backdropPath: string | null }[]
    >(this.apiUrl, {
      headers: {
        Authorization: `Bearer ${token}`
      }
    });
  }
}

