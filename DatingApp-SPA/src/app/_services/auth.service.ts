import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

//allows us to inject different thing into out service
//service are not automatically injectable like components so we have to use the 
//@injectable decorator
@Injectable({
  //this our services & any components using this service which module is providing this service
  providedIn: 'root'
})


export class AuthService {
  //we store our baseurl in a variable
  baseUrl = "http://localhost:5000/api/auth/";

//we need to inject the http client service  
constructor(private http: HttpClient) { }

//login method will take in model object being passed in from navbar form
login( model: any) {
  //we replicate http.post from postman in our service
  //in the body of this request we send our model object
  //the third part of the request allows to specify any option
  //we want to send as part of the request(like header, authorization e.t.c.)
  //no third parameter
  //pipe method will allow us to chain rxjs operators to our request
  //we store the response object in the const user variable
  return this.http.post(this.baseUrl + 'login', model).pipe(
    map((response: any) => {
      const user = response;
      //if theres a user then store the token in local storage
      if (user) {
        localStorage.setItem('token', user.token);
      }
    })
  )
}

}
