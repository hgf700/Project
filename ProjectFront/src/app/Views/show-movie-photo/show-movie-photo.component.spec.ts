import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowMoviePhotoComponent } from './show-movie-photo.component';

describe('ShowMoviePhotoComponent', () => {
  let component: ShowMoviePhotoComponent;
  let fixture: ComponentFixture<ShowMoviePhotoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ShowMoviePhotoComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(ShowMoviePhotoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
