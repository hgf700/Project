import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class ManageSocialService {
  private apiurlMovies = 'https://localhost:7218/movies';
  private apiurlRate = 'https://localhost:7218/rating';

  constructor(private http: HttpClient) {}

  
}
