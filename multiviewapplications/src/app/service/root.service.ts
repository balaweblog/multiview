import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class RootService {

  constructor(private http: HttpClient) { }

  getfilms(): Promise<any> {
    return  this.http.get(`https://ghibliapi.herokuapp.com/films`).toPromise()
   .then(res =>  res as any).catch(this.handleErrorPromise);
  }
  getpeople(): Promise<any> {
    return  this.http.get(`https://ghibliapi.herokuapp.com/people`).toPromise()
    .then(res =>  res as any).catch(this.handleErrorPromise);
 }
 getlocations(): Promise<any> {
    return  this.http.get(`https://ghibliapi.herokuapp.com/locations`).toPromise()
    .then(res =>  res as any).catch(this.handleErrorPromise);
 }

 private handleErrorPromise (error: Response | any) {
  return Promise.reject(error.message || error);
 }
}
