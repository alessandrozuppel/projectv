import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AutobusListComponent } from './autobus-list.component';

describe('AutobusListComponent', () => {
  let component: AutobusListComponent;
  let fixture: ComponentFixture<AutobusListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AutobusListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AutobusListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
