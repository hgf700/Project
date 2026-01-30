import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginCallbackComponent } from '../Views/login-callback/login-callback.component';
import { LoginComponent } from '../Views/login/login.component';
import { ShowMoviePhotoComponent } from '../Views/show-movie-photo/show-movie-photo.component';
import { ManageMovieComponent } from '../Views/manage-movie/manage-movie.component';
import { DownloadComponent } from '../Views/download/download.component';
import { PlaylistWindow } from '../Views/playlist-window/playlist-window';
import { ManageFriends } from '../Views/manage-friends/manage-friends';

export const routes: Routes = [
  { path: '', component: LoginComponent },
  { path: 'login-callback', component: LoginCallbackComponent },
  { path: 'show-movie-photo', component: ShowMoviePhotoComponent },
  { path: 'manage-movie', component: ManageMovieComponent },
  { path: 'download', component: DownloadComponent },
  { path: 'playlist-window', component: PlaylistWindow },
  { path: 'manage-friends', component: ManageFriends },

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
