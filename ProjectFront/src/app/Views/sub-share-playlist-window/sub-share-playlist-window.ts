import { Component, OnInit, Inject } from '@angular/core';
import {
  MAT_DIALOG_DATA,
  MatDialogRef,
  MatDialogModule,
} from '@angular/material/dialog';
import { CommonModule } from '@angular/common';
import { FriendService } from '../../Services/FriendService';
import { ManageSocialService } from '../../Services/ManageSocialService';
import { FriendAG } from '../../interfaces/friend';

@Component({
  selector: 'app-sub-share-playlist-window',
  standalone: true,
  imports: [CommonModule,MatDialogModule],
  templateUrl: './sub-share-playlist-window.html',
  styleUrl: './sub-share-playlist-window.css',
})
export class SubSharePlaylistWindow implements OnInit{
  loading = false;
  friends: FriendAG[] = [];

  constructor(
      @Inject(MAT_DIALOG_DATA) public data: { playlistId: number },
      private dialogRef: MatDialogRef<SubSharePlaylistWindow>,
      private friendService: FriendService,
      private manageSocialService: ManageSocialService
  ) {}

  ngOnInit(): void {
    this.loadFriends();
  }

  loadFriends() {
    this.loading = true;
    this.friendService.getFriends().subscribe({
      next: (data) => {
        this.friends = data;
        this.loading = false;
      },
      error: (err) => {
        console.error(err);
        alert('Nie udało się pobrać znajomych');
        this.loading = false;
      },
    });
  }

  sharePlaylistToFriends(friendId: string){
    const playlistId = this.data.playlistId;
    this.manageSocialService.sharePlaylistWithFriends(playlistId, friendId).subscribe({
      next: () => {
        alert('Playlist udostępniona!');
        this.dialogRef.close();
      },
      error: (err) => {
        console.error(err);
        alert('Nie udało się udostępnić playlisty');
      },
    });
  }

  close() {
    this.dialogRef.close();
  }
}
