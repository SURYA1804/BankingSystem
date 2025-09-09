import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, Router } from '@angular/router';

type AppRole = 'customer' | 'staff' | 'manager' ;

@Injectable({ providedIn: 'root' })
export class RoleGuard implements CanActivate {
  constructor(private router: Router) {}

  private getUserRoles(): string[] {
    const raw = localStorage.getItem('UserDetails');
    if (raw) {
      try {
        const user = JSON.parse(raw) as { roleName?: string; roles?: string[] };
        if (user?.roles?.length) return user.roles;
        if (user?.roleName) return [user.roleName];
      } catch {}
    }
    const token = localStorage.getItem('token');
    if (token) {
      try {
        const payload = JSON.parse(atob(token.split('.')[21]));
        const roles = payload['roles'] || payload['role'] || payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
        if (Array.isArray(roles)) return roles;
        if (typeof roles === 'string') return [roles];
      } catch {}
    }
    return [];
  }

  canActivate(route: ActivatedRouteSnapshot): boolean {
    const IsOTPVerified = localStorage.getItem("IsOTPVerified")

    const required = (route.data['roles'] as AppRole[] | undefined) ?? [];
    const roles = this.getUserRoles();
    if(IsOTPVerified == "false" || IsOTPVerified == null)
    {
          this.router.navigate(['/OtpRequired']);
          return false;
    }
    const allowed = required.length === 0 || required.some(r => roles.includes(r));
    if (allowed) 
      {
        return true
      };

    this.router.navigate(['/unauthorized']);
    return false;



  }
}
