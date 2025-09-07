import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LoanSupportComponent } from './loan-support.component';

describe('LoanSupportComponent', () => {
  let component: LoanSupportComponent;
  let fixture: ComponentFixture<LoanSupportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LoanSupportComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LoanSupportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
