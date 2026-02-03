import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { ProfileMessageService } from '../../Services/ProfileMessageService';
import { profileMessageDto } from '../../Dto/profileMessageDto';

@Component({
  selector: 'app-view-friend-profile',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule],
  templateUrl: './view-friend-profile.html',
  styleUrl: './view-friend-profile.css',
})
export class ViewFriendProfile implements OnInit {
  writeCommentForm!: FormGroup;
  loading = false;
  profileMessage: profileMessageDto[] = [];
  friendId!: string;
  submitted = false;

  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private profileMessageServ: ProfileMessageService,

    
  ) {
    this.writeCommentForm = this.fb.group({
      text: [''],
    });
  }

  ngOnInit() {
    this.route.paramMap.subscribe((params) => {
      this.friendId = params.get('id')!;
      this.loadComments();
    });
  }

  loadComments() {
    this.loading = true;
    this.profileMessageServ.getProfileMessage(this.friendId).subscribe({
      next: (value) => {
        this.profileMessage = value;
        this.loading = false;
      },
      error: (err) => {
        console.error(err);
        this.loading = false;
      },
    });
  }

  onSubmit() {
    this.submitted = true;

    if (this.writeCommentForm.invalid) return;

    this.profileMessageServ
      .writeProfileMessage(this.friendId, this.writeCommentForm.value.text)
      .subscribe({
        next: () => {
          alert('wiadomosc wysłane');
          this.loadComments();
        },
        error: (err) => alert(err.error),
      });
  }

  deleteProfileMessage(messageId: number) {
    this.loading = true;
    this.profileMessageServ.deleteProfileMessage(messageId).subscribe({
      next: () => {
        this.loadComments();
        this.loading = false;
      },
      error: (err) => {
        console.error(err);
        this.loading = false;
      },
    });
  }

  followFriend() {}

  unfollowFriend() {}
}
