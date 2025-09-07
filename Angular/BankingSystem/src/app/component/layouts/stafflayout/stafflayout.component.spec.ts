import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StafflayoutComponent } from './stafflayout.component';

describe('StafflayoutComponent', () => {
  let component: StafflayoutComponent;
  let fixture: ComponentFixture<StafflayoutComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StafflayoutComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StafflayoutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
