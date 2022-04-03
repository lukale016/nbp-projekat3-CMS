import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { swapSlashes } from 'src/app/helpers/swapSlashes';
import { Folder } from 'src/app/models/folder';
import { FolderItem } from 'src/app/models/folderItem';
import { User } from 'src/app/models/user';

const baseUrl = 'https://localhost:7068/api/' ;
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
  public currentFolder :Folder;
  
  constructor(
    private router: Router,
    private http: HttpClient,
  ) {
    this.userSubject = new BehaviorSubject<User>(
      JSON.parse(String(localStorage.getItem('currentUser')))
    );
    this.user = this.userSubject.asObservable();
    this.currentFolder = new Folder('',[]);
  }

  public get userValue(): User {
    return this.userSubject.value;
  }

   login(username: string, password: string) {
    console.log("logging in");
    
    const bodyLogin = {
          "Username": username,
          "Password": password
        }
    const urlLogin = baseUrl + service.Auth + "/login";
    return this.http.post(urlLogin, bodyLogin, {headers: {
        'Access-Control-Allow-Origin': '*',
        'Content-Type': 'application/json',
        'Accept': 'application/json'
      }});
  }
  setUserObservable(token :any){
    const username = localStorage.getItem('username');
    const response = this.http
    .get<User>('https://localhost:7068/api/User/GetUser/'+username,{headers: {
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
    // this.userSubject.next(new User('','','',''));
    this.loggedIn = false;
    this.user = of();
    this.router.navigate(['/login']);
   }

  register(user: User) {
    console.log("registering",user.username,user.password,user);
    return this.http.post("https://localhost:7068/api/User/AddUser", user, {headers: {
      'Access-Control-Allow-Origin': '*',
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    }});
  }

  getUser(username: string) {
    const url = `https://localhost:7068/api/User/GetUser/${username}`
    return this.http.get<User>(url, {headers: {
      'Access-Control-Allow-Origin': '*',
      'Content-Type': 'application/json',
      'Accept': 'application/json',
    }})
  }

  setUser(user :User){
    this.user = of(user);
  } 
  
  setCurrentFolder(folder :Folder){
    this.currentFolder = folder;
  }



  getFolderContents(path :string){
    const url = `https://localhost:7068/api/Folder/FolderContent`;
    if(path[path.length-1] === '/') 
      path = path.slice(0,path.length-1);
    path = swapSlashes(path);
    return this.http.post(url, `"${path}"`, 
    {headers: {
      'Access-Control-Allow-Origin': '*',
      'Content-Type': 'application/json',
      'Accept': 'application/json',
    }})
    //folders
    //files
  }
  createNewFolder(name :string, path :string){
    let parentPath = path;
    if(parentPath[parentPath.length-1] === '/') 
      parentPath = path.slice(0,path.length-1);
    parentPath = swapSlashes(parentPath);
    console.log("createNewFDolder34333", parentPath, name, path);
    console.log("PARENTPATH IS ",parentPath, parentPath.includes('\\'), path)
    const url = `https://localhost:7068/api/Folder/CreateFolder`;
    // swapSlashes(parentPath);
    return this.http.post(url, parentPath, 
    {headers: {
      'Access-Control-Allow-Origin': '*',
      'Content-Type': 'application/json',
      'Data-Type': 'text/plain',
      'Accept': 'application/json',
    }})
  }

  updateFolder(oldPath :string, newName :string){
    console.log("updateFOlder", oldPath, newName)
    const parentPath = oldPath.slice(oldPath.length-1);
    const url = `https://localhost:7068/api/Folder/UpdateFolder`;
    return this.http.put(url, {oldPath: oldPath, newName : newName }, 
    {headers: {
      'Access-Control-Allow-Origin': '*',
      'Content-Type': 'application/json',
      'Accept': 'application/json',
    }})
  }

  deleteFolder(path :string){
    console.log("deleteFolder", path)
    // const parentPath = oldPath.slice(oldPath.length-1);
    const url = `https://localhost:7068/api/Folder/UpdateFolder`;
    return this.http.delete(url, {headers: {
      'Access-Control-Allow-Origin': '*',
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      },
    body: `"${path}"`,
    });
  }

  createNewFile(file :any, path :string){
    let parentPath = path.substring(0,path.length-1);
    parentPath = swapSlashes(parentPath);

    console.log("create file", parentPath, path, file);
    const url = `https://localhost:7068/api/File/StoreFile`;
    return this.http.post(url, file, 
    {headers: {
      // 'Access-Control-Allow-Origin': '*',
      'Content-Disposition': 'multipart/form-data',
      // 'Accept': '*',
      'X_ParentPath': parentPath,
    },
    // responseType: "text"
  })
  }

  readFile(fileName :string, path :string){
    let parentPath = path;
    parentPath = swapSlashes(parentPath);
    console.log("parent path read file", parentPath, fileName, path);
    const url = `https://localhost:7068/api/File/ReadFile`;
    return this.http.post(url, {fileName: fileName, parent: parentPath}, 
    { headers: {
        'Access-Control-Allow-Origin': '*',
        'Content-Type': 'image/png',
        'Accept': '*',
      },
      responseType: 'image/png'
     })
  }

}
