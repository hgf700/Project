import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateOrAddToPlaylistComponent } from './create-or-add-to-playlist.component';

describe('CreateOrAddToPlaylistComponent', () => {
  let component: CreateOrAddToPlaylistComponent;
  let fixture: ComponentFixture<CreateOrAddToPlaylistComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateOrAddToPlaylistComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateOrAddToPlaylistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
