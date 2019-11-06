import { AuthService } from 'src/app/_services/auth.service';
import { Message } from './../_models/message';
import { UserService } from '../_services/user.service';
import {Injectable} from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class MessagesResolver implements Resolve<Message []> {

    pageNumber = 1;
    pageSize = 5;
    messageContainer = 'Unread';

    constructor(private userService: UserService,
                private router: Router,
                private toast: ToastrService,
                private authService: AuthService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<Message []> {
        return this.userService.getMessages(this.authService.decodedToken.nameid,
                                            this.pageNumber, this.pageSize,
                                            this.messageContainer).pipe(
            catchError( error => {
                this.toast.error('Problem retriving messages');
                this.router.navigate(['/home']);
                return of (null);
            })
        );
    }
}
