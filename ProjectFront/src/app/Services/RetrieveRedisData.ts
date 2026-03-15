import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { getAuthHeaders } from '../helpers/GetAuthHeaders';
import { retrieveRedisDataDto } from '../Dto/retrieveRedisDataDto';

@Injectable({ providedIn: 'root' })
export class RetrieveRedisData {
  private apiurl = 'https://localhost:7218/redis';

  constructor(private http: HttpClient) {}

  getRedisData() {
    return this.http.get<retrieveRedisDataDto[]>(
      `${this.apiurl}/get-redis-data/`,
      { headers: getAuthHeaders() },
    );
  }

}
