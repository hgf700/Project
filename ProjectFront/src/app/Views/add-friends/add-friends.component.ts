import { Component } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FriendService } from '../../Services/FriendService';

@Component({
  selector: 'app-add-friends',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './add-friends.component.html',
  styleUrls: ['./add-friends.component.css'],
})
export class AddFriendsComponent {
  addFriendForm!: FormGroup;
  submitted = false;

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

  friends: { friendUserId: string; email: string }[] = [];

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
