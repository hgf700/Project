import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RedisRetiveData } from './redis-retive-data';

describe('RedisRetiveData', () => {
  let component: RedisRetiveData;
  let fixture: ComponentFixture<RedisRetiveData>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RedisRetiveData]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RedisRetiveData);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
