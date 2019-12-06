import { Component, OnInit, Input } from '@angular/core';
import { Autobus } from '../autobus';

@Component({
  selector: 'app-autobus-list',
  templateUrl: './autobus-list.component.html',
  styleUrls: ['./autobus-list.component.scss']
})
export class AutobusListComponent implements OnInit {

  @Input()
  autobusList: Autobus[] = [];

   step: any = 0;
  
  constructor() { }

  ngOnInit() {
  }
  
  //List navigation
  setStep(index: number) {
    this.step = index;
  }

  nextStep() {
    this.step++;
  }

  prevStep() {
    this.step--;
  }

}
