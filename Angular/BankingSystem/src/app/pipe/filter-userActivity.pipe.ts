import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'appFilter' })
export class UserActivityFilterPipe implements PipeTransform {
  transform(items: any[], searchText: string): any[] {
    if (!items) return [];
    if (!searchText) return items;

    searchText = searchText.toLowerCase();
    return items.filter(it => it.userName.toLowerCase().includes(searchText));
  }
}
