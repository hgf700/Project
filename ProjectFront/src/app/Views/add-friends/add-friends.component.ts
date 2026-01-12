import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FriendService } from '../../Services/add-friend';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-add-friends',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterOutlet
  ],
  templateUrl: './add-friends.component.html',
  styleUrl: './add-friends.component.css'
})
export class AddFriendsComponent {

  addFriendForm!: FormGroup;
  submitted = false;

  constructor(
    private fb: FormBuilder,
    private friendService: FriendService
  ) {
    // inicjalizacja formularza
    this.addFriendForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]]
    });
  }

  onSubmit() {
    this.submitted = true;

    if (this.addFriendForm.invalid) return;

    this.friendService
      .addFriend(this.addFriendForm.value.email)
      .subscribe({
        next: () => alert('Zaproszenie wysłane'),
        error: err => alert(err.error)
      });
  }
}
