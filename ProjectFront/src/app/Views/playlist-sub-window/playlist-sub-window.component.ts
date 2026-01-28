import { Component, OnInit, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  MAT_DIALOG_DATA,
  MatDialogRef,
  MatDialogModule,
} from '@angular/material/dialog';
import { FormsModule } from '@angular/forms';
import { PlaylistService } from '../../Services/PlaylistService';
import { PlaylistAG } from '../../interfaces/playlist';

type ViewMode = 'list' | 'create';

@Component({
  selector: 'app-playlist-sub-window',
  standalone: true,
  imports: [CommonModule, MatDialogModule,FormsModule],
  templateUrl: './playlist-sub-window.component.html',
  styleUrl: './playlist-sub-window.component.css',
})
export class PlaylistSubWindowComponent implements OnInit {
  playlists: PlaylistAG[] = [];
  viewMode: ViewMode = 'list';
  loading = false;

  newPlaylistName = '';

  constructor(
    private playlistService: PlaylistService,
    @Inject(MAT_DIALOG_DATA) public data: { tmdbId: number },
    private dialogRef: MatDialogRef<PlaylistSubWindowComponent>
  ) {}

  // 🔹 lifecycle
  ngOnInit(): void {
    this.loadPlaylists();
  }

  // 🔹 data
  loadPlaylists() {
    this.loading = true;
    this.playlistService.getPlaylists().subscribe({
      next: (value) => {
        this.playlists = value;
        this.loading = false;
      },
      error: (err) => {
        console.error(err);
        this.loading = false;
      },
    });
  }

  // 🔹 UI state
  showList() {
    this.viewMode = 'list';
  }

  showCreate() {
    this.viewMode = 'create';
  }

  // 🔹 actions
  createPlaylist() {
    if (!this.newPlaylistName.trim()) return;

    this.loading = true;
    this.playlistService
      .createPlaylist(this.newPlaylistName)
      .subscribe({
        next: () => {
          this.newPlaylistName = '';
          this.viewMode = 'list';
          this.loadPlaylists();
        },
        error: (err) => {
          console.error(err);
          this.loading = false;
        },
      });
  }

  selectedPlaylistName: string | null = null;

  addToPlaylist(playlistId: number, playlistName: string) {
    this.selectedPlaylistName = playlistName;
    this.loading = true;

    this.playlistService
      .addMovieToPlaylist(playlistId, this.data.tmdbId)
      .subscribe({
        next: () => {
          this.dialogRef.close({
            success: true,
            playlistName,
          });
        },
        error: (err) => {
          console.error(err);
          this.loading = false;
        },
      });
  }

  close() {
    this.dialogRef.close();
  }
}