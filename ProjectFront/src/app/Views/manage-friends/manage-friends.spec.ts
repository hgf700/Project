import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageFriends } from './manage-friends';

describe('ManageFriends', () => {
  let component: ManageFriends;
  let fixture: ComponentFixture<ManageFriends>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ManageFriends]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManageFriends);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
