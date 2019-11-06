import { Message } from './../_models/message';
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

  getUsers(page?, itemsPerPage?, userParams?, likesParam?): Observable<PaginatedResult<User[]>> {
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
    if (likesParam === 'Likers') {
      params = params.append('likers', 'true');

    }
    if (likesParam === 'Likees') {
      params = params.append('likees', 'true');

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

 sendLike( id: number, recipentId: number) {
   return this.http.post( this.baseUrl + 'users/' + id + '/like/' + recipentId, {});
 }
 getMessages(id: number, page?, itemsPerPage? , messageContainer?) {

  const paginatedResult: PaginatedResult<Message[]> = new PaginatedResult<Message[]>();

  let params = new HttpParams();

  params = params.append('MessageContainer', messageContainer);

  if (page != null && itemsPerPage != null) {
    params = params.append('pageNumber', page);
    params = params.append('pageSize', itemsPerPage);
  }
  return this.http.get<Message[]>(this.baseUrl + 'users/' + id + '/message', {observe: 'response', params})
                   .pipe(
                   map(responce => {
                    paginatedResult.result = responce.body;
                    if ( responce.headers.get('Pagination' ) !== null) {
                      paginatedResult.pagination = JSON.parse(responce.headers.get('Pagination'));
                     }
                    return paginatedResult;
                   })
                   );
}
getMessageThread(id: number, recepientId: number) {
  return this.http.get<Message[]>(this.baseUrl + 'users/' + id + '/message/thread/' + recepientId);

}
sendMessage(id: number, message: Message) {
 return this.http.post( this.baseUrl + 'users/' + id + '/message', message);

}
  deleteMessage(id: number, userId: number) {
    return this.http.post( this.baseUrl + 'users/' + userId + '/message/' + id, {});
  }
  markAsRead(userId: number, messageId: number) {
    this.http.post( this.baseUrl + 'users/' + userId + '/message/' + messageId +'/read', {})
              .subscribe();
  }
}
