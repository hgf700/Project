import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { getAuthHeaders } from '../helpers/GetAuthHeaders';
import { profileMessageDto } from '../Dto/profileMessageDto';
import { followedProfilesDto } from '../Dto/followedProfilesDto';
import { viewLikedPlaylistDto } from '../Dto/viewLikedPlaylistDto';

@Injectable({ providedIn: 'root' })
export class SharePlaylistToFriendsService {
  private apiurlSocial = 'https://localhost:7218/share-playlist-to-friend';

  profileMessage: profileMessageDto[] = [];
  followedProfiles: followedProfilesDto[] = [];
  viewLikedPlaylist: viewLikedPlaylistDto[] = [];

  constructor(private http: HttpClient) {}

  sharePlaylistWithFriends(playlistId: number, friendId: string) {
    return this.http.post(
      `${this.apiurlSocial}/share-playlist/${playlistId}/members`,
      { friendId },
      { headers: getAuthHeaders() },
    );
  }
}
