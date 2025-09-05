export interface IUser {
  userId: number;
  email: string;
  name: string;
  userName: string;
  age: number;
  phoneNumber: string;
  dob: string; 

  customerTypeId: number;
  customerTypeName?: string;

  roleId: number;
  roleName?: string;

  address?: string;
  createdAt: string; 
  lastLoginAt: string;
  isVerified: boolean;
  isEmployed: boolean;
}
