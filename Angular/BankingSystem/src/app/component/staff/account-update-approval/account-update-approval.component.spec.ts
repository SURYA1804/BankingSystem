import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccountUpdateApprovalComponent } from './account-update-approval.component';

describe('AccountUpdateApprovalComponent', () => {
  let component: AccountUpdateApprovalComponent;
  let fixture: ComponentFixture<AccountUpdateApprovalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AccountUpdateApprovalComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AccountUpdateApprovalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
