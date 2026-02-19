import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { RouterModule } from '@angular/router';
import { ActivatedRoute, Router } from '@angular/router';
import { ManageMovieService } from '../../Services/MovieManageService';
import { MovieAG } from '../../interfaces/movie';
import { PlaylistSubWindowComponent } from '../sub-playlist-window/playlist-sub-window.component';
import { movieActorsDto } from '../../Dto/movieActorsDto';

@Component({
  selector: 'app-manage-movie',
  standalone: true,
  imports: [CommonModule, MatDialogModule, RouterModule],
  templateUrl: './manage-movie.component.html',
  styleUrl: './manage-movie.component.css',
})
export class ManageMovieComponent implements OnInit {
  movies: MovieAG[] = [];
  movieActors:movieActorsDto[]=[];
  loading = true;

  constructor(
    private managemovieService: ManageMovieService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadMovies();
  }

  loadMovies(){
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

  showMovieDetails(movieTmdbId: number) {
    this.router.navigate(['/movie-details', movieTmdbId]);
  }
}
