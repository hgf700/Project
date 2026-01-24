import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SeedGenreComponent } from './seed-genre.component';

describe('SeedGenreComponent', () => {
  let component: SeedGenreComponent;
  let fixture: ComponentFixture<SeedGenreComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SeedGenreComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SeedGenreComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
