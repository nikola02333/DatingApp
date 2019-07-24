import { AuthService } from './../_services/auth.service';
import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, CanActivate, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate  {

  constructor(private authService: AuthService, private router: Router,
              private toastr: ToastrService) {
  }
  canActivate(): boolean {
    if (this.authService.loggedId()) {
     return true;
    }
    this.toastr.error('You shall not pass!!!');
    this.router.navigate(['/home']);
    return false;
  }
}
