import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ManageMovieService } from '../../Services/ManageMovieService';

@Component({
  selector: 'app-create-or-add-to-playlist',
  imports: [CommonModule],
  templateUrl: './create-or-add-to-playlist.component.html',
  styleUrl: './create-or-add-to-playlist.component.css'
})
export class CreateOrAddToPlaylistComponent implements OnInit {

  loading = true;


constructor(
    private managemovieService: ManageMovieService
  ){}
  
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
