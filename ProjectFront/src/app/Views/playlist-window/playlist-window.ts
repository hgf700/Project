import { Component, OnInit, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PlaylistService } from '../../Services/PlaylistService';
import { PlaylistAG } from '../../interfaces/playlist';
import { PlaylistResultAG } from '../../interfaces/playlistResult';

@Component({
  selector: 'app-playlist-window',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './playlist-window.html',
  styleUrl: './playlist-window.css',
})
export class PlaylistWindow implements OnInit {
  playlists: PlaylistAG[] = [];
  selectedPlaylist?: PlaylistResultAG;
  loading = false;

  newPlaylistName = '';

  constructor(private playlistService: PlaylistService) {}

  ngOnInit(): void {
    this.loadPlaylists();
  }

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

  createPlaylist() {
    if (!this.newPlaylistName.trim()) return;
    this.loading = true;
    this.playlistService.createPlaylist(this.newPlaylistName).subscribe({
      next: () => {
        this.newPlaylistName = '';
        this.loadPlaylists();
      },
      error: (err) => {
        console.error(err);
        this.loading = false;
      },
    });
  }

  showSelectedPlaylist(playlistId: number) {
    this.loading = true;

    this.playlistService.showResultFromPlaylist(playlistId).subscribe({
      next: (value) => {
        this.selectedPlaylist = value;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      },
    });
  }

  deleteFromPlaylist(playlistId: number, movieId: number) {
    this.loading = true;

    this.playlistService.deleteFromPlaylist(playlistId, movieId).subscribe({
      next: () => {
        this.loadPlaylists();
      },
      error: (err) => {
        console.error(err);
        this.loading = false;
      },
    });
  }
}
