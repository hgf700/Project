import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddMoviesTMDBComponent } from './add-movies-tmdb.component';

describe('AddMoviesTMDBComponent', () => {
  let component: AddMoviesTMDBComponent;
  let fixture: ComponentFixture<AddMoviesTMDBComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddMoviesTMDBComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddMoviesTMDBComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
