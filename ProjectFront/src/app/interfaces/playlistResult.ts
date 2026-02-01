import { MovieAG } from './movie';
import { PlaylistRole } from "./playlistRole";

export interface PlaylistResultAG {
  id: number;
  name: string;
  movies: MovieAG[];
  role: PlaylistRole;
}
