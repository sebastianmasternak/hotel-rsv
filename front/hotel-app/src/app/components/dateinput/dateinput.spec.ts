import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Dateinput } from './dateinput';

describe('Dateinput', () => {
  let component: Dateinput;
  let fixture: ComponentFixture<Dateinput>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Dateinput]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Dateinput);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
