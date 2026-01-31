import { Component, OnInit, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { PlaylistService } from '../../Services/PlaylistService';
import { PlaylistAG } from '../../interfaces/playlist';
import { PlaylistResultAG } from '../../interfaces/playlistResult';
import { SubSharePlaylistWindow } from '../sub-share-playlist-window/sub-share-playlist-window';

@Component({
  selector: 'app-playlist-window',
  standalone: true,
  imports: [CommonModule, FormsModule,MatDialogModule],
  templateUrl: './playlist-window.html',
  styleUrl: './playlist-window.css',
})
export class PlaylistWindow implements OnInit {
  playlists: PlaylistAG[] = [];
  selectedPlaylist?: PlaylistResultAG;
  loading = false;

  newPlaylistName = '';

  constructor(private playlistService: PlaylistService,
    private dialog: MatDialog,
  ) {}

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
  
  deletePlaylist(playlistId: number){
    this.playlistService.deletePlaylist(playlistId).subscribe({
      next: () => {
        this.loadPlaylists();
      },
      error: (err) => {
        console.error(err);
        this.loading = false;
      },
    });
  }

  openShareDialog(playlistId: number) {
      const dialogRef = this.dialog.open(SubSharePlaylistWindow, {
        width: '600px',
        height: '400px',
        data: {
          playlistId
        },
      });
  
      dialogRef.afterClosed().subscribe((result) => {
        console.log('Dialog closed:', result);
      });
    }
}