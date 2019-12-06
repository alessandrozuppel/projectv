import { Component } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Autobus } from './autobus';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'WheelWare';
  url: string = "http://localhost:3000/list";
  hasLoaded: boolean = false;
  data: Autobus[] = [];


  constructor(private http: HttpClient) {

  }
  async ngOnInit(): Promise<void> {
    //Called after the constructor, initializing input properties, and the first call to ngOnChanges.
    //Add 'implements OnInit' to the class.
    //Recupero dati
    await fetch(this.url)
      .then((res: any) => {
        return res.json().then((data) => {
          console.log(data.result);
          this.data = data.result;
        }).catch((err) => {
          console.log(err);
        })
      });
    console.log(this.data);
    setTimeout(() => {
      this.hasLoaded = true;
    }, 1000);
  }
}
