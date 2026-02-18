import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-movie-recomendations',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './movie-recomendations.html',
  styleUrl: './movie-recomendations.css',
})
export class MovieRecomendations implements OnInit{

  constructor(

  ) {}


  ngOnInit(): void {

  }
}
