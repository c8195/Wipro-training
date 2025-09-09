import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Postcomponent } from './postcomponent';

describe('Postcomponent', () => {
  let component: Postcomponent;
  let fixture: ComponentFixture<Postcomponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Postcomponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Postcomponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
