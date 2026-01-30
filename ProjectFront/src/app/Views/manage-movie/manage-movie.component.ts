import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { ManageMovieService } from '../../Services/ManageMovieService';
import { MovieAG } from '../../interfaces/movie';
import { PlaylistSubWindowComponent } from '../playlist-sub-window/playlist-sub-window.component';

@Component({
  selector: 'app-manage-movie',
  standalone: true,
  imports: [CommonModule, MatDialogModule],
  templateUrl: './manage-movie.component.html',
  styleUrl: './manage-movie.component.css',
})
export class ManageMovieComponent implements OnInit {
  movies: MovieAG[] = [];
  loading = true;

  constructor(
    private managemovieService: ManageMovieService,
    private dialog: MatDialog,
  ) {}

  ngOnInit(): void {
    this.managemovieService.getMovies().subscribe({
      next: (value) => {
        this.movies = value;
        this.loading = false;
      },
      error: (err) => {
        console.error(err);
        this.loading = false;
      },
    });
  }
  RateGood(movieId: number) {
    this.managemovieService.rateMovie(movieId, 10).subscribe({
      next: () => console.log('Ocena zapisana 10'),
      error: (err) => console.error(err),
    });
  }
  RateNeutral(movieId: number) {
    this.managemovieService.rateMovie(movieId, 3).subscribe({
      next: () => console.log('Ocena zapisana 3'),
      error: (err) => console.error(err),
    });
  }
  RateBad(movieId: number) {
    this.managemovieService.rateMovie(movieId, 1).subscribe({
      next: () => console.log('Ocena zapisana 1'),
      error: (err) => console.error(err),
    });
  }

  selectedMovieTmdbId: number | null = null;

  AddToPlaylist(movieTmdbId: number) {
    const dialogRef = this.dialog.open(PlaylistSubWindowComponent, {
      width: '600px',
      height: '400px',
      data: {
        tmdbId: movieTmdbId,
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      console.log('Dialog closed:', result);
    });
  }

  removeRateFromMedia(movieId: number){
    this.managemovieService.removeRateFromMedia(movieId).subscribe({
      next: () => console.log('removeRateFromMedia'),
      error: (err) => console.error(err),
    });
  }
}
