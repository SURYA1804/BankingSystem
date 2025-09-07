declare module 'bootstrap' {
export class Offcanvas {
constructor(element: any, options?: any);
show(): void;
hide(): void;
static getInstance(element: any): Offcanvas | null;
}
}