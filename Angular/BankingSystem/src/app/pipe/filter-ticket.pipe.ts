import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'ticketFilter', standalone: true })
export class TicketFilterPipe implements PipeTransform {
  transform<T extends { queryType?: string; status?: string; priority?: string }>(items: T[] | null | undefined, q: string | null | undefined): T[] {
    if (!items) return [];
    if (!q) return items;
    const s = q.toLowerCase();
    return items.filter(x =>
      (x.queryType || '').toLowerCase().includes(s) ||
      (x.status || '').toLowerCase().includes(s) ||
      (x.priority || '').toLowerCase().includes(s)
    );
  }
}
