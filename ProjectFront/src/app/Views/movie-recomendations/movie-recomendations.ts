import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RecomendationMoviesService } from '../../Services/RecomendationMoviesService';
import { MovieRecommendationsDto } from '../../Dto/movieRecommendationsDto';

@Component({
  selector: 'app-movie-recomendations',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './movie-recomendations.html',
  styleUrl: './movie-recomendations.css',
})
export class MovieRecomendations implements OnInit{
  movieRecommendations: MovieRecommendationsDto[]=[];
  loading = true;

  constructor(
    private recomendMoviesService: RecomendationMoviesService,

  ) {}


  ngOnInit(): void {
    this.loadMovieRecommendations();
  }

  loadMovieRecommendations(){
    this.recomendMoviesService.getMovieRecommendation().subscribe({
      next: (value) => {
        this.movieRecommendations = value;
        this.loading = false;
      },
      error: (err) => {
        console.error(err);
        this.loading = false;
      },
    });
  }
}
