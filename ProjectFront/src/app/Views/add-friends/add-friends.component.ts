import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FriendService } from '../../Services/add-friend';


@Component({
  selector: 'app-add-friends',
  imports: [],
  templateUrl: './add-friends.component.html',
  styleUrl: './add-friends.component.css'
})
export class AddFriendsComponent{

  addFriendForm!: FormGroup;
  submitted = false;

  constructor(
    private fb: FormBuilder,
    private friendService: FriendService
  ) {}

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

