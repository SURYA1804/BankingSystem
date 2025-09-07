import { TestBed } from '@angular/core/testing';

import { CustomerSupportService } from './customer-support.service';

describe('CustomerSupportService', () => {
  let service: CustomerSupportService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CustomerSupportService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
