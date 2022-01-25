import { Injectable } from '@angular/core';
import { Game } from './models/game';
import { Observable, of } from 'rxjs';
import { HttpClient, HttpHeaders} from '@angular/common/http';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class GameService {
  private _baseURL = 'https://localhost:7057/api/v1/Game/';
  constructor(private http: HttpClient) { }

  getGames(): Observable<Array<Game>>{
    let url = "getAll"
    return this.http.get<Array<Game>>(this._baseURL + url)
        .pipe(
          catchError(this.handleError<Array<Game>>('getGames', []))
        );
  }

  getResult(games: Array<Game>): Observable<Array<Game>>{
    let url = "result"
    return this.http.post<Array<Game>>(this._baseURL + url, games)
      .pipe(
        catchError(this.handleError<Array<Game>>('getResult', []))
      );
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(error);
      return of(result as T);
    };
  }
}
