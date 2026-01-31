import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DevelopingView } from './developing-view';

describe('DevelopingView', () => {
  let component: DevelopingView;
  let fixture: ComponentFixture<DevelopingView>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DevelopingView]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DevelopingView);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
