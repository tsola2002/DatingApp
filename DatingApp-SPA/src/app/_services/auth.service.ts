import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {BehaviorSubject} from 'rxjs';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';

// allows us to inject different thing into out service
// service are not automatically injectable like components so we have to use the
// @injectable decorator
@Injectable({
  // this our services & any components using this service which module is providing this service
  providedIn: 'root'
})


export class AuthService {
  // we store our baseurl in a variable
  baseUrl = environment.apiUrl + 'auth/';
  jwtHelper = new JwtHelperService();
  decodedToken: any;
  currentUser: User;
  photoUrl = new BehaviorSubject<string>('../../assets/user.png');
  currentPhotoUrl = this.photoUrl.asObservable();

// we need to inject the http client service
constructor(private http: HttpClient) { }

changeMemberPhoto(photoUrl: string) {
  this.photoUrl.next(photoUrl);
}

// login method will take in model object being passed in from navbar form
login( model: any) {
  // we replicate http.post from postman in our service
  // in the body of this request we send our model object
  // the third part of the request allows to specify any option
  // we want to send as part of the request(like header, authorization e.t.c.)
  // no third parameter
  // pipe method will allow us to chain rxjs operators to our request
  // we store the response object in the const user variable
  return this.http.post(this.baseUrl + 'login', model).pipe(
    map((response: any) => {
      const user = response;
      // if theres a user then store the token in local storage
      if (user) {
        localStorage.setItem('token', user.token);
        // we convert the user object into a string using jsonStringify
        localStorage.setItem('user', JSON.stringify(user.user));
        this.decodedToken = this.jwtHelper.decodeToken(user.token);
        this.currentUser = user.user;
        this.changeMemberPhoto(this.currentUser.photoUrl);
        // console.log(this.decodedToken);
        // console.log(this.currentUser);
      }
    })
  );
}

register(model: any) {
  return this.http.post(this.baseUrl + 'register', model);
}

loggedIn() {
  const token = localStorage.getItem('token');
  return !this.jwtHelper.isTokenExpired(token);
}
}
