import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StaffCustomerSupportComponent } from './staff-customer-support.component';

describe('StaffCustomerSupportComponent', () => {
  let component: StaffCustomerSupportComponent;
  let fixture: ComponentFixture<StaffCustomerSupportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StaffCustomerSupportComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StaffCustomerSupportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
