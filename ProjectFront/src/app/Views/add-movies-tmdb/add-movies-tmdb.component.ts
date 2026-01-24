import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MovieService } from '../../Services/MovieService';

@Component({
  selector: 'app-add-movies-tmdb',
  imports: [],
  templateUrl: './add-movies-tmdb.component.html',
  styleUrl: './add-movies-tmdb.component.css'
})
export class AddMoviesTMDBComponent {

  constructor(private moviesService: MovieService) {}
  
  DownloadFilms() {
    this.moviesService.importFromTmdb().subscribe({
      next: () => alert('Filmy zapisane do bazy'),
      error: err => {
        alert(err.error || 'Błąd importu');
      }
    });
  }
}
