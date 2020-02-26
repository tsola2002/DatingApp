import {Injectable} from '@angular/core';
import { User } from '../_models/user';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
// the member detailed resolver will resolve to a user
export class MemberDetailResolver implements Resolve<User> {
    // in the constructor we bring userService to get the user, Router, & Alertify
    constructor(private userService: UserService, private router: Router,
        private alertify: AlertifyService) {}

    // resolve method needs a route from ActivatedRouteSnapshot
    // the method will return an observable of type user
    resolve(route: ActivatedRouteSnapshot): Observable<User> {
        // we need to get the id from the get User method
        // when using a resolve it automatically subscribes to the method
        // we get the data from the route instead of using the userservice to get it
        return this.userService.getUser(route.params['id']).pipe(
            // we catch any errors that ouccur so that we can redirect the user back
            // get out of this method as well(so we use pipe method for that)
            catchError(error => {
                // display alertify modal error box
                this.alertify.error('Problem retrieving data');
                // we navigate the user back to the members page
                this.router.navigate(['/members']);
                // we return an observable of null(using of operator from rxjs library)
                return of(null);
            })
        );
    }
}
