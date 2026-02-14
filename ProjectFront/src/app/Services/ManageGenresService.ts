import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { allGenresDto } from '../Dto/allGenresDto';
import { getAuthHeaders } from '../helpers/GetAuthHeaders';

@Injectable({ providedIn: 'root' })
export class ManageGenresService {
  private apiurl = 'https://localhost:7218/preffered-genre';

  allGenres: allGenresDto[] = [];

  constructor(private http: HttpClient) {}

  chooseGenre(genreId: number) {
      return this.http.post(
        `${this.apiurl}/choose-genre/${genreId}`,
        {},
        { headers: getAuthHeaders() },
      );
    }
  
    deleteChoosenGenre(genreId: number) {
      return this.http.delete(
        `${this.apiurl}/remove-choosen-genre/${genreId}`,
        { headers: getAuthHeaders() },
      );
    }
  
    getGenres() {
      return this.http.get<allGenresDto[]>(
        `${this.apiurl}/show-genres`,
        { headers: getAuthHeaders() },
      );
    }

}