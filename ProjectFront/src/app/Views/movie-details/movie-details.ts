import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ActivatedRoute, Router } from '@angular/router';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { ManageMovieService } from '../../Services/MovieManageService';
import { MovieAG } from '../../interfaces/movie';
import { PlaylistSubWindowComponent } from '../sub-playlist-window/playlist-sub-window.component';
import { movieActorsDto } from '../../Dto/movieActorsDto';

@Component({
  selector: 'app-movie-details',
  standalone: true,
  imports: [CommonModule, MatDialogModule, RouterModule],
  templateUrl: './movie-details.html',
  styleUrl: './movie-details.css',
})
export class MovieDetails implements OnInit{
  movie!: MovieAG;
  movieActors:movieActorsDto[]=[];
  loading = true;
  movieTmdbId!: number;

  constructor(
    private route: ActivatedRoute,
    private managemovieService: ManageMovieService,
    private dialog: MatDialog,
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe((params) => {
      this.movieTmdbId = Number(params.get('id'));
      this.loadSelectedMovie(this.movieTmdbId);
      this.loadMovieActors(this.movieTmdbId);
    });
  }

  loadSelectedMovie(movieTmdbId: number){
    this.managemovieService.getSelectedMovie(this.movieTmdbId).subscribe({
      next: (value) => {
        this.movie = value;
        this.loading = false;
      },
      error: (err) => {
        console.error(err);
        this.loading = false;
      },
    });
  }

  loadMovieActors(movieId: number){
    this.managemovieService.getMovieActors(movieId).subscribe({
      next: (value) => {
        this.movieActors = value;
        this.loading = false;
      },
      error: (err) => {
        console.error(err);
        this.loading = false;
      },
    })
  }

  RateGood(movieId: number) {
    this.managemovieService.rateMovie(movieId, 2).subscribe({
      next: () => console.log('Ocena zapisana 2'),
      error: (err) => console.error(err),
    });
  }

  RateNeutral(movieId: number) {
    this.managemovieService.rateMovie(movieId, 0).subscribe({
      next: () => console.log('Ocena zapisana 0'),
      error: (err) => console.error(err),
    });
  }

  RateBad(movieId: number) {
    this.managemovieService.rateMovie(movieId, -1).subscribe({
      next: () => console.log('Ocena zapisana -1'),
      error: (err) => console.error(err),
    });
  }

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

  removeRateFromMedia(movieId: number) {
    this.managemovieService.removeRateFromMedia(movieId).subscribe({
      next: () => console.log('removeRateFromMedia'),
      error: (err) => console.error(err),
    });
  }
}
