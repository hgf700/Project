import { Component ,OnInit} from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FriendService } from '../../Services/FriendService';
import { SocialManageService } from '../../Services/SocialManageService';
import { FriendAG } from '../../interfaces/friend';

@Component({
  selector: 'app-manage-friends',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule,RouterModule],
  templateUrl: './manage-friends.html',
  styleUrl: './manage-friends.css',
})
export class ManageFriends implements OnInit {
  addFriendForm!: FormGroup;
  submitted = false;
  friends: FriendAG[] = [];

  constructor(
    private fb: FormBuilder,
    private friendService: FriendService,
    private manageSocialService: SocialManageService,
    private router: Router,
  ) {
    // inicjalizacja formularza
    this.addFriendForm = this.fb.group({
      email: [''],
    });
  }

  ngOnInit(): void {
    this.loadFriends();
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

  onSubmit() {
    this.submitted = true;

    if (this.addFriendForm.invalid) return;

    this.friendService.addFriend(this.addFriendForm.value.email).subscribe({
      next: () => {
        alert('Zaproszenie wysłane')
        this.loadFriends();
      },
      error: (err) => alert(err.error),
    });
  }

  deleteFriend(friendId: string) {
    this.friendService.deleteFriend(friendId).subscribe({
      next: () => {
        this.loadFriends();
      },
      error: (err) => {
        console.error(err);
      },
    });
  }

  showFriendSocialAccount(friendId: string) {
    this.router.navigate(['/view-friend-profile',friendId]);
  }
  
}
