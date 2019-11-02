import { map } from 'rxjs/operators';
import { PaginatedResult } from './../_models/pagination';
import { User } from './../_models/user';
import { HttpClient, HttpHeaderResponse, HttpHeaders, HttpParams } from '@angular/common/http';
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

  getUsers(page?, itemsPerPage?, userParams?): Observable<PaginatedResult<User[]>> {
    const paginatedReuslt: PaginatedResult<User[]> = new PaginatedResult<User[]>();
    let params = new HttpParams();

    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }
    if (userParams != null) {
      params = params.append('minAge', userParams.minAge);
      params = params.append('maxAge', userParams.maxAge);
      params = params.append('gender', userParams.gender);
      params = params.append('orderBy', userParams.orderBy);
    }
    return this.http.get<User[]>(this.baseUrl + 'users', {observe: 'response', params})
    .pipe(
      map(responce => {
        paginatedReuslt.result = responce.body;
        if (responce.headers.get('Pagination') != null ) {
          paginatedReuslt.pagination = JSON.parse(responce.headers.get('Pagination'))
        }
        return paginatedReuslt;
      })
    );
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
