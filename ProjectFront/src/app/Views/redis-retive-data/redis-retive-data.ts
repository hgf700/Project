import { RetrieveRedisData } from '../../Services/RetrieveRedisData';
import { Component, OnInit, Inject } from '@angular/core';
import { retrieveRedisDataDto } from '../../Dto/retrieveRedisDataDto';
import { CommonModule } from '@angular/common';
import { UserActionType } from '../../enum/userActionType';
import { ObjectType } from '../../enum/objectType';


@Component({
  selector: 'app-redis-retive-data',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './redis-retive-data.html',
  styleUrl: './redis-retive-data.css',
})
export class RedisRetiveData implements OnInit{
  redisdata: retrieveRedisDataDto[]=[];
  loading = false;  
  
  constructor(
    private retrieveRedis: RetrieveRedisData,
  ) {}

  ngOnInit(): void {
    this.loadRedisData();
  }

  loadRedisData() {
    this.loading = true;
    this.retrieveRedis.getRedisData().subscribe({
      next: (value) => {
        console.log(value); 
        this.redisdata = value.map(x => ({ ...x, createdDate: new Date(x.createdDate) }));
        this.loading = false;
      },
      error: (err) => {
        console.error(err);
        this.loading = false;
      },
    });
  }

  getMediaText(action: ObjectType): string{
    switch (action) {
      case ObjectType.Movie:
        return 'film ';
      case ObjectType.Playlist:
        return 'playlsita';
      default:
        return 'media';
      }
  }

   getActionText(action: UserActionType): string {
    switch (action) {
      case UserActionType.RateCreated:
        return 'ocenił film';
      case UserActionType.RateRemoved:
        return 'usunął ocenę filmu';
      case UserActionType.CommentCreated:
        return 'dodał komentarz';
      case UserActionType.CommentRemoved:
        return 'usunął komentarz';
      case UserActionType.PostCreated:
        return 'dodał post';
      case UserActionType.PostRemoved:
        return 'usunął post';
      case UserActionType.PlaylistLiked:
        return 'polubił playlistę';
      case UserActionType.PlaylistUnliked:
        return 'usunął polubienie playlisty';
      case UserActionType.PlaylistAdded:
        return 'dodał element do playlisty';
      case UserActionType.PlaylistCreated:
        return 'utworzył playlistę';
      case UserActionType.PlaylistMadePublic:
        return 'ustawił playlistę jako publiczną';
      case UserActionType.PlaylistSharedWithFriends:
        return 'udostępnił playlistę znajomym';
      case UserActionType.PlaylistUnsharedWithFriends:
        return 'przestał udostępniać playlistę';
      case UserActionType.PlaylistValueDeleted:
        return 'usunął element z playlisty';
      default:
        return 'wykonał akcję';
    }
  }

}
