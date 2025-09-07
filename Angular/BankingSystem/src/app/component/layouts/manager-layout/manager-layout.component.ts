import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-manager-layout',
  imports: [RouterModule],
  templateUrl: './manager-layout.component.html',
  styleUrl: './manager-layout.component.css'
})
export class ManagerLayoutComponent {
  constructor(private router: Router) {}

  logout() {
    localStorage.clear();
    this.router.navigate(['/login']);
  }
}
