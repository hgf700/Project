import { Component, OnInit } from '@angular/core';
import { PhotoService } from '../../Services/PhotoService';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-show-movie-photo',
  templateUrl: './show-movie-photo.component.html',
  styleUrl: './show-movie-photo.component.css',
  imports: [
    CommonModule
  ]
})
export class ShowMoviePhotoComponent implements OnInit {

  images: { posterPath: string | null; backdropPath: string | null }[] = [];

  constructor(private photoService: PhotoService) {}

  ngOnInit() {
    this.photoService.getImages().subscribe({
      next: data => this.images = data,
      error: err => console.error(err)
    });
  }

  getPosterUrl(path: string | null) {
    if (!path) return 'assets/no-poster.jpg';
    return `https://image.tmdb.org/t/p/w500${path}`;
  }

  getBackdropUrl(path: string | null) {
    if (!path) return '';
    return `https://image.tmdb.org/t/p/w1280${path}`;
  }
}
