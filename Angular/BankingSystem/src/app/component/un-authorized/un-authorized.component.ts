import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-unauthorized',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './un-authorized.component.html',
  styleUrls: ['./un-authorized.component.css']
})
export class UnauthorizedComponent {
  constructor(private router: Router) {}

  back() {
    this.router.navigate(['/']);
  }

  login() {
    this.router.navigate(['/login']);
  }
}
