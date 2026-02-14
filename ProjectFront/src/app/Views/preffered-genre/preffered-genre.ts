import { ManageGenresService } from '../../Services/ManageGenresService';
import { Component, OnInit, Inject } from '@angular/core';
import { allGenresDto } from '../../Dto/allGenresDto';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-preffered-genre',
  imports: [CommonModule],
  templateUrl: './preffered-genre.html',
  styleUrl: './preffered-genre.css',
})
export class PrefferedGenre implements OnInit{
  genres: allGenresDto[] = [];
  loading = false;  
  
  constructor(
    private manageGenre: ManageGenresService,
  ) {}

  ngOnInit(): void {
    this.loadGenres();
  }

  loadGenres() {
    this.loading = true;
    this.manageGenre.getGenres().subscribe({
      next: (value) => {
        this.genres = value;
        this.loading = false;
      },
      error: (err) => {
        console.error(err);
        this.loading = false;
      },
    });
  }

  chooseGenre(genreId: number) {
    this.loading = true;
    this.manageGenre.chooseGenre(genreId).subscribe({
      next: () => {
        this.loadGenres();
      },
      error: (err) => {
        console.error(err);
        this.loading = false;
      },
    });
  }

  removeChoosenGenre(genreId: number) {
    this.loading = true;
    this.manageGenre.deleteChoosenGenre(genreId).subscribe({
      next: () => {
        this.loadGenres();
      },
      error: (err) => {
        console.error(err);
        this.loading = false;
      },
    });
  }

  
}
