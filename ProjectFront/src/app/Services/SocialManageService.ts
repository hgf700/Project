import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { getAuthHeaders } from '../helpers/GetAuthHeaders';
import { profileMessageDto } from '../Dto/profileMessageDto';
import { followedProfilesDto } from '../Dto/followedProfilesDto';
import { viewLikedPlaylistDto } from '../Dto/viewLikedPlaylistDto';



@Injectable({ providedIn: 'root' })
export class SocialManageService {
  private apiurlSocial = 'https://localhost:7218/social';

  profileMessage: profileMessageDto[]=[];
  followedProfiles: followedProfilesDto[]=[];
  viewLikedPlaylist: viewLikedPlaylistDto[]=[];

  constructor(private http: HttpClient) {}

  sharePlaylistWithFriends(playlistId: number, friendId: string) {
    return this.http.post(
      `${this.apiurlSocial}/share-playlist/${playlistId}/members`,
      { friendId },
      { headers: getAuthHeaders() },
    );
  }

  sharePlaylistPublically(playlistId: number) {
    return this.http.put(
      `${this.apiurlSocial}/change-to-public/${playlistId}`,
      {},
      { headers: getAuthHeaders() },
    );
  }

  stopSharePlaylsitPublically(playlistId: number) {
    return this.http.put(
      `${this.apiurlSocial}/change-to-private/${playlistId}`,
      {},
      { headers: getAuthHeaders() },
    );
  }

  writeProfileMessage(targetUserId: string,text: string) {
    return this.http.post(
      `${this.apiurlSocial}/${targetUserId}/write-profile-message`,
      {text},
      { headers: getAuthHeaders() },
    );
  }

  getProfileMessage(targetUserId: string) {
    return this.http.get<profileMessageDto[]>(
      `${this.apiurlSocial}/users/${targetUserId}/view-profile-message`,
      { headers: getAuthHeaders() }
    );
  }

  deleteProfileMessage(messageId: number) {
    return this.http.delete(
      `${this.apiurlSocial}/delete-profile-message/${messageId}`,
      { headers: getAuthHeaders() }
    );
  }

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
      { headers: getAuthHeaders() }
    );
  }

  getProfileFollows() {
    return this.http.get<followedProfilesDto[]>(
      `${this.apiurlSocial}/view-follows-of-profile`,
      { headers: getAuthHeaders() }
    );
  }

  likePlaylist(playlistId: number) {
    return this.http.post(
      `${this.apiurlSocial}/like-playlist/${playlistId}`,
      {},
      { headers: getAuthHeaders() },
    );
  }

  stopLikePlaylist(playlistId: number) {
    return this.http.delete(
      `${this.apiurlSocial}/stop-like-playlist/${playlistId}`,
      { headers: getAuthHeaders() }
    );
  }

  getLikedPlaylist() {
    return this.http.get<viewLikedPlaylistDto[]>(
      `${this.apiurlSocial}/view-liked-playlist`,
      { headers: getAuthHeaders() }
    );
  }
}
