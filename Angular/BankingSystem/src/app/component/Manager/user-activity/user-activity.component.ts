import { Component, OnInit } from '@angular/core';
import { ManagerService, UserActivityDto } from '../../../services/Manager/manager.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-user-activity',
  templateUrl: './user-activity.component.html',
  styleUrls: ['./user-activity.component.css'],
  imports:[FormsModule,CommonModule]
})
export class UserActivityComponent implements OnInit {
  users: UserActivityDto[] = [];
  filteredUsers: UserActivityDto[] = [];
  pageOfUsers: UserActivityDto[] = [];
  Math = Math;
  searchText: string = '';
  selectedUser: UserActivityDto | null = null;

  pageSize = 5;
  currentPage = 1;
totalPages(): number {
  return Math.ceil(this.filteredUsers.length / this.pageSize);
}

  constructor(private managerService: ManagerService) {}

  ngOnInit() {
    this.loadUsers();
  }

  loadUsers() {
    this.managerService.getAllUsersActivity().subscribe(users => {
      this.users = users;
      this.applyFilterAndPagination();
    });
  }

  applyFilterAndPagination() {
    this.filteredUsers = this.searchText ? 
      this.users.filter(u => u.userName.toLowerCase().includes(this.searchText.toLowerCase())) 
      : this.users;

    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.pageOfUsers = this.filteredUsers.slice(startIndex, endIndex);
  }

  onSearchChange() {
    this.currentPage = 1; 
    this.applyFilterAndPagination();
  }

  onPageChange(newPage: number) {
    this.currentPage = newPage;
    this.applyFilterAndPagination();
  }

  selectUser(user: UserActivityDto) {
    this.selectedUser = user;
  }

  clearSelection() {
    this.selectedUser = null;
  }
}
