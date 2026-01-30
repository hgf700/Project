import { Component } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FriendService } from '../../Services/FriendService';
import { FriendAG } from '../../interfaces/friend';

@Component({
  selector: 'app-manage-friends',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './manage-friends.html',
  styleUrl: './manage-friends.css',
})
export class ManageFriends {
  addFriendForm!: FormGroup;
  submitted = false;
  friends: FriendAG[] = [];

  constructor(
    private fb: FormBuilder,
    private friendService: FriendService,
  ) {
    // inicjalizacja formularza
    this.addFriendForm = this.fb.group({
      email: [''],
    });
  }

  onSubmit() {
    this.submitted = true;

    if (this.addFriendForm.invalid) return;

    this.friendService.addFriend(this.addFriendForm.value.email).subscribe({
      next: () => alert('Zaproszenie wysłane'),
      error: (err) => alert(err.error),
    });
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

  deleteFriend(friendId: string){
    this.friendService.deleteFriend(friendId).subscribe({
      next: () => {
        this.loadFriends();
      },
      error: (err) => {
        console.error(err);
      },
    })
  }

  
}
