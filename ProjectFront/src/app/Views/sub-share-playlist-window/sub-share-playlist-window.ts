import { Component, OnInit, Inject } from '@angular/core';
import {
  MAT_DIALOG_DATA,
  MatDialogRef,
  MatDialogModule,
} from '@angular/material/dialog';
import { CommonModule } from '@angular/common';
import { FriendService } from '../../Services/FriendService';
import { SharePlaylistToFriendsService } from '../../Services/SharePlaylistToFriendsService';
import { FriendAG } from '../../interfaces/friend';
import { PlaylistFriendsDto } from '../../Dto/playlistFriendsDto';

@Component({
  selector: 'app-sub-share-playlist-window',
  standalone: true,
  imports: [CommonModule, MatDialogModule],
  templateUrl: './sub-share-playlist-window.html',
  styleUrl: './sub-share-playlist-window.css',
})
export class SubSharePlaylistWindow implements OnInit {
  loadingFriends = false;
  loadingPlaylist = false;
  loadingAction = false;

  friends: FriendAG[] = [];
  playlistFriends: PlaylistFriendsDto[]=[];

  playlistId!: number;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: { playlistId: number },
    private dialogRef: MatDialogRef<SubSharePlaylistWindow>,
    private friendService: FriendService,
    private sharePlaylistToFriend: SharePlaylistToFriendsService,
  ) {}

  ngOnInit(): void {
    this.playlistId = this.data.playlistId;
    this.loadFriends();
  }

  loadFriends() {
    this.loadingFriends = true;
    this.friendService.getFriends().subscribe({
      next: (data) => {
        this.friends = data;
        this.loadingFriends = false;
      },
      error: (err) => {
        console.error(err);
        alert('Nie udało się pobrać znajomych');
        this.loadingFriends = false;
      },
    });
  }

  loadPlaylistWithFriends() {
    this.loadingPlaylist = true;
    this.sharePlaylistToFriend.getPlaylistWithFriends(this.playlistId).subscribe({
      next: (data) => {
        this.playlistFriends = data;
        this.loadingPlaylist = false;
      },
      error: (err) => {
        console.error(err);
        alert('Nie udało się pobrać znajomych');
        this.loadingPlaylist = false;
      },
    });
  }

  sharePlaylistToFriends(friendId: string) {
    this.sharePlaylistToFriend
      .sharePlaylistWithFriends(this.playlistId, friendId)
      .subscribe({
        next: () => {
          this.loadingAction = false;
          console.log('Playlist udostępniona!');
          this.loadPlaylistWithFriends();
          // this.dialogRef.close();
        },
        error: (err) => {
          this.loadingAction = false;
          console.error(err);
          alert('Nie udało się udostępnić playlisty');
        },
      });
  }

  stopSharePlaylistToFriends(friendId: string) {
    this.loadingAction = true;

    this.sharePlaylistToFriend
      .stopSharePlaylistWithFriends(this.playlistId, friendId)
      .subscribe({
        next: () => {
          this.loadingAction = false;
          console.log('Playlist nieudostępniona!');
          // this.dialogRef.close();
          this.loadPlaylistWithFriends();
        },
        error: (err) => {
          console.error(err);
          this.loadingAction = false;
          alert('Nie udało się nieudostępniona playlisty');
        },
      });
  }



  close() {
    this.dialogRef.close();
  }
}
