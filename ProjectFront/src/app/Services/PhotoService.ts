import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { getAuthHeaders } from '../helpers/GetAuthHeaders';

@Injectable({ providedIn: 'root' })
export class PhotoService {
  private apiUrl = 'https://localhost:7218/image/show-images';

  constructor(private http: HttpClient) {}

  getImages() {
    return this.http.get<
      { posterPath: string | null; backdropPath: string | null }[]
    >(this.apiUrl, {  headers: getAuthHeaders() });
  }
}
