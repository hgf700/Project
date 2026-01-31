import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SubSharePlaylistWindow } from './sub-share-playlist-window';

describe('SubSharePlaylistWindow', () => {
  let component: SubSharePlaylistWindow;
  let fixture: ComponentFixture<SubSharePlaylistWindow>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SubSharePlaylistWindow]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SubSharePlaylistWindow);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
