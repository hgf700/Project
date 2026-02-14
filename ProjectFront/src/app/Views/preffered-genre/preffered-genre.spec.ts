import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PrefferedGenre } from './preffered-genre';

describe('PrefferedGenre', () => {
  let component: PrefferedGenre;
  let fixture: ComponentFixture<PrefferedGenre>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PrefferedGenre]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PrefferedGenre);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
