import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ManageMovieService } from '../../Services/ManageMovieService';

@Component({
  selector: 'app-playlist-sub-window',
  imports: [CommonModule],
  templateUrl: './playlist-sub-window.component.html',
  styleUrl: './playlist-sub-window.component.css'
})
export class PlaylistSubWindowComponent implements OnInit {

  loading = true;

constructor(
    private managemovieService: ManageMovieService
  ){}

// pod okno docs 
// https://material.angular.dev/components/dialog/overview

  ngOnInit(): void {
      // this.managemovieService.getMovies().subscribe({
      //   next: value=>{
      //     this.movies=value;
      //     this.loading=false;
      //   },
      //   error: err =>{
      //     console.error(err);
      //     this.loading=false;
      //   }
      // })
  }

}