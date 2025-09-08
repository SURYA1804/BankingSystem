import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OTPVerficationRequiredComponent } from './otpverfication-required.component';

describe('OTPVerficationRequiredComponent', () => {
  let component: OTPVerficationRequiredComponent;
  let fixture: ComponentFixture<OTPVerficationRequiredComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OTPVerficationRequiredComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(OTPVerficationRequiredComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
