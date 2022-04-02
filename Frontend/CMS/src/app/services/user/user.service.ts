import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { User } from 'src/app/models/user';

const baseUrl = 'https://localhost:7084/api' ;
const service = {
  'Auth': "Auth",
  'User': "User",
};

interface logInSessionToken{
  token : string;
}
@Injectable({
  providedIn: 'root'
})
export class UserService {

  private userSubject: BehaviorSubject<User>;
  public user: Observable<User>;
  public loggedIn : boolean = false;
  constructor(
    private router: Router,
    private http: HttpClient,
  ) {
    this.userSubject = new BehaviorSubject<User>(
      JSON.parse(String(localStorage.getItem('currentUser')))
    );
    this.user = this.userSubject.asObservable();
  }

  public get userValue(): User {
    return this.userSubject.value;
  }

   login(username: string, password: string) {
    console.log("logging in");
    
    const post =  this.http
    .post('https://localhost:7084/api/Auth/login', {
       "Username":username,
      "Password": password,
    },{headers: {
      'Access-Control-Allow-Origin': '*',
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    }});;
    
    post.toPromise().then((data : any) => {
      console.log("ejta",data);
      const token = data.token;
      console.log("tokenche",token);
      localStorage.setItem('token', token );
      localStorage.setItem('username', username);
      this.setUserObservable(token);
      this.loggedIn = true;
      this.router.navigate(['']);
    })
    return  post;
      
  }
  setUserObservable(token :any){
    const username = localStorage.getItem('username');
    const response = this.http
    .get<User>('https://localhost:7084/api/User/GetUser/'+username,{headers: {
      'Access-Control-Allow-Origin': '*',
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization' : token
    }});
    console.log("resposne is user", response);
    this.user = response;
  }

   logout() {
    localStorage.removeItem('currentUser');
    localStorage.removeItem('username');
    localStorage.removeItem('token');
    this.userSubject.next(new User('','','',''));
    this.loggedIn = false;
    this.router.navigate(['/login']);
   }

  register(user: User) {
    console.log("registering",user.username,user.password,user);
    return this.http.post("https://localhost:7084/api/User/AddUser", user, {headers: {
      'Access-Control-Allow-Origin': '*',
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    }});
  }

  getUser(username: string) {
    const url = `https://localhost:7084/api/User/GetUser/${username}`
    return this.http.get<User>(url, {headers: {
      'Access-Control-Allow-Origin': '*',
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': String(localStorage.getItem("token"))
    }})
  }
}
