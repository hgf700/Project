import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RecomendationMoviesService } from '../../Services/RecomendationMoviesService';
// import { MovieRecommendationsDto } from '../../Dto/movieRecommendationsDto';

@Component({
  selector: 'app-movie-recommendations',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './movie-recommendations.html',
  styleUrl: './movie-recommendations.css',
})
export class MovieRecomendations implements OnInit{
  // movieRecommendations: MovieRecommendationsDto[]=[];
  loading = true;
  recommendations: string[] = [];

  constructor(
    private recomendMoviesService: RecomendationMoviesService,
  ) {}

  ngOnInit(): void {
    this.startMovieRecommendationsML();
    // this.receiveMovieRecommendations();
  }

  startMovieRecommendationsML(){
    this.recomendMoviesService.postMovieRecommendationByML().subscribe({
      next: (data) => {
        this.recommendations = data;
        this.loading = false;
      },
      error: (err) => console.error(err),
    });
  }

  // receiveMovieRecommendations(){
  //   this.recomendMoviesService.postReceiveRecommendationsFromPy().subscribe({
  //     next: (data) => {
  //       this.recommendations = data;
  //       this.loading = false;
  //     },
  //     error: (err) => console.error(err),
  //   });
  // }

}
