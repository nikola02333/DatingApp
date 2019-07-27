import { UserService } from './../_services/user.service';
import {Injectable} from '@angular/core';
import { User } from '../_models/user';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class MemberDetailResolver implements Resolve<User> {

    constructor(private userService: UserService,
                private router: Router,
                private toast: ToastrService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<User> {
        return this.userService.getUser( route.params.id).pipe(
            catchError( error => {
                this.toast.error(error);
                this.router.navigate(['/members']);
                return of (null);
            })
        );
    }
}
