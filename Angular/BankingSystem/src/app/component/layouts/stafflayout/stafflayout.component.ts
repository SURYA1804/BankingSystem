import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-stafflayout',
  imports: [RouterModule],
  templateUrl: './stafflayout.component.html',
  styleUrl: './stafflayout.component.css'
})
export class StafflayoutComponent {
  constructor(private router: Router) {}

  logout() {
    localStorage.clear();
    this.router.navigate(['/login']);
  }
}
