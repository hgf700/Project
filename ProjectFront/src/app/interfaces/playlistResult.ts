import { MovieAG } from './movie';

export interface PlaylistResultAG {
  id: number;
  name: string;
  movies: MovieAG[];
}
