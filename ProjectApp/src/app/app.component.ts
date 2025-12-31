import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'ProjectApp';
  loginWithGoogle() {
  // Otwórz endpoint API w nowym oknie lub przekieruj
  window.location.href = 'https://localhost:7218/auth/signin-google';
}

}
