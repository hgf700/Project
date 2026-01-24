import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginCallbackComponent } from '../Views/login-callback/login-callback.component';
import { LoginComponent } from '../Views/login/login.component';
import { AddFriendsComponent } from '../Views/add-friends/add-friends.component';
import { AddMoviesTMDBComponent } from '../Views/add-movies-tmdb/add-movies-tmdb.component';




export const routes: Routes = [
  { path: '', component: LoginComponent },
  { path: 'login-callback', component: LoginCallbackComponent },
  { path: 'add-friends', component: AddFriendsComponent },
  { path: 'add-movies-tmdb', component: AddMoviesTMDBComponent }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}