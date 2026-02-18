import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { getAuthHeaders } from '../helpers/GetAuthHeaders';
import { profileMessageDto } from '../Dto/profileMessageDto';

@Injectable({ providedIn: 'root' })
export class RecomendationMovies {
  private apiurl = 'https://localhost:7218/profile-message';



}