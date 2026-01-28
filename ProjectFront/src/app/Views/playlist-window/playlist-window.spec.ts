import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlaylistWindow } from './playlist-window';

describe('PlaylistWindow', () => {
  let component: PlaylistWindow;
  let fixture: ComponentFixture<PlaylistWindow>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PlaylistWindow]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PlaylistWindow);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
