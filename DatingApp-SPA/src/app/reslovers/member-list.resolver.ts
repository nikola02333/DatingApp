import { UserService } from './../_services/user.service';
import {Injectable} from '@angular/core';
import { User } from '../_models/user';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class MemberListResolver implements Resolve<User []> {

    pageNumber = 1;
    pageSize = 5;

    constructor(private userService: UserService,
                private router: Router,
                private toast: ToastrService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<User []> {
        return this.userService.getUsers( this.pageNumber, this.pageSize).pipe(
            catchError( error => {
                this.toast.error('Problem retriving data');
                this.router.navigate(['/home']);
                return of (null);
            })
        );
    }
}
