import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginCallbackComponent } from '../Views/login-callback/login-callback.component';
import { LoginComponent } from '../Views/login/login.component';
import { ShowMoviePhotoComponent } from '../Views/show-movie-photo/show-movie-photo.component';
import { ManageMovieComponent } from '../Views/manage-movie/manage-movie.component';
import { DownloadComponent } from '../Views/download/download.component';
import { PlaylistWindow } from '../Views/playlist-window/playlist-window';
import { ManageFriends } from '../Views/manage-friends/manage-friends';
import { DevelopingView } from '../Views/developing-view/developing-view';
import { ViewFriendProfile } from '../Views/view-friend-profile/view-friend-profile';
import { PrefferedGenre } from '../Views/preffered-genre/preffered-genre';
import { MovieRecomendations } from '../Views/movie-recomendations/movie-recomendations';
import { MovieDetails } from '../Views/movie-details/movie-details';



export const routes: Routes = [
  { path: '', component: LoginComponent },
  { path: 'login-callback', component: LoginCallbackComponent },
  { path: 'show-movie-photo', component: ShowMoviePhotoComponent },
  { path: 'manage-movie', component: ManageMovieComponent },
  { path: 'movie-details/:id', component: MovieDetails },
  { path: 'download', component: DownloadComponent },
  { path: 'playlist-window', component: PlaylistWindow },
  { path: 'manage-friends', component: ManageFriends },
  { path: 'developing-view', component: DevelopingView },
  { path: 'view-friend-profile/:id', component: ViewFriendProfile },
  { path: 'preffered-genre', component: PrefferedGenre },
  { path: 'movie-recomendations', component: MovieRecomendations },


];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
