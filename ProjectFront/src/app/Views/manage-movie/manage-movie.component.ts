import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ManageMovieService } from '../../Services/ManageMovieService';
import { MovieAG } from '../../interfaces/movie';

@Component({
  selector: 'app-manage-movie',
  imports: [CommonModule],
  templateUrl: './manage-movie.component.html',
  styleUrl: './manage-movie.component.css'
})
export class ManageMovieComponent implements OnInit {

  movies: MovieAG[]=[];
  loading = true;

  constructor(
    private managemovieService: ManageMovieService
  ){}

  ngOnInit(): void {
      this.managemovieService.getMovies().subscribe({
        next: value=>{
          this.movies=value;
          this.loading=false;
        },
        error: err =>{
          console.error(err);
          this.loading=false;
        }
      })
  }
  RateGood(movieId: number) {
    this.managemovieService.rateMovie(movieId, 10).subscribe({
      next: () => console.log('Ocena zapisana 10'),
     error: err => console.error(err)
    });
  }
  RateNeutral(movieId: number) {
    this.managemovieService.rateMovie(movieId, 3).subscribe({
      next: () => console.log('Ocena zapisana 3'),
     error: err => console.error(err)
    });

  }
  RateBad(movieId: number) {
    this.managemovieService.rateMovie(movieId, 1).subscribe({
      next: () => console.log('Ocena zapisana 1'),
     error: err => console.error(err)
    });

  }

  selectedMovieTmdbId: number | null = null;

  addToPlaylist(movieTmdbId: number) {
    this.selectedMovieTmdbId = movieTmdbId;
    // this.openPlaylistModal();
    }
}