export interface IAccountSummary {
  id: number;
  accountNumber: number;
  userId: number;
  accountType: string;
  balance: number;
  status: 'ACTIVE'|'CLOSED'|'PENDING';
  createdAt: string;
}