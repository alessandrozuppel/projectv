import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AutobusMapComponent } from './autobus-map.component';

describe('AutobusMapComponent', () => {
  let component: AutobusMapComponent;
  let fixture: ComponentFixture<AutobusMapComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AutobusMapComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AutobusMapComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
