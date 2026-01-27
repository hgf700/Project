import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlaylistSubWindowComponent } from './playlist-sub-window.component';

describe('PlaylistSubWindowComponent', () => {
  let component: PlaylistSubWindowComponent;
  let fixture: ComponentFixture<PlaylistSubWindowComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PlaylistSubWindowComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PlaylistSubWindowComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
