import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MovieRecomendations } from './movie-recomendations';

describe('MovieRecomendations', () => {
  let component: MovieRecomendations;
  let fixture: ComponentFixture<MovieRecomendations>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MovieRecomendations]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MovieRecomendations);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
