import { User } from './../_models/user';
import { HttpClient, HttpHeaderResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';

/*const httpOptions = {
    headers: new HttpHeaders({
       Authorization: 'Bearer ' + localStorage.getItem('token')
    })
};*/

@Injectable({
  providedIn: 'root'
})
export class UserService {

  baseUrl = environment.apiUrl;

constructor(private http: HttpClient) { }

  // moram ubaciti Barear jwt Token
  // da bi Server znao s kim prica

  getUsers(): Observable<User[]> {

      return this.http.get<User[]>(this.baseUrl + 'users');
  }
 getUser(id ): Observable<User> {
   return this.http.get<User>(this.baseUrl + 'users/' + id);
 }
 updateUser(id: number, user: User) {
    return this.http.put( this.baseUrl + 'users/' + id, user);
 }
 setMainPhoto(userId: number, id: number) {
   return this.http.post(this.baseUrl + 'users/' + userId + '/photos/' + id + '/setMain', {});
 }
 deletePhoto( userId: number, id: number) {
  return this.http.delete(this.baseUrl + 'users/' + userId + '/photos/' + id);
 }
}