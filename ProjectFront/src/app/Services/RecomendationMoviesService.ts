import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { getAuthHeaders } from '../helpers/GetAuthHeaders';
import { MovieRecommendationsDto } from '../Dto/movieRecommendationsDto';

@Injectable({ providedIn: 'root' })
export class RecomendationMoviesService {
  private apiurl = 'https://localhost:7218/recommendations';

  constructor(private http: HttpClient) {}

  getMovieRecommendation() {
    return this.http.get<MovieRecommendationsDto[]>(
      `${this.apiurl}/show-recommendations`,
      { headers: getAuthHeaders() },
    );
  }

}