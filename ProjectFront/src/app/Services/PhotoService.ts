import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class PhotoService {

  private apiUrl = 'https://localhost:7218/image/show-images';

  constructor(private http: HttpClient) {}

  getImages() {
    const token = localStorage.getItem('jwt');
    const headers = { Authorization: `Bearer ${token}` };

    return this.http.get<
      { posterPath: string | null; backdropPath: string | null }[]
    >(this.apiUrl, 
      {headers});
  }
}
