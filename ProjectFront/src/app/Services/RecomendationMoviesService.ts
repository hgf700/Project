import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { getAuthHeaders } from '../helpers/GetAuthHeaders';
// import { MovieRecommendationsDto } from '../Dto/movieRecommendationsDto';

@Injectable({ providedIn: 'root' })
export class RecomendationMoviesService {
  private apiurl = 'https://localhost:7218/recommendations';

  constructor(private http: HttpClient) {}

  postMovieRecommendationByML(){
    return this.http.post(
      `${this.apiurl}/start-recomend-process-asp`,
      null,
      { headers: getAuthHeaders() },
    );
  }

  postReceiveRecommendationsFromPy(){
    return this.http.post<string[]>(
      `${this.apiurl}/receive-recommend-process-py`,
      {},
      { headers: getAuthHeaders() },
    );
  }

}