import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-autobus-map',
  templateUrl: './autobus-map.component.html',
  styleUrls: ['./autobus-map.component.scss']
})
export class AutobusMapComponent implements OnInit {

  @Input()
  coordinates: Coordinates[] = [];
  zoom: number = 15;
  constructor() { }

  ngOnInit() {
  }

  
}
export class Coordinates{
    lat: number;
    long: number;
}