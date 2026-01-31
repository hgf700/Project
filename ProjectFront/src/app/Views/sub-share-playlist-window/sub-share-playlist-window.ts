import { Component, OnInit, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  MAT_DIALOG_DATA,
  MatDialogRef,
  MatDialogModule,
} from '@angular/material/dialog';
import { FormsModule } from '@angular/forms';
import { FriendService } from '../../Services/FriendService';
import { FriendAG } from '../../interfaces/friend';

@Component({
  selector: 'app-sub-share-playlist-window',
  imports: [],
  templateUrl: './sub-share-playlist-window.html',
  styleUrl: './sub-share-playlist-window.css',
})
export class SubSharePlaylistWindow implements OnInit{
  loading = false;
  friends: FriendAG[] = [];



  constructor(
      private friendService: FriendService,
      @Inject(MAT_DIALOG_DATA) public data: { tmdbId: number },
      private dialogRef: MatDialogRef<SubSharePlaylistWindow>,
  ) {}

  ngOnInit(): void {
    this.loadFriends();
  }

  loadFriends() {
    this.friendService.getFriends().subscribe({
      next: (data) => {
        this.friends = data;
      },
      error: (err) => {
        console.error(err);
        alert('Nie udało się pobrać znajomych');
      },
    });
  }



}
