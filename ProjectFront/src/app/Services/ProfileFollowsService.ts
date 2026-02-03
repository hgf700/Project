import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { getAuthHeaders } from '../helpers/GetAuthHeaders';
import { followedProfilesDto } from '../Dto/followedProfilesDto';

@Injectable({ providedIn: 'root' })
export class ProfileFollowsService {
  private apiurlSocial = 'https://localhost:7218/social';

  followedProfiles: followedProfilesDto[] = [];

  constructor(private http: HttpClient) {}

  followProfile(targetUserId: string) {
    return this.http.post(
      `${this.apiurlSocial}/follow-profile/${targetUserId}`,
      {},
      { headers: getAuthHeaders() },
    );
  }

  stopFollowProfile(targetUserId: string) {
    return this.http.delete(
      `${this.apiurlSocial}/stop-follow-profile/${targetUserId}`,
      { headers: getAuthHeaders() },
    );
  }

  getProfileFollows() {
    return this.http.get<followedProfilesDto[]>(
      `${this.apiurlSocial}/view-follows-of-profile`,
      { headers: getAuthHeaders() },
    );
  }
}
