import { Component } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { DevelopingLoginService } from '../../Services/DevelopingLoginService';
import { RouterModule } from '@angular/router';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-developing-view',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule,RouterModule],
  templateUrl: './developing-view.html',
  styleUrls: ['./developing-view.css'],
})
export class DevelopingView {
  developingLoginForm!: FormGroup;
  submitted = false;

  constructor(
    private fb: FormBuilder,
    private devLogin: DevelopingLoginService,
    private router: Router,
  ) {
    this.developingLoginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
    });
  }

  onSubmit() {
    this.submitted = true;

    console.log(this.developingLoginForm.errors);
    console.log(this.developingLoginForm.valid);

    const email = this.developingLoginForm.value.email;

    this.devLogin.developingLogin(email).subscribe({
  next: (res) => {
    localStorage.setItem('jwt', res.token);
    this.router.navigate(['/login-callback']); 
  },
  error: (err) => alert(err.error),
});

  }
}
