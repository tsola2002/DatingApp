import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { User } from '../_models/user';


@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl = environment.apiUrl;

  // in our constructor we will make use of our http client service
constructor(private http: HttpClient) { }

// method to get users which returns an observable(import from rxjs is required)
// the observable is an array of type user(this must be imported as well)
// authorization token needs to send along with the request
getUsers(): Observable<User[]> {
  return this.http.get<User[]>(this.baseUrl + 'users');
}

// method to get detailed user which will take in an id
// authorization token needs to send along with the request
getUser(id): Observable<User> {
  return this.http.get<User>(this.baseUrl + 'users/' + id);
}

// it will take in userId of type number & user of type user
// it will use the httpPut method
updateUser(id: number, user: User) {
  return this.http.put(this.baseUrl + 'users/' + id, user);
}

// iy will take in the user id & the photo id of types number
setMainPhoto(userId: number, id: number) {
  return this.http.post(this.baseUrl + 'users/' + userId + '/photos/' + id + '/setMain', {});
}

}
