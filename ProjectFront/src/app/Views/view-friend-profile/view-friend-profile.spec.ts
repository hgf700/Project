import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewFriendProfile } from './view-friend-profile';

describe('ViewFriendProfile', () => {
  let component: ViewFriendProfile;
  let fixture: ComponentFixture<ViewFriendProfile>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ViewFriendProfile],
    }).compileComponents();

    fixture = TestBed.createComponent(ViewFriendProfile);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
