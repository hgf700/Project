import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { DownloadService } from '../../Services/DownloadService';

@Component({
  selector: 'app-download',
  imports: [CommonModule, RouterModule],
  templateUrl: './download.component.html',
  styleUrl: './download.component.css',
})
export class DownloadComponent implements OnInit {
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private downloadService: DownloadService,
  ) {}

  ngOnInit(): void {
    this.downloadService.importFromTmdb().subscribe({
      next: () => alert('Filmy zapisane do bazy'),
      error: (err) => {
        alert(err.error || 'Błąd importu');
      },
    });

    this.downloadService.importSeedFromTmdb().subscribe({
      next: () => alert('genre zapisane do bazy'),
      error: (err) => {
        alert(err.error || 'Błąd importu');
      },
    });
  }
}
