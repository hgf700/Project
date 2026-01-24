import { Component } from '@angular/core';

import { GenreSeedService } from '../../Services/GenreSeedService';

@Component({
  selector: 'app-seed-genre',
  imports: [],
  templateUrl: './seed-genre.component.html',
  styleUrl: './seed-genre.component.css'
})
export class SeedGenreComponent {
  constructor(private genreseedService: GenreSeedService) {}
  
  DownloadGenreSeed() {
    this.genreseedService.importSeedFromTmdb().subscribe({
      next: () => alert('genre zapisane do bazy'),
      error: err => {
        alert(err.error || 'Błąd importu');
      }
    });
  }
}
