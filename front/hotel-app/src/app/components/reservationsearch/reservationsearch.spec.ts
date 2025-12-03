import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Reservationsearch } from './reservationsearch';

describe('Reservationsearch', () => {
  let component: Reservationsearch;
  let fixture: ComponentFixture<Reservationsearch>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Reservationsearch]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Reservationsearch);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
