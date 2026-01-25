import { Component, OnInit } from '@angular/core';
import { ManageMovieService } from '../../Services/ManageMovieService';
import { Movie } from '../../interfaces/movie';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-manage-movie',
  imports: [CommonModule],
  templateUrl: './manage-movie.component.html',
  styleUrl: './manage-movie.component.css'
})
export class ManageMovieComponent implements OnInit {

  movies: Movie[]=[];
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
  RateGood(movieId: number) {}
  RateNeutral(movieId: number) {}
  RateBad(movieId: number) {}

  AddToPlaylist(movieId: number,userId: number){}

}
