import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PlaylistService } from '../../Services/PlaylistService';
import { PlaylistAG } from '../../interfaces/playlist';

@Component({
  selector: 'app-playlist-sub-window',
  imports: [CommonModule],
  templateUrl: './playlist-sub-window.component.html',
  styleUrl: './playlist-sub-window.component.css'
})
export class PlaylistSubWindowComponent implements OnInit {
  playlists: PlaylistAG[]=[];
  loading = true;

constructor(
    private playlistService: PlaylistService
  ){}

// pod okno docs 
// https://material.angular.dev/components/dialog/overview

  ngOnInit(): void {
    this.playlistService.getPlaylists().subscribe({
      next: value=>{
        this.playlists=value;
        this.loading=false;
      },
      error: err =>{
        console.error(err);
        this.loading=false;
      }
    })
  }
}