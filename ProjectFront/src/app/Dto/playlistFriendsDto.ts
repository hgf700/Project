import { PlaylistRole } from "../enum/playlistRole";

export interface PlaylistFriendsDto {
  playlistId: number;
  userId: string;
  role: PlaylistRole;
}
