import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { GenreSeedService } from '../../Services/GenreSeedService';
import { MovieService } from '../../Services/MovieService';

@Component({
  selector: 'app-download',
  imports: [
    CommonModule,
    RouterModule
  ],
  templateUrl: './download.component.html',
  styleUrl: './download.component.css'
})

export class DownloadComponent implements OnInit {
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private genreseedService: GenreSeedService,
    private moviesService: MovieService
  ) {}

    ngOnInit(): void {
  
      this.moviesService.importFromTmdb().subscribe({
      next: () => alert('Filmy zapisane do bazy'),
      error: err => {
        alert(err.error || 'Błąd importu');
      }
    });

    this.genreseedService.importSeedFromTmdb().subscribe({
      next: () => alert('genre zapisane do bazy'),
      error: err => {
        alert(err.error || 'Błąd importu');
      }
    });

  }

}

